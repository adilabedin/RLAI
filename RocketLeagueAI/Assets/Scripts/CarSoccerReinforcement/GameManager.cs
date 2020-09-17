using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class PlayStates
{
    public int playerIndx;
    [FormerlySerializedAs("agentRB")]
    public Rigidbody CarAgent;
    public Vector3 startingPosition;
    public CarAgent CarAgentScript;
    public float ballPosReward;
}

public class GameManager : MonoBehaviour
{

    public GameObject ball;
    [FormerlySerializedAs("ballRB")]
    [HideInInspector]
    public Rigidbody ballRb;
    SoccerBall m_BallController;
    public List<PlayStates> playStates = new List<PlayStates>();
    [HideInInspector]
    public Vector3 ballStartingPos;
    [HideInInspector]
    public bool canResetBall;

    EnvironmentParameters m_ResetParams;

    void Awake()
    {
        canResetBall = true;
        ballRb = ball.GetComponent<Rigidbody>();
        m_BallController = ball.GetComponent<SoccerBall>();
        m_BallController.area = this;
        ballStartingPos = ball.transform.position;

        m_ResetParams = Academy.Instance.EnvironmentParameters;
    }

    public static GameManager gameManager;


    public void GoalTouched(CarAgent.Team scoredTeam)
    {
        foreach (var ps in playStates)
        {
            if (ps.CarAgentScript.team == scoredTeam)
            {
                print(scoredTeam + "Scored");
                ps.CarAgentScript.AddReward(1 + ps.CarAgentScript.timePenalty);
            }
            else
            {
                ps.CarAgentScript.AddReward(-1);
            }
            ps.CarAgentScript.EndEpisode();

        }
    }

    public void ResetBall()
    {
        ball.transform.position = ballStartingPos;
        ballRb.velocity = Vector3.zero;
        ballRb.angularVelocity = Vector3.zero;
    }
}

