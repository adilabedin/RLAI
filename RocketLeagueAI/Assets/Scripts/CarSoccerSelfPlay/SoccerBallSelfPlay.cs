using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoccerBallSelfPlay : MonoBehaviour
{
    [HideInInspector]
    public GameManagerSelfPlay area;
    public string orangeGoalTag; 
    public string blueGoalTag; 

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag(orangeGoalTag)) //ball touched purple goal
        {
            area.GoalTouched(CarAgentSelfPlay.Team.Blue);
        }
        if (col.gameObject.CompareTag(blueGoalTag)) //ball touched blue goal
        {
            area.GoalTouched(CarAgentSelfPlay.Team.Orange);
        }
    }
}
