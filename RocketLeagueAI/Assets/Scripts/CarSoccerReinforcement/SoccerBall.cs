using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoccerBall : MonoBehaviour
{
    [HideInInspector]
    public GameManager area;
    public string orangeGoalTag;
    public string blueGoalTag;

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag(orangeGoalTag)) 
        {
            area.GoalTouched(CarAgent.Team.Blue);
        }
        if (col.gameObject.CompareTag(blueGoalTag))
        {
            area.GoalTouched(CarAgent.Team.Orange);
        }
    }

}

