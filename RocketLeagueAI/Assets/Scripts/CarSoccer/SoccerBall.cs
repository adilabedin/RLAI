using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;

public class SoccerBall : MonoBehaviour
{

    public CarAgent agent;

    void OnCollisionEnter(Collision col)
    {
        // Touched goal.
        if (col.gameObject.CompareTag("Goal"))
        {
            GameManager.gameManager.GoalScored();
            agent.ScoredAGoal();
        }
    }
}
