using UnityEngine;
using System.Collections;

// ----- Low Poly FPS Pack Free Version -----
public class GrenadeScript : MonoBehaviour {

	[Header("Timer")]
	//Time before the grenade explodes
	[Tooltip("Time before the grenade explodes")]
	public float grenadeTimer = 5.0f;

	[Header("Explosion Prefabs")]
	public Transform explosionPrefab;

	[Header("Explosion Options")]

	[Tooltip("The radius of the explosion force")] public float radius = 25.0F;
	[Tooltip("The intensity of the explosion force")] public float power = 350.0F;
	[Tooltip("The damage point of the explosion")] public float damage = 1;

	[Header("Throw Force")]
	[Tooltip("Minimum throw force")]
	public float minimumForce = 1500.0f;
	[Tooltip("Maximum throw force")]
	public float maximumForce = 2500.0f;
	private float throwForce;

	[Header("Audio")]
	public AudioSource impactSound;

	private void Awake () 
	{
		//Generate random throw force
		//based on min and max values
		throwForce = Random.Range
			(minimumForce, maximumForce);

		//Random rotation of the grenade
		GetComponent<Rigidbody>().AddRelativeTorque 
		   (Random.Range(500, 1500), //X Axis
			Random.Range(0,0), 		 //Y Axis
			Random.Range(0,0)  		 //Z Axis
			* Time.deltaTime * 5000);
	}

	private void Start () 
	{
		//Launch the projectile forward by adding force to it at start
		GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * throwForce);

		//Start the explosion timer
		StartCoroutine (ExplosionTimer ());
	}

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    private void OnCollisionEnter (Collision collision) 
	{
		//Play the impact sound on every collision
		impactSound.Play ();
	}

	private IEnumerator ExplosionTimer () 
	{
		//Wait set amount of time
		yield return new WaitForSeconds(grenadeTimer);

		//Raycast downwards to check ground
		if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit checkGround, 50))
		{
			//Instantiate metal explosion prefab on ground
			Instantiate (explosionPrefab, checkGround.point, Quaternion.FromToRotation (Vector3.forward, checkGround.normal)); 
		}

		//Explosion force
		Vector3 explosionPos = transform.position;
		//Use overlapshere to check for nearby colliders

		Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);

		foreach (Collider hit in colliders)
        {
			Rigidbody rb = hit.GetComponent<Rigidbody> ();

			if (rb != null) rb.AddExplosionForce (power * 5, explosionPos, radius, 3.0F);

            IDamagable damagable = hit.GetComponent<IDamagable>();

            if(damagable != null)
            {
                damagable.TakeDamage(new DamageInfo(damage, hit.transform.position,Vector3.zero));
            }
		}

        GameManager.instance.CallOnPlayerShoot(new PlayerShootInfo(transform.position, Time.time));

		//Destroy the grenade object on explosion
		Destroy (gameObject);
	}
}