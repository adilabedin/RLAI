using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoccerBallCurriculum : MonoBehaviour
{
    [HideInInspector]
    public GameManagerCurriculum gameArea;
    public string orangeGoalTag;
    public string blueGoalTag;

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag(orangeGoalTag))
        {
            gameArea.GoalTouched(CarAgentCurriculum.Team.Blue);
        }
        if (col.gameObject.CompareTag(blueGoalTag))
        {
            gameArea.GoalTouched(CarAgentCurriculum.Team.Blue);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "curriculumReward")
        {
            gameArea.ZoneTouched(CarAgentCurriculum.Team.Blue);
        }
        if (other.tag == "curriculumReward02")
        {
            gameArea.ZoneTouched(CarAgentCurriculum.Team.Blue);
        }
        if (other.tag == "curriculumReward03")
        {
            gameArea.ZoneTouched(CarAgentCurriculum.Team.Blue);
        }
        if (other.tag == "curriculumReward1")
        {
            gameArea.ZoneTouched(CarAgentCurriculum.Team.Blue);
        }
        if (other.tag == "curriculumReward12")
        {
            gameArea.ZoneTouched(CarAgentCurriculum.Team.Blue);
        }
        if (other.tag == "curriculumReward13")
        {
            gameArea.ZoneTouched(CarAgentCurriculum.Team.Blue);
        }
        if (other.tag == "curriculumReward2")
        {
            gameArea.ZoneTouched(CarAgentCurriculum.Team.Blue);
        }
        if (other.tag == "curriculumReward22")
        {
            gameArea.ZoneTouched(CarAgentCurriculum.Team.Blue);
        }
        if (other.tag == "curriculumReward23")
        {
            gameArea.ZoneTouched(CarAgentCurriculum.Team.Blue);
        }
        if (other.tag == "curriculumNegReward")
        {
            gameArea.ZoneTouched(CarAgentCurriculum.Team.Blue);
        }
    }
}
