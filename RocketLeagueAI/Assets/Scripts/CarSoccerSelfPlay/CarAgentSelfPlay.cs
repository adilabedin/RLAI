using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Policies;
using System;

public class CarAgentSelfPlay : Agent
{

    [SerializeField] private string horizontalInputName, verticalInputName;

    [HideInInspector]
    public SoccerBall soccerBall;
    public Bounds areaBounds;
    public float timePenalty;

    private Rigidbody agentRb;
    Rigidbody m_Ball;

    public enum Team
    {
        Blue = 0,
        Orange = 1
    }

    public GameObject Stadium;
    public GameObject Ball;

    public GameManagerSelfPlay area;

    //private Vector3 spawnLocation;
    //private Quaternion spawnRotation;

    //private Vector3 ballSpawnLocation;
    //private Quaternion ballSpawnRotation;

    public string boostInputAxis = "Boost";
    public float boostJolt = 5f;
    public bool carIsOnTheGround = true;

    public float JumpSpeed = 10f;

    private float horizontalInput;
    private float verticalInput;

    private bool RollRightInput;
    private bool RollLeftInput;

    public float rotationSpeed = 10f;

    public Transform car;
    public Transform goal1;
    public Transform goal2;
    public Team team;

    BehaviorParameters m_BehaviorParameters;
    Vector3 m_Transform;
    private int m_PlayerIndex;
    public bool blueTeamfull = false;

    public override void Initialize()
    {
        m_BehaviorParameters = gameObject.GetComponent<BehaviorParameters>();

        if (blueTeamfull == false)
        {
            team = Team.Blue;
            m_Transform = new Vector3(transform.position.x + 0f, transform.position.y + 0f, transform.position.z + 0f);
        }
        else if (blueTeamfull == true) 
        {
            team = Team.Orange;
            m_Transform = new Vector3(transform.position.x + 0f, transform.position.y + 0f, transform.position.z + 0f);
        }

        agentRb = GetComponent<Rigidbody>();
        m_Ball = Ball.GetComponent<Rigidbody>();

        var ps = new PlayerState
        {
            agentRb = agentRb,
            startingPos = transform.position,
            agentScript = this,
        };

        area.playerState.Add(ps);
        m_PlayerIndex = area.playerState.IndexOf(ps);
        ps.playerIndex = m_PlayerIndex;

    }

    public override void OnEpisodeBegin()
    {
        transform.position = m_Transform;
        agentRb.velocity = Vector3.zero;
        agentRb.angularVelocity = Vector3.zero;
        SetResetParameters();
    }

    public void SetResetParameters()
    {
        area.ResetBall();
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        if (carIsOnTheGround)
        {
            MoveAgentGround(vectorAction);
        }
        else
        {
            MoveAgentAir(vectorAction);
        }

        AddReward(-1f / MaxStep);
    }

    public override void Heuristic(float[] actionsOut)
    {

        Array.Clear(actionsOut, 0, actionsOut.Length);

        //Drive
        if (Input.GetAxis("Forward") > 0)
        {
            actionsOut[0] = 1;
        }
        //Reverse
        if (Input.GetAxis("Reverse") > 0)
        {
            actionsOut[0] = 2;
        }
        //TurnLeft
        if (Input.GetAxis("TurnLeft") > 0)
        {
            actionsOut[0] = 3;
        }
        //TurnRight
        if (Input.GetAxis("TurnRight") > 0)
        {
            actionsOut[0] = 4;
        }
        //jump
        if (Input.GetAxis("jumpKey") > 0)
        {
            actionsOut[0] = 5;
        }
        //Boost
        if (Input.GetAxis("Boost") > 0)
        {
            actionsOut[1] = 1;
        }
        //Rotate Horizontally
        if (Input.GetAxis("Horizontal") > 0)
        {
            actionsOut[1] = 2;
        }
        if (Input.GetAxis("Horizontal") < 0)
        {
            actionsOut[1] = 3;
        }
        //Rotate Vertically
        if (Input.GetAxis("Vertical") > 0)
        {
            actionsOut[1] = 4;
        }
        if (Input.GetAxis("Vertical") < 0)
        {
            actionsOut[1] = 5;
        }
        //AirRoll / Barrell Roll
        if (Input.GetAxis("AirRollLeft") > 0)
        {
            actionsOut[1] = 6;
        }
        if (Input.GetAxis("AirRollRight") > 0)
        {
            actionsOut[1] = 7;
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(car.position);
        sensor.AddObservation(m_Ball.position);
        sensor.AddObservation(agentRb.position);
        sensor.AddObservation(goal1.position);
        sensor.AddObservation(goal2.position);

        //distance to opponent
        sensor.AddObservation(Vector3.Distance(car.transform.position, transform.position));
        //direction to ball
        sensor.AddObservation((car.transform.position - transform.position).normalized);

        //distance to ball
        sensor.AddObservation(Vector3.Distance(m_Ball.transform.position, transform.position));
        //direction to ball
        sensor.AddObservation((m_Ball.transform.position - transform.position).normalized);

        //distance from ball to goal1
        sensor.AddObservation(Vector3.Distance(m_Ball.transform.position, goal1.transform.position));
        //distance from ball to goal2
        sensor.AddObservation(Vector3.Distance(m_Ball.transform.position, goal2.transform.position));

        //distance to Goal1
        sensor.AddObservation(Vector3.Distance(goal1.transform.position, transform.position));
        //direction to Goal1
        sensor.AddObservation((goal1.transform.position - transform.position).normalized);

        //distance to Goal2
        sensor.AddObservation(Vector3.Distance(goal2.transform.position, transform.position));
        //direction to Goal2
        sensor.AddObservation((goal2.transform.position - transform.position).normalized);
    }

    private void Jump()
    {
        if (carIsOnTheGround)
        {
            agentRb.AddForce(new Vector3(0, 4000, 0), ForceMode.Impulse);
            carIsOnTheGround = false;
        }
    }


    public void MoveAgentGround(float[] act)
    {
        var action = Mathf.FloorToInt(act[0]);

        switch (action)
        {
            case 1:
                agentRb.velocity = transform.forward * boostJolt;
                break;
            case 2:
                agentRb.velocity = transform.forward * -boostJolt;
                break;
            case 3:
                this.transform.Rotate(Vector3.up, rotationSpeed);
                break;
            case 4:
                this.transform.Rotate(Vector3.down, rotationSpeed);
                break;
            case 5:
                Jump();
                break;
        }
    }

    void MoveAgentAir(float[] act)
    {
        if (carIsOnTheGround == false)
        {
            var action = Mathf.FloorToInt(act[1]);

            switch (action)
            {
                case 1:
                    Boost();
                    break;
                case 2:
                    RotateLeft();
                    break;
                case 3:
                    RotateRight();
                    break;
                case 4:
                    RotateUp();
                    break;
                case 5:
                    RotateDown();
                    break;
                case 6:
                    AirRollLeft();
                    break;
                case 7:
                    AirRollRight();
                    break;
            }
        }
    }


    private void OnCollisionEnter(Collision collidedObj)
    {
        if (collidedObj.gameObject.CompareTag("Ground"))
        {
            carIsOnTheGround = true;
        }

        if (collidedObj.gameObject.CompareTag("Ball"))
        {
            AddReward(0.2f);
            print("rewardTouch");
        }

    }

    void Boost()
    {
       agentRb.velocity = transform.forward * boostJolt;
    }

    void Update()
    {
        PlayerInput();
    }

    private void PlayerInput()
    {
        // Grab player input
        verticalInput = Input.GetAxis(verticalInputName);
        horizontalInput = Input.GetAxis(horizontalInputName);

        if (Input.GetAxis("AirRollRight") > 0)
        {
            RollRightInput = true;
        }
        if (Input.GetAxis("AirRollLeft") > 0)
        {
            RollLeftInput = true;
        }

    }

    private void RotateUp()
    {
        if (carIsOnTheGround == false)
        {
            if (verticalInput > 0)
            {
                this.transform.Rotate(Vector3.right, rotationSpeed);
            }
        }
    }

    private void RotateDown()
    {
        if (carIsOnTheGround == false)
        {
            if (verticalInput < 0)
            {
                this.transform.Rotate(Vector3.left, rotationSpeed);
            }
        }
    }

    private void RotateLeft()
    {
        if (horizontalInput > 0)
        {
            this.transform.Rotate(Vector3.up, rotationSpeed);
        }

    }

    private void RotateRight()
    {
        if (horizontalInput < 0)
        {
            this.transform.Rotate(Vector3.down, rotationSpeed);
        }
    }

    private void AirRollLeft()
    {
        if (carIsOnTheGround == false)
        {
            if (RollRightInput)
            {
                this.transform.Rotate(Vector3.back, rotationSpeed);
            }
        }
    }

    private void AirRollRight()
    {
        if (carIsOnTheGround == false)
        {
            if (RollLeftInput)
            {
                this.transform.Rotate(Vector3.forward, rotationSpeed);
            }
        }

    }


}



