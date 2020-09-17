using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class PlayState
{
    public int playerIndix;
    [FormerlySerializedAs("agentRB")]
    public Rigidbody CarAgentCurriculum;
    public Vector3 startingPosition;
    public CarAgentCurriculum CarAgentScripts;
    public float ballPosReward;
}

public class GameManagerCurriculum : MonoBehaviour
{

    public GameObject ball;
    [FormerlySerializedAs("ballRB")]
    [HideInInspector]
    public Rigidbody ballRb;
    SoccerBallCurriculum m_BallController;
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
        m_BallController = ball.GetComponent<SoccerBallCurriculum>();
        m_BallController.gameArea = this;
        ballStartingPos = ball.transform.position;

        m_ResetParams = Academy.Instance.EnvironmentParameters;
    }

    public static GameManagerCurriculum gameManager;


    public void GoalTouched(CarAgentCurriculum.Team scoredTeam)
    {
        foreach (var ps in playState)
        {
            if (ps.CarAgentScripts.team == scoredTeam)
            {
                print(scoredTeam + "Scored");
                ps.CarAgentScripts.AddReward(1 + ps.CarAgentScripts.timePenalty);
            }
            else
            {
                ps.CarAgentScripts.AddReward(-1);
            }
            ps.CarAgentScripts.EndEpisode();

        }
    }

    public void ZoneTouched(CarAgentCurriculum.Team scoredTeam)
    {
        foreach (var ps in playState)
        {
            if (ps.CarAgentScripts.team == scoredTeam)
            {
                print("pos zone touched");
                ps.CarAgentScripts.AddReward(+1);
            }
        }
    }

    public void NegZoneTouched(CarAgentCurriculum.Team scoredTeam)
    {
        foreach (var ps in playState)
        {
            if (ps.CarAgentScripts.team == scoredTeam)
            {
                print("neg zone touched");
                ps.CarAgentScripts.AddReward(-1);
            }
        }

    }

    public void ResetBall()
    {
        ball.transform.position = ballStartingPos;
        ballRb.velocity = Vector3.zero;
        ballRb.angularVelocity = Vector3.zero;
    }
}

