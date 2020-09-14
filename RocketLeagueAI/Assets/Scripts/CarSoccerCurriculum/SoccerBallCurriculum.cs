using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;

public class SoccerBallCurriculum : MonoBehaviour
{

    public CarAgentCurriculum agent;
    public string orangeGoalTag;
    public string blueGoalTag;
    //public string curriculumReward;
    //public string curriculumReward1;
    //public string curriculumReward2;

    void OnCollisionEnter(Collision col)
    {
        // Touched goal.
        if (col.gameObject.CompareTag(orangeGoalTag))
        {
            GameManagerCurriculum.gameManager.GoalScored();
            agent.ScoredAGoal();
        }
        if (col.gameObject.CompareTag(blueGoalTag))
        {
            GameManagerCurriculum.gameManager.GoalScored();
            agent.ConcededAGoal();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "curriculumReward")
        {
            agent.CurriculumReward();
        }
        if (other.tag == "curriculumReward02")
        {
            agent.CurriculumReward01();
        }
        if (other.tag == "curriculumReward03")
        {
            agent.CurriculumReward02();
        }
        if (other.tag == "curriculumReward1")
        {
            agent.CurriculumReward1();
        }
        if (other.tag == "curriculumReward12")
        {
            agent.CurriculumReward11();
        }
        if (other.tag == "curriculumReward13")
        {
            agent.CurriculumReward12();
        }
        if (other.tag == "curriculumReward2")
        {
            agent.CurriculumReward2();
        }
        if (other.tag == "curriculumReward22")
        {
            agent.CurriculumReward21();
        }
        if (other.tag == "curriculumReward23")
        {
            agent.CurriculumReward22();
        }
        if (other.tag == "curriculumNegReward")
        {
            agent.CurriculumNegReward();
        }
    }
}
