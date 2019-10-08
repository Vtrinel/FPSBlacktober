using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using NaughtyAttributes;

public class AutomaticGunScriptLPFP : MonoBehaviour
{

    //Animator component attached to weapon
    Animator anim;

    [Header("Gun Camera")]
    //Main gun camera
    public Camera gunCamera;

    [Header("Gun Camera Options")]
    public bool canAim = true;
    //How fast the camera field of view changes when aiming 
    [Tooltip("How fast the camera field of view changes when aiming.")]
    public float fovSpeed = 15.0f;
    //Default camera field of view
    [Tooltip("Default value for camera field of view (40 is recommended).")]
    public float defaultFov = 40.0f;

    public float aimFov = 25.0f;


    [Header("Weapon Sway")]
    //Enables weapon sway
    [Tooltip("Toggle weapon sway.")]
    public bool weaponSway;

    public float swayAmount = 0.02f;
    public float maxSwayAmount = 0.06f;
    public float swaySmoothValue = 4.0f;

    private Vector3 initialSwayPosition;

    //Used for fire rate
    private float lastFired;

    [Header("Weapon Settings")]

    [Tooltip("How fast the weapon fires, higher value means faster rate of fire.")] public float fireRate;

    [Tooltip("Enables auto reloading when out of ammo.")] public bool autoReload;

    [Tooltip("Delay between shooting last bullet and reloading")] public float autoReloadDelay = 0.25f;

    [SerializeField,Required] WeaponData m_weaponData;
    [SerializeField] LayerMask m_targetLayer;
    [SerializeField,Required] GameObject m_impactFx;
    private bool isReloading; //Check if reloading

    //Holstering weapon
    private bool hasBeenHolstered = false;
    //If weapon is holstered
    private bool holstered;
    //Check if running
    private bool isRunning;
    //Check if aiming
    private bool isAiming;
    //Check if walking
    private bool isWalking;
    //Check if inspecting weapon
    private bool isInspecting;

    //How much ammo is currently left
    [SerializeField] private int currentAmmo;
    [Tooltip("How much ammo the weapon should have.")] [SerializeField] private int ammo;
    private bool outOfAmmo;
    [SerializeField] private int ammoStock;

    [Header("Bullet Settings")]
    //Bullet
    [Tooltip("How much force is applied to the bullet when shooting.")]
    public float bulletForce = 400.0f;
    [Tooltip("How long after reloading that the bullet model becomes visible " +
        "again, only used for out of ammo reload animations.")]
    public float showBulletInMagDelay = 0.6f;
    [Tooltip("The bullet model inside the mag, not used for all weapons.")]
    public SkinnedMeshRenderer bulletInMagRenderer;

    [Header("Grenade Settings")]
    public float grenadeSpawnDelay = 0.35f;
    [SerializeField] int m_grenadeStock = 3;

    [Header("Muzzleflash Settings")]
    public bool randomMuzzleflash = false;
    //min should always bee 1
    private int minRandomValue = 1;

    [Range(2, 25)]
    public int maxRandomValue = 5;

    private int randomMuzzleflashValue;

    public bool enableMuzzleflash = true;
    public ParticleSystem muzzleParticles;
    public bool enableSparks = true;
    public ParticleSystem sparkParticles;
    public int minSparkEmission = 1;
    public int maxSparkEmission = 7;

    [Header("Muzzleflash Light Settings")]
    public Light muzzleflashLight;
    public float lightDuration = 0.02f;

    [Header("Audio Source")]
    //Main audio source
    public AudioSource mainAudioSource;
    //Audio source used for shoot sound
    public AudioSource shootAudioSource;

    [System.Serializable]
    public class prefabs
    {
        [Header("Prefabs")]
        public Transform bulletPrefab;
        public Transform casingPrefab;
        public Transform grenadePrefab;
    }
    public prefabs Prefabs;
    public GameObject objToDisableOnGameOver;

    [System.Serializable]
    public class spawnpoints
    {
        [Header("Spawnpoints")]
        //Array holding casing spawn points 
        //(some weapons use more than one casing spawn)
        //Casing spawn point array
        public Transform casingSpawnPoint;
        //Bullet prefab spawn from this point
        public Transform bulletSpawnPoint;

        public Transform grenadeSpawnPoint;
    }
    public spawnpoints Spawnpoints;

    [System.Serializable]
    public class soundClips
    {
        public AudioClip shootSound;
        public AudioClip takeOutSound;
        public AudioClip holsterSound;
        public AudioClip reloadSoundOutOfAmmo;
        public AudioClip reloadSoundAmmoLeft;
        public AudioClip aimSound;
    }
    public soundClips SoundClips;

    private bool soundHasPlayed = false;

    public int CurrentAmmo { get => currentAmmo; private set => currentAmmo = value; }
    public int AmmoStock { get => ammoStock; private set => ammoStock = value; }
    public int Ammo { get => ammo; private set => ammo = value; }
    public int Grenade { get => m_grenadeStock; private set => m_grenadeStock = value; }
    public bool IsRunning { get => isRunning; set => isRunning = value; }

    private void Awake()
    {

        //Set the animator component
        anim = GetComponent<Animator>();
        //Set current ammo to total ammo value
        CurrentAmmo = Ammo;

        muzzleflashLight.enabled = false;
    }

    private void Start()
    {
        //Weapon sway
        initialSwayPosition = transform.localPosition;

        //Set the shoot sound to audio source
        shootAudioSource.clip = SoundClips.shootSound;
    }

    private void LateUpdate()
    {

        //Weapon sway
        if (weaponSway == true)
        {
            float movementX = -Input.GetAxis("Mouse X") * swayAmount;
            float movementY = -Input.GetAxis("Mouse Y") * swayAmount;
            //Clamp movement to min and max values
            movementX = Mathf.Clamp
                (movementX, -maxSwayAmount, maxSwayAmount);
            movementY = Mathf.Clamp
                (movementY, -maxSwayAmount, maxSwayAmount);
            //Lerp local pos
            Vector3 finalSwayPosition = new Vector3
                (movementX, movementY, 0);
            transform.localPosition = Vector3.Lerp
                (transform.localPosition, finalSwayPosition +
                    initialSwayPosition, Time.deltaTime * swaySmoothValue);
        }
    }

    private void Update()
    {
        Aim();

        //If randomize muzzleflash is true, genereate random int values
        if (randomMuzzleflash == true)
        {
            randomMuzzleflashValue = Random.Range(minRandomValue, maxRandomValue);
        }

        //Continosuly check which animation 
        //is currently playing
        AnimationCheck();

        /*
        //Play knife attack 1 animation when Q key is pressed
        if (Input.GetKeyDown(KeyCode.Q) && !isInspecting)
        {
            anim.Play("Knife Attack 1", 0, 0f);
        }
        */

        if (Input.GetKeyDown(KeyCode.F) && !isInspecting)
        {
            anim.Play("Knife Attack 2", 0, 0f);
        }



        //Throw grenade when pressing G key
        if (Input.GetKeyDown(KeyCode.G) && !isInspecting)
        {
            ThrowGrenade();
        }

        //If out of ammo
        if (CurrentAmmo == 0)
        {
            //Toggle bool
            outOfAmmo = true;
            //Auto reload if true
            if (autoReload == true && !isReloading)
            {
                StartCoroutine(AutoReload());
            }
        }
        else
        {
            //Toggle bool
            outOfAmmo = false;
            //anim.SetBool ("Out Of Ammo", false);
        }

        //AUtomatic fire
        //Left click hold 
        if (Input.GetMouseButton(0) && !outOfAmmo && !isReloading && !isInspecting && !IsRunning)
        {
            Shoot();
        }

        /*
        //Inspect weapon when T key is pressed
        if (Input.GetKeyDown(KeyCode.T))
        {
            anim.SetTrigger("Inspect");
        }
        */

        /*
        //Toggle weapon holster when E key is pressed
        if (Input.GetKeyDown(KeyCode.E) && !hasBeenHolstered)
        {
            holstered = true;

            mainAudioSource.clip = SoundClips.holsterSound;
            mainAudioSource.Play();

            hasBeenHolstered = true;
        }
        else if (Input.GetKeyDown(KeyCode.E) && hasBeenHolstered)
        {
            holstered = false;

            mainAudioSource.clip = SoundClips.takeOutSound;
            mainAudioSource.Play();

            hasBeenHolstered = false;
        }
        */

        //Holster anim toggle
        if (holstered == true)
        {
            anim.SetBool("Holster", true);
        }
        else
        {
            anim.SetBool("Holster", false);
        }

        //Reload 
        if (Input.GetKeyDown(KeyCode.R) && !isReloading && !isInspecting)
        {
            //Reload
            Reload();
        }

        //Walking when pressing down WASD keys
        if (Input.GetKey(KeyCode.W) && !IsRunning ||
            Input.GetKey(KeyCode.A) && !IsRunning ||
            Input.GetKey(KeyCode.S) && !IsRunning ||
            Input.GetKey(KeyCode.Z) && !IsRunning ||
            Input.GetKey(KeyCode.Q) && !IsRunning ||
            Input.GetKey(KeyCode.D) && !IsRunning)
        {
            anim.SetBool("Walk", true);
        }
        else
        {
            anim.SetBool("Walk", false);
        }

        /*
        //Running when pressing down W and Left Shift key
        if ((Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift)))
        {
            IsRunning = true;
        }
        else
        {
            IsRunning = false;
        }
        */

        //Run anim toggle
        if (IsRunning == true)
        {
            anim.SetBool("Run", true);
        }
        else
        {
            anim.SetBool("Run", false);
        }
    }

    private void Aim()
    {
        if (!canAim) return;

        //Aiming
        //Toggle camera FOV when right click is held down
        if (Input.GetButton("Fire2") && !isReloading && !IsRunning && !isInspecting)
        {

            isAiming = true;
            //Start aiming
            anim.SetBool("Aim", true);

            //When right click is released
            gunCamera.fieldOfView = Mathf.Lerp(gunCamera.fieldOfView,
                aimFov, fovSpeed * Time.deltaTime);

            if (!soundHasPlayed)
            {
                mainAudioSource.clip = SoundClips.aimSound;
                mainAudioSource.Play();

                soundHasPlayed = true;
            }
        }
        else
        {
            //When right click is released
            gunCamera.fieldOfView = Mathf.Lerp(gunCamera.fieldOfView,
                defaultFov, fovSpeed * Time.deltaTime);

            isAiming = false;
            //Stop aiming
            anim.SetBool("Aim", false);

            soundHasPlayed = false;
        }
        //Aiming end
    }

    private void ThrowGrenade()
    {
        if (Grenade <= 0) return;

        Grenade--;
        StartCoroutine(GrenadeSpawnDelay());
        anim.Play("GrenadeThrow", 0, 0.0f);
    }

    public void AddAmmo(int p_ammoToAdd)
    {
        AmmoStock += p_ammoToAdd;
    }

    public void AddGrenade(int p_grenadeToAdd)
    {
        Grenade += p_grenadeToAdd;
    }

    public void GameOver()
    {
        objToDisableOnGameOver.SetActive(false);
    }

    private void Shoot()
    {
        //Shoot automatic
        if (Time.time - lastFired > 1 / fireRate)
        {
            lastFired = Time.time;

            //Remove 1 bullet from ammo
            CurrentAmmo -= 1;

            shootAudioSource.clip = SoundClips.shootSound;
            shootAudioSource.Play();

            if (!isAiming) //if not aiming
            {
                anim.Play("Fire", 0, 0f);
                //If random muzzle is false
                if (!randomMuzzleflash &&
                    enableMuzzleflash == true)
                {
                    muzzleParticles.Emit(1);
                    //Light flash start
                    StartCoroutine(MuzzleFlashLight());
                }
                else if (randomMuzzleflash == true)
                {
                    //Only emit if random value is 1
                    if (randomMuzzleflashValue == 1)
                    {
                        if (enableSparks == true)
                        {
                            //Emit random amount of spark particles
                            sparkParticles.Emit(Random.Range(minSparkEmission, maxSparkEmission));
                        }
                        if (enableMuzzleflash == true)
                        {
                            muzzleParticles.Emit(1);
                            //Light flash start
                            StartCoroutine(MuzzleFlashLight());
                        }
                    }
                }
            }
            else //if aiming
            {

                anim.Play("Aim Fire", 0, 0f);

                //If random muzzle is false
                if (!randomMuzzleflash)
                {
                    muzzleParticles.Emit(1);
                    //If random muzzle is true
                }
                else if (randomMuzzleflash == true)
                {
                    //Only emit if random value is 1
                    if (randomMuzzleflashValue == 1)
                    {
                        if (enableSparks == true)
                        {
                            //Emit random amount of spark particles
                            sparkParticles.Emit(Random.Range(minSparkEmission, maxSparkEmission));
                        }
                        if (enableMuzzleflash == true)
                        {
                            muzzleParticles.Emit(1);
                            //Light flash start
                            StartCoroutine(MuzzleFlashLight());
                        }
                    }
                }
            }

            //-----------SHOOT RAYCAST
            /*
            Vector3 rayOrigin = new Vector3(0.5f, 0.5f, 0f); // center of the screen
            float rayLength = 500f;

            // actual Ray
            Ray ray = Camera.main.ViewportPointToRay(rayOrigin);

            // debug Ray
            Debug.DrawRay(ray.origin, ray.direction * rayLength, Color.green,1.5f);
            */

            Physics.Raycast(gunCamera.transform.position, gunCamera.transform.forward,out RaycastHit l_hit,m_weaponData.Range,m_targetLayer,QueryTriggerInteraction.Ignore);
            Debug.DrawRay(gunCamera.transform.position, gunCamera.transform.forward * m_weaponData.Range, Color.red,1);

            if (l_hit.collider != null)
            {
                IDamagable l_damagable = l_hit.collider.GetComponent<IDamagable>();

                if (l_damagable != null)
                {
                    l_damagable.TakeDamage(new DamageInfo(m_weaponData.Damage, l_hit.point,l_hit.normal));
                }
                else
                {
                    //Impact FX
                    Instantiate(m_impactFx, l_hit.point,Quaternion.LookRotation(l_hit.normal));
                }
            }

            GameManager.instance.CallOnPlayerShoot(new PlayerShootInfo(transform.position,Time.time));

            //Spawn casing prefab at spawnpoint
            Instantiate(Prefabs.casingPrefab,Spawnpoints.casingSpawnPoint.transform.position,Spawnpoints.casingSpawnPoint.transform.rotation);
        }
    }

    private IEnumerator GrenadeSpawnDelay()
    {

        //Wait for set amount of time before spawning grenade
        yield return new WaitForSeconds(grenadeSpawnDelay);
        //Spawn grenade prefab at spawnpoint
        Instantiate(Prefabs.grenadePrefab,
            Spawnpoints.grenadeSpawnPoint.transform.position,
            Spawnpoints.grenadeSpawnPoint.transform.rotation);
    }

    private IEnumerator AutoReload()
    {
        //Wait set amount of time
        yield return new WaitForSeconds(autoReloadDelay);
        Reload();
        /*
		if (outOfAmmo == true) 
		{
			//Play diff anim if out of ammo
			anim.Play ("Reload Out Of Ammo", 0, 0f);

			mainAudioSource.clip = SoundClips.reloadSoundOutOfAmmo;
			mainAudioSource.Play ();

			//If out of ammo, hide the bullet renderer in the mag
			//Do not show if bullet renderer is not assigned in inspector
			if (bulletInMagRenderer != null) 
			{
				bulletInMagRenderer.GetComponent
				<SkinnedMeshRenderer> ().enabled = false;
				//Start show bullet delay
				StartCoroutine (ShowBulletInMag ());
			}
		} 
		//Restore ammo when reloading
		currentAmmo = ammo;
		outOfAmmo = false;
        */
    }

    //Reload
    private void Reload()
    {

        if (CurrentAmmo == Ammo) return;
        if (AmmoStock <= 0) return;

        if (outOfAmmo == true)
        {
            //Play diff anim if out of ammo
            anim.Play("Reload Out Of Ammo", 0, 0f);

            mainAudioSource.clip = SoundClips.reloadSoundOutOfAmmo;
            mainAudioSource.Play();

            //If out of ammo, hide the bullet renderer in the mag
            //Do not show if bullet renderer is not assigned in inspector
            if (bulletInMagRenderer != null)
            {
                bulletInMagRenderer.GetComponent
                <SkinnedMeshRenderer>().enabled = false;
                //Start show bullet delay
                StartCoroutine(ShowBulletInMag());
            }
        }
        else
        {
            //Play diff anim if ammo left
            anim.Play("Reload Ammo Left", 0, 0f);

            mainAudioSource.clip = SoundClips.reloadSoundAmmoLeft;
            mainAudioSource.Play();

            //If reloading when ammo left, show bullet in mag
            //Do not show if bullet renderer is not assigned in inspector
            if (bulletInMagRenderer != null)
            {
                bulletInMagRenderer.GetComponent
                <SkinnedMeshRenderer>().enabled = true;
            }
        }
        //Restore ammo when reloading

        if (AmmoStock >= Ammo)
        {
            AmmoStock -= Ammo - CurrentAmmo;
            CurrentAmmo = CurrentAmmo + (Ammo - CurrentAmmo);
        }
        else
        {
            AmmoStock -= AmmoStock - CurrentAmmo;

            if (AmmoStock <= 0)
            {
                CurrentAmmo = CurrentAmmo + AmmoStock;
            }
            else
            {
                CurrentAmmo = CurrentAmmo + (Ammo - CurrentAmmo);
            }
        }
        outOfAmmo = false;

    }

    //Enable bullet in mag renderer after set amount of time
    private IEnumerator ShowBulletInMag()
    {

        //Wait set amount of time before showing bullet in mag
        yield return new WaitForSeconds(showBulletInMagDelay);
        bulletInMagRenderer.GetComponent<SkinnedMeshRenderer>().enabled = true;
    }

    //Show light when shooting, then disable after set amount of time
    private IEnumerator MuzzleFlashLight()
    {

        muzzleflashLight.enabled = true;
        yield return new WaitForSeconds(lightDuration);
        muzzleflashLight.enabled = false;
    }

    //Check current animation playing
    private void AnimationCheck()
    {

        //Check if reloading
        //Check both animations
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Reload Out Of Ammo") ||
            anim.GetCurrentAnimatorStateInfo(0).IsName("Reload Ammo Left"))
        {
            isReloading = true;
        }
        else
        {
            isReloading = false;
        }

        //Check if inspecting weapon
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Inspect"))
        {
            isInspecting = true;
        }
        else
        {
            isInspecting = false;
        }
    }
}