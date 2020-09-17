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
}
