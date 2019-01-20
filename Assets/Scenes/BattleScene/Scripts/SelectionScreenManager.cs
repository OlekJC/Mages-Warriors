using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectionScreenManager : MonoBehaviour
{
    //private const int NumberOfHeroes = 6;
    private Button selectedButton;
    private Button heroButton;
    private const string DefaultName = "Hero";

    public Button[] selectionButtons;
    public Button[] heroButtons;

    public Button playButton;

	// Use this for initialization
	void Start () {
        foreach (var btn in selectionButtons)
        {
            btn.GetComponentInChildren<Text>().text = DefaultName;
            //btn.GetComponentInChildren<Toggle>().isOn = false;
            var btn1 = btn;
            btn.onClick.AddListener(() =>
            {
                selectedButton = btn1;
                Debug.Log("Pressed "+selectedButton.name);
            });
        }

        foreach (var btn in heroButtons)
        {
            var btn1 = btn;
            btn.onClick.AddListener(() =>
            {
                if (selectedButton != null)
                    selectedButton.GetComponentInChildren<Text>().text = btn1.GetComponentInChildren<Text>().text;
            });
        }

        playButton.onClick.AddListener(() =>
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1));
        StartCoroutine(CheckPlay());
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private IEnumerator CheckPlay()
    {
        yield return new WaitForSeconds(1f);
        bool allHeroesSelected = false;
        int buttonsSelected = 0;
        while (!allHeroesSelected)
        {
            foreach (var btn in selectionButtons)
            {
                string btnText = btn.GetComponentInChildren<Text>().text;
                if (btnText != DefaultName)
                    buttonsSelected++;
            }

            if (buttonsSelected == HeroLoadingData.NumberOfHeroes)
                allHeroesSelected = true;
            buttonsSelected = 0;
            yield return new WaitForSeconds(0.1f);
        }

        SaveHeroes();
        
        Debug.Log("All heroes selected");
        playButton.interactable = true;
    }

    private void SaveHeroes()
    {
        int heroLoadingDataIterator = 0;
        foreach (var btn in selectionButtons)
        {
            string btnText = btn.GetComponentInChildren<Text>().text;
            HeroLoadingData.heroLoadingData[heroLoadingDataIterator++] = btnText;
        }
    }
}
