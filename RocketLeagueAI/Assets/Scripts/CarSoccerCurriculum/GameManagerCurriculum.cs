using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//Create our new type of event... It will trigger when we score a goal
[System.Serializable]
public class GoalEvent : UnityEvent<int> { }

public class GameManagerCurriculum : MonoBehaviour
{
    //store the score. Whenever we score a goal this number will go up by 1
    public int score = 0;

    //store a reference to the GameManager in the scene. (Lookup "sigletons" for more info)
    public static GameManagerCurriculum gameManager;


    //store a reference to all the things we need to tell when a goal is scored
    public GoalEvent OnGoalScored;

    //At the very start of the game...
    void Awake()
    {
        //store a reference to the GameManager in the scene. (Lookup "sigletons" for more info)
        if (gameManager == null)
        {
            gameManager = this;
        }
    }

    //When we are told a goal is scored...
    public void GoalScored()
    {
        //increase the score value by 1
        score++;

        //tell everyone else that a goal was scored.
        OnGoalScored.Invoke(score);
    }
}

