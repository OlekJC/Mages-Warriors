using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    private enum Turn { Player1, Player2 }

    public GameObject magePrefab,warriorPrefab,necromancerPrefab,priestPrefab,druidPrefab,paladinPrefab;
    public HeroBase[] p1Heroes,p2Heroes;
    public Transform[] spawnPoints;
    public Text round;
    public Camera mainCamera;
    public Button exitButton;

    //public Button[] attackButtons;
    
    private Turn turn;
    private Turn gameWinner;
    private const float StartingTime = 3f;
    private readonly WaitForSeconds checkInterval = new WaitForSeconds(0.1f);
    private bool isWinner = false;
    private int[] alive;

    private GameObject selectedHero;
    private GameObject attack;
    
    private HeroBase attacker, target;

	// Use this for initialization
	void Start ()
    {
        StartCoroutine(GameLoop());
    }

    // Update is called once per frame
    void Update () {
        if(Input.GetMouseButtonDown(0))
            CheckSelection();
	}

    private void CheckSelection()
    {
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray,out hit))
        {
            if(hit.collider.tag == "Heros")
            {
                selectedHero = hit.collider.gameObject;
                selectedHero.GetComponent<MeshRenderer>().material.color = Color.gray;
            }
        }
    }

    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(StartingRound());
        yield return StartCoroutine(RoundLoop());
    }

    private IEnumerator StartingRound()
    {
        for (float i = StartingTime;i>0;i--)
        {
            round.text = "Round is about to start!\n" + i;
            yield return new WaitForSeconds(1);
        }
    }

    private IEnumerator RoundLoop()
    {
        InitPlayers();

        while (!isWinner)
        {
            yield return StartCoroutine(SelectHero());
            yield return StartCoroutine(SelectTarget());
            yield return StartCoroutine(CheckWinner());
        }

        exitButton.gameObject.SetActive(true);
        exitButton.onClick.AddListener(() => Application.Quit());
        round.text = "End of the game!\n"+gameWinner+" won!\n";
        
    }

    private IEnumerator EndRoundRoutine()
    {
        switch (turn)
        {
            case Turn.Player1:
                turn = Turn.Player2;
                break;
            case Turn.Player2:
                turn = Turn.Player1;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        round.text = turn + " turn!";
        yield return null;
    }

    private IEnumerator CheckWinner()
    {
        yield return new WaitForSeconds(3);

        int player1Alive = p1Heroes.Count(hero => hero != null);

        int player2Alive = p2Heroes.Count(hero => hero != null);

        if (player1Alive < 1)
        {
            gameWinner = Turn.Player2;
            isWinner = true;
        }

        if (player2Alive < 1)
        {
            gameWinner = Turn.Player1;
            isWinner = true;
        }

        yield return StartCoroutine(EndRoundRoutine());
    }

    private IEnumerator SelectTarget()
    {
        while (selectedHero == null)
        {
            yield return checkInterval;
        }

        target = selectedHero.GetComponent<HeroBase>();
        selectedHero = null;

        TourAttack();
    }

    private void TourAttack()
    {
        attacker.ProjectileAttack(target);
        
        attacker.RestoreOriginalColor();
        target.RestoreOriginalColor();

        attacker = null;
        target = null;
    }

    private IEnumerator SelectHero()
    {

        while(selectedHero == null)
        {
            yield return checkInterval;
        }

        attacker = selectedHero.GetComponent<HeroBase>();

        //int attackId = 0;
        //foreach (var button in attackButtons)
        //{
        //    button.GetComponentInChildren<Text>().text
        //        = selectedHero.GetComponent<HeroBase>().attacksNames[attackId];
        //    attackId++;
        //    //createdButton.GetComponentsInChildren<Text>().text = "New Super Cool Button Text";
        //}
        //attackButtons[0].GetComponentInChildren<Text>().text = "LOL!";

        selectedHero = null;

    }

    private void InitPlayers()
    {
        p1Heroes = new HeroBase[3];
        p2Heroes = new HeroBase[3];

        if (HeroLoadingData.heroLoadingData == null)
        {
            HeroLoadingData.heroLoadingData = new []
            {
                "Mage", "Warrior", "Necromancer", "Priest", "Druid", "Paladin"
            };

        }

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            GameObject selectedPrefab = null;
            string heroName = HeroLoadingData.heroLoadingData[i];
            switch (heroName)
            {
                case "Mage":
                    selectedPrefab=magePrefab;
                    break;
                case "Necromancer":
                    selectedPrefab=necromancerPrefab;
                    break;
                case "Warrior":
                    selectedPrefab=warriorPrefab;
                    break;
                case "Druid":
                    selectedPrefab=druidPrefab;
                    break;
                case "Priest":
                    selectedPrefab=priestPrefab;
                    break;
                case "Paladin":
                    selectedPrefab=paladinPrefab;
                    break;
            }

            GameObject hero = Instantiate(selectedPrefab, spawnPoints[i].position, spawnPoints[i].rotation);
            HeroBase _hero = hero.GetComponent<HeroBase>();
            _hero.SetGameManager(this);
            hero.name = "Hero" + i;
            if (i < 3)
            {
                hero.GetComponent<HeroBase>().SetOwner(HeroBase.Owner.Player1);
                p1Heroes[i] = _hero;
            }
            else
            {
                hero.GetComponent<HeroBase>().SetOwner(HeroBase.Owner.Player2);
                p2Heroes[i - 3] = _hero;
            }
            
            turn = Turn.Player1;
            round.text = turn + " turn!";
        }
    }
}
