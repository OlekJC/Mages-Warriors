using UnityEngine;
using UnityEngine.SceneManagement;

public class startGame : MonoBehaviour {

    public void nextScene()
    {
        //Debug.Log("Loading next scene");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
