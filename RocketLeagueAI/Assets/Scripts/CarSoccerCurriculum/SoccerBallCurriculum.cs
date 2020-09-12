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
        if (other.tag == "curriculumReward12")
        {
            agent.CurriculumReward();
        }
        if (other.tag == "curriculumReward13")
        {
            agent.CurriculumReward();
        }
        if (other.tag == "curriculumReward2")
        {
            GameManagerCurriculum.gameManager.GoalScored();
            agent.CurriculumReward1();
        }
        if (other.tag == "curriculumReward22")
        {
            GameManagerCurriculum.gameManager.GoalScored();
            agent.CurriculumReward1();
        }
        if (other.tag == "curriculumReward23")
        {
            GameManagerCurriculum.gameManager.GoalScored();
            agent.CurriculumReward1();
        }
        if (other.tag == "curriculumReward3")
        {
            agent.CurriculumReward2();
        }
        if (other.tag == "curriculumReward32")
        {
            agent.CurriculumReward2();
        }
        if (other.tag == "curriculumReward33")
        {
            agent.CurriculumReward2();
        }
        if (other.tag == "curriculumNegReward")
        {
            agent.CurriculumNegReward();
        }
    }
}
