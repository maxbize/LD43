using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour {

    // Set in editor
    public GameObject endGameScreen;
    public Text endGameText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void EndGame() {
        endGameScreen.SetActive(true);
        int scrapDestroyed = StatsManager.meleeEnemiesDied + StatsManager.rangedEnemiesDied + StatsManager.trapsDestroyed;
        if (scrapDestroyed == 0) {
            endGameText.text = "You did not sacrifice any of your minions to protect yourself. You are" +
                " a benevolent king.\nBut\n<b>Sacrifices must be made!</b>";
        } else {
            endGameText.text = string.Format("You sacrificed {0} of your minions to clean up {1} " +
                "pieces of scrap so that you could live {2} seconds longer.\n<b>Was it worth it?</b>",
                StatsManager.minionsDied, scrapDestroyed, (int)Time.timeSinceLevelLoad);
        }
    }

    // Called from UI
    public void RestartGame() {
        int scene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }
}
