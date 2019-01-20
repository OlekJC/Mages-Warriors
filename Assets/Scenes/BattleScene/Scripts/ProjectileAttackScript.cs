using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ProjectileAttackScript : MonoBehaviour {

    private Rigidbody shell;
    protected readonly int maxProjectileDamage=30;
    protected int force = 1000;

	// Use this for initialization
	protected void Start () {
        //Destroy object after 10 seconds
        Destroy(gameObject, 10f);
        shell = GetComponent<Rigidbody>();
        shell.AddForce(transform.forward*force);
        
	}

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Heros")
        {
            Destroy(gameObject);
            Rigidbody targetRigidBody = collider.GetComponent<Rigidbody>();
            if(targetRigidBody)
            {
                targetRigidBody.AddForce(0, 200, 0);
            }

            HeroBase heroInfo = collider.GetComponent<HeroBase>();
            heroInfo.TakeDamage(maxProjectileDamage - Random.Range(0,25));
        }

    }

    // Update is called once per frame
    void Update () {}
}
