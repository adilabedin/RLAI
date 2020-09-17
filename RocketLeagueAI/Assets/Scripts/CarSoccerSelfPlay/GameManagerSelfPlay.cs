using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class PlayState
{
    public int playerIndex;
    [FormerlySerializedAs("agentRB")]
    public Rigidbody agentRb;
    public Vector3 startingPos;
    public CarAgentSelfPlay agentScript;
    public float ballPosReward;
}

public class GameManagerSelfPlay : MonoBehaviour
{

    public GameObject ball;
    [FormerlySerializedAs("ballRB")]
    [HideInInspector]
    public Rigidbody ballRb;
    SoccerBallSelfPlay m_BallController;
    public List<PlayState> playState = new List<PlayState>();
    [HideInInspector]
    public Vector3 ballStartingPos;
    [HideInInspector]
    public bool canResetBall;

    EnvironmentParameters m_ResetParams;

    void Awake()
    {
        canResetBall = true;
        ballRb = ball.GetComponent<Rigidbody>();
        m_BallController = ball.GetComponent<SoccerBallSelfPlay>();
        m_BallController.area = this;
        ballStartingPos = ball.transform.position;

        m_ResetParams = Academy.Instance.EnvironmentParameters;
    }

    public static GameManagerSelfPlay gameManager;


    public void GoalTouched(CarAgentSelfPlay.Team scoredTeam)
    {
        foreach (var ps in playState)
        {
            if (ps.agentScript.team == scoredTeam)
            {
                print(scoredTeam + "Scored");
                ps.agentScript.AddReward(1 + ps.agentScript.timePenalty);
            }
            else
            {
                ps.agentScript.AddReward(-1);
            }
            ps.agentScript.EndEpisode();

        }
    }

    public void ResetBall()
    {
        ball.transform.position = ballStartingPos;
        ballRb.velocity = Vector3.zero;
        ballRb.angularVelocity = Vector3.zero;
    }
}
