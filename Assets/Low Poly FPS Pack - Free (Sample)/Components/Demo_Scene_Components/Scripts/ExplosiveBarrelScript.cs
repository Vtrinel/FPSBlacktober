using UnityEngine;
using System.Collections;


public class ExplosiveBarrelScript : MonoBehaviour,IDamagable
{

	float randomTime;
	bool routineStarted = false;

	//Used to check if the barrel 
	//has been hit and should explode 
	public bool explode = false;

	[Header("Prefabs")]
	//The explosion prefab
	public Transform explosionPrefab;
	//The destroyed barrel prefab
	public Transform destroyedBarrelPrefab;

	[Header("Customizable Options")]
	//Minimum time before the barrel explodes
	public float minTime = 0.05f;
	//Maximum time before the barrel explodes
	public float maxTime = 0.25f;

	[Header("Explosion Options")]
	//How far the explosion will reach
	public float explosionRadius = 5;
	//How powerful the explosion is
	public float explosionForce = 4000.0f;

    [Header("Barrel Damage")]
    [SerializeField] float m_damage = 100;
	
	private void Update () {
		//Generate random time based on min and max time values
		randomTime = Random.Range (minTime, maxTime);

		//If the barrel is hit
		if (explode == true) 
		{
			if (routineStarted == false) 
			{
				//Start the explode coroutine
				StartCoroutine(Explode());
				routineStarted = true;
			} 
		}
	}

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    private IEnumerator Explode () {
		//Wait for set amount of time
		yield return new WaitForSeconds(randomTime);

		//Spawn the destroyed barrel prefab
		Instantiate (destroyedBarrelPrefab, transform.position, 
		             transform.rotation); 

		//Explosion force
		Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, explosionRadius);

        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null) rb.AddExplosionForce(explosionForce * 5, explosionPos, explosionRadius, 3.0F);

            IDamagable damagable = hit.GetComponent<IDamagable>();

            if (damagable != null)
            {
                damagable.TakeDamage(new DamageInfo(m_damage, hit.transform.position,Vector3.zero));
            }
        }

        //Raycast downwards to check the ground tag
        RaycastHit checkGround;
		if (Physics.Raycast(transform.position, Vector3.down, out checkGround, 50))
		{
			//Instantiate explosion prefab at hit position
			Instantiate (explosionPrefab, checkGround.point, 
				Quaternion.FromToRotation (Vector3.forward, checkGround.normal)); 
		}

		//Destroy the current barrel object
		Destroy (gameObject);
	}

    public void TakeDamage(DamageInfo p_damageInfo)
    {
        StartCoroutine(Explode());
        routineStarted = true;
    }

    public void ForceExplosion()
    {
        TakeDamage(new DamageInfo(0, Vector3.zero, Vector3.zero));
    }
}
// ----- Low Poly FPS Pack Free Version -----