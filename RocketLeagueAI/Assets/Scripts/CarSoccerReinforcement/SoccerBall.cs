using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;

public class SoccerBall : MonoBehaviour
{

    public CarAgent agent;
    public string orangeGoalTag;
    public string blueGoalTag;

    void OnCollisionEnter(Collision col)
    {
        // Touched goal.
        if (col.gameObject.CompareTag(orangeGoalTag))
        {
            agent.ScoredAGoal();
        }
        if (col.gameObject.CompareTag(blueGoalTag))
        {
            agent.ConcededAGoal();
        }
    }
}
