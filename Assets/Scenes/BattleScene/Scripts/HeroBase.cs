using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class HeroBase : MonoBehaviour {

    public enum Owner
    {
        Player1,Player2
    }

    //public string[] attacksStrings = {"Null1","Null2","Null3"};
    //public string[] attacksNames = new string[3];

    public GameManager gameManager;

    //Components and properties
    public int health=100;
    public Slider healthSlider;
    public GameObject projectileAttackPrefab;
    public Owner owner;
    private MeshRenderer heroMesh;
    private Color initialColor;

    //Audio
    public AudioSource audioSource;
    public AudioClip hit;

    protected Transform attackPosition;
    
    //Attacks
    protected HeroBase[] enemyHeroes;
    
	void Start () {
        heroMesh = GetComponent<MeshRenderer>();
        initialColor = heroMesh.material.color;
        attackPosition = gameObject.transform.Find("AttackPosition");
        
        //attacksNames = attacksStrings;
        switch (owner)
        {
            case Owner.Player1:
                enemyHeroes = gameManager.p2Heroes;
                break;
            case Owner.Player2:
                enemyHeroes = gameManager.p1Heroes;
                break;
            default:
                enemyHeroes = null;
                break;
        }

        SetHealthUI();
    }

    private void SetHealthUI()
    {
        healthSlider.value = health;
    }


    void Update ()
    {}

    public void TakeDamage(int damage)
    {
        health -= damage;
        audioSource.Play();
        SetHealthUI();
        CheckHP();
    }

    private void CheckHP()
    {
        if (health <= 0)
        {
            Destroy(gameObject,hit.length);
        }
    }

    public void SetOwner(Owner owner)
    {
        this.owner = owner;
    }

    public void ProjectileAttack(HeroBase target)
    {
        Vector3 attackVector = target.GetComponent<Transform>().position - attackPosition.transform.position;
        Instantiate(projectileAttackPrefab, attackPosition.position, Quaternion.LookRotation(attackVector, Vector3.up));
    }

    public void SetGameManager(GameManager gm)
    {
        gameManager = gm;
    }

    public void RestoreOriginalColor()
    {
        heroMesh.material.color = initialColor;
    }


}
