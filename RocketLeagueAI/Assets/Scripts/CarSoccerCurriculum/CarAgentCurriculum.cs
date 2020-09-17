using UnityEngine;
using Unity.MLAgents;
using System;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Policies;

public class CarAgentCurriculum : Agent
{
    [SerializeField] private string horizontalInputName, verticalInputName;

    [HideInInspector]
    public SoccerBall soccerBall;
    public Bounds gameAreaBounds;
    public float timePenalty;

    public enum Team
    {
        Blue = 0,
        Orange = 1
    }

    Rigidbody m_Rigidbody;
    Rigidbody m_Ball;

    public GameObject Stadium;
    public GameObject Ball;

    public GameManagerCurriculum gameArea;

    private Vector3 spawnLocation;
    private Quaternion spawnRotation;

    private Vector3 ballSpawnLocation;
    private Quaternion ballSpawnRotation;

    public string boostInputAxis = "Boost";
    public float boostJolt = 5f;
    public bool carIsOnTheGround = true;

    //public float resetCooldownBall = 10;
    //private float nextResetTimeBall = 10;
    //public float resetCooldownCar = 5;
    //private float nextResetTimeCar = 5;

    public float JumpSpeed = 10f;

    private float horizontalInput;
    private float verticalInput;

    private bool RollRightInput;
    private bool RollLeftInput;

    public float rotationSpeed = 10f;

    private int m_PlayerIndex;

    public Transform car;
    public Transform goal1;
    public Transform goal2;
    public Transform CurriuculumRewardZone;
    public Transform CurriuculumRewardZone02;
    public Transform CurriuculumRewardZone03;
    public Transform CurriuculumRewardZone1;
    public Transform CurriuculumRewardZone12;
    public Transform CurriuculumRewardZone13;
    public Transform CurriuculumRewardZone2;
    public Transform CurriuculumRewardZone22;
    public Transform CurriuculumRewardZone23;
    public Transform CurriculumNegativeRewardZone;
    public Team team;

    Vector3 m_Transform;

    BehaviorParameters m_BehaviorParameters;

    public override void Initialize()
    {
        m_BehaviorParameters = gameObject.GetComponent<BehaviorParameters>();
        team = Team.Blue;
        m_Transform = new Vector3(transform.position.x + 0f, transform.position.y + 0f, transform.position.z + 0f);

        m_Rigidbody = GetComponent<Rigidbody>();
        m_Ball = Ball.GetComponent<Rigidbody>();

        m_Rigidbody = GetComponent<Rigidbody>();
        m_Ball = Ball.GetComponent<Rigidbody>();

        var ps = new PlayState
        {
            CarAgentCurriculum = m_Rigidbody,
            startingPosition = transform.position,
            CarAgentScripts = this,
        };

        gameArea.playState.Add(ps);
        m_PlayerIndex = gameArea.playState.IndexOf(ps);
        ps.playerIndix = m_PlayerIndex;



        gameAreaBounds = Stadium.GetComponent<Collider>().bounds;

    }

    public Vector3 GetRandomSpawnPos()
    {
        var foundNewSpawnLocation = false;
        var randomSpawnPos = Vector3.zero;
        while (foundNewSpawnLocation == false)
        {
            var randomPosX = UnityEngine.Random.Range(-gameAreaBounds.extents.x * 1,
                gameAreaBounds.extents.x * 1);

            var randomPosZ = UnityEngine.Random.Range(-gameAreaBounds.extents.z * 1,
                gameAreaBounds.extents.z * 1);

            randomSpawnPos = Stadium.transform.position + new Vector3(randomPosX, 1f, randomPosZ);
            if (Physics.CheckBox(randomSpawnPos, new Vector3(2.5f, 0.01f, 2.5f)) == false)
            {
                foundNewSpawnLocation = true;
            }
        }
        return randomSpawnPos;
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

    public override void OnEpisodeBegin()
    {
        ResetBall();
        ResetCar();
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
        sensor.AddObservation(m_Rigidbody.position);
        sensor.AddObservation(goal1.position);
        sensor.AddObservation(goal2.position);
        sensor.AddObservation(CurriuculumRewardZone.position);
        sensor.AddObservation(CurriuculumRewardZone02.position);
        sensor.AddObservation(CurriuculumRewardZone03.position);
        sensor.AddObservation(CurriuculumRewardZone1.position);
        sensor.AddObservation(CurriuculumRewardZone12.position);
        sensor.AddObservation(CurriuculumRewardZone13.position);
        sensor.AddObservation(CurriuculumRewardZone2.position);
        sensor.AddObservation(CurriuculumRewardZone22.position);
        sensor.AddObservation(CurriuculumRewardZone23.position);
        sensor.AddObservation(CurriculumNegativeRewardZone.position);


        //distance to reward zones
        sensor.AddObservation(Vector3.Distance(CurriuculumRewardZone.transform.position, transform.position));
        sensor.AddObservation(Vector3.Distance(CurriuculumRewardZone02.transform.position, transform.position));
        sensor.AddObservation(Vector3.Distance(CurriuculumRewardZone03.transform.position, transform.position));
        sensor.AddObservation(Vector3.Distance(CurriuculumRewardZone1.transform.position, transform.position));
        sensor.AddObservation(Vector3.Distance(CurriuculumRewardZone12.transform.position, transform.position));
        sensor.AddObservation(Vector3.Distance(CurriuculumRewardZone13.transform.position, transform.position));
        sensor.AddObservation(Vector3.Distance(CurriuculumRewardZone2.transform.position, transform.position));
        sensor.AddObservation(Vector3.Distance(CurriuculumRewardZone22.transform.position, transform.position));
        sensor.AddObservation(Vector3.Distance(CurriuculumRewardZone23.transform.position, transform.position));
        //distance to neg zone
        sensor.AddObservation(Vector3.Distance(CurriculumNegativeRewardZone.transform.position, transform.position));

        //direction to reward Zones
        sensor.AddObservation((CurriuculumRewardZone.transform.position - transform.position).normalized);
        sensor.AddObservation((CurriuculumRewardZone02.transform.position - transform.position).normalized);
        sensor.AddObservation((CurriuculumRewardZone03.transform.position - transform.position).normalized);
        sensor.AddObservation((CurriuculumRewardZone1.transform.position - transform.position).normalized);
        sensor.AddObservation((CurriuculumRewardZone12.transform.position - transform.position).normalized);
        sensor.AddObservation((CurriuculumRewardZone13.transform.position - transform.position).normalized);
        sensor.AddObservation((CurriuculumRewardZone2.transform.position - transform.position).normalized);
        sensor.AddObservation((CurriuculumRewardZone22.transform.position - transform.position).normalized);
        sensor.AddObservation((CurriuculumRewardZone23.transform.position - transform.position).normalized);
        //direction to neg zone
        sensor.AddObservation((CurriculumNegativeRewardZone.transform.position - transform.position).normalized);

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
            m_Rigidbody.AddForce(new Vector3(0, 4000, 0), ForceMode.Impulse);
            carIsOnTheGround = false;
        }
    }

    public void ResetCar()
    {
        //if (Time.time > nextResetTimeCar)
        //{
        if (m_Rigidbody != null)
        {
            m_Rigidbody.velocity = Vector3.zero;
            m_Rigidbody.angularVelocity = Vector3.zero;
        }

        transform.position = GetRandomSpawnPos();
        transform.rotation = spawnRotation;
        //nextResetTimeCar = Time.time + resetCooldownCar;
        //}

    }

    public void ResetBall()
    {
        //if (Time.time > nextResetTimeBall)
        //{
        if (m_Ball != null)
        {
            m_Ball.velocity = Vector3.zero;
            m_Ball.angularVelocity = Vector3.zero;
        }

        m_Ball.transform.position = ballSpawnLocation;
        m_Ball.transform.rotation = ballSpawnRotation;
        //    nextResetTimeBall = Time.time + resetCooldownBall;
        //}
    }

    void Boost()
    {
        m_Rigidbody.velocity = transform.forward * boostJolt;
    }

    void Update()
    {
        PlayerInput();
    }

    public void MoveAgentGround(float[] act)
    {
        var dirToGo = Vector3.zero;

        var action = Mathf.FloorToInt(act[0]);

        switch (action)
        {
            case 1:
                m_Rigidbody.velocity = transform.forward * boostJolt;
                break;
            case 2:
                m_Rigidbody.velocity = transform.forward * -boostJolt;
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
            AddReward(2f);
            print("rewardTouch");
        }

    }

    //void ifBallCloseToGoal() {
    //    float angle = 30;
    //    float reward = 0;
    //    float AngleBallNet = Vector3.Angle(m_Ball.velocity, goal1.transform.localPosition - Ball.transform.localPosition);
    //    if (m_Ball.velocity.magnitude > 0.5f && AngleBallNet < angle)
    //        reward = scale(0, angle, 0.001f, 0, AngleBallNet);

    //}


    void FixedUpdate()
    {


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