using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

public class MageScript : HeroBase
{
    public string[] mageAttacksStrings = {"Fireball","Thunderstorm","Null"};
    

    //Attacks
    public GameObject fireballAttack,thunderstormAttack;

	// Use this for initialization
	void Start ()
    {
        //attacksNames = mageAttacksStrings;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ProjectileAttack(HeroBase target)
    {
        Vector3 attackVector = target.GetComponent<Transform>().position - attackPosition.transform.position;
        Instantiate(projectileAttackPrefab, attackPosition.position, Quaternion.LookRotation(attackVector, Vector3.up));
    }

    public void ThunderstormAttack()
    {
        List<GameObject> thunderstorms = new List<GameObject>();
        //int enemyAmmount = 0;
        foreach (var enemy in enemyHeroes)
        {
            if (enemy != null)
            {
                thunderstorms.Add(Instantiate(thunderstormAttack,enemy.GetComponent<Transform>().position,Quaternion.identity));
            }
        }
    }

    
}
