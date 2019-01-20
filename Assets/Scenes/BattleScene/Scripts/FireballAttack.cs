using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballAttack : ProjectileAttackScript
{
    public GameObject explosionPrefab;

    // Use this for initialization
    void Start ()
    {
        force = 1000;
        base.Start();
    }
	
	// Update is called once per frame
	//void Update () {
	//	
	//}

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Heros")
        {
            Destroy(gameObject);
            Rigidbody targetRigidBody = collider.GetComponent<Rigidbody>();
            if (targetRigidBody)
            {
                targetRigidBody.AddForce(0, 200, 0);
            }

            HeroBase heroInfo = collider.GetComponent<HeroBase>();
            heroInfo.TakeDamage(maxProjectileDamage - Random.Range(0, 10));

            Debug.Log("Collided with " + collider.name);

            var explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(explosion,2f);
            
        }
        else
        {
            Debug.Log("Collided with " + collider.name);
        }
    }
}
