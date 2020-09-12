using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;// <---- We need to include this if we want to work with UI in this code document

public class ScoreIndicator : MonoBehaviour
{
    //store a reference to our UI Text component so we can change the text on it.
    private Text scoreText;

    //At the start of the game...
    void Start()
    {
        //get our UI Text component that is on this gameobject and save it to our "scoreText" variable
        scoreText = GetComponent<Text>();
        
        //Ask the game manager to add us to the list of things to update when a goal is scored
        GameManager.gameManager.OnGoalScored.AddListener(GameManager_OnGoalScored);
    }
    
    //The GameManager will call this when a goal is scored
    void GameManager_OnGoalScored(int score)
    {
        //change the text of our UI Text to what the GameManager tells us is the score
        scoreText.text = score.ToString();
    }
}
