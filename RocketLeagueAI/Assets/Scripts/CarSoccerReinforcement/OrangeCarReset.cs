using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class OrangeCarReset : MonoBehaviour
{
    Rigidbody m_Rigidbody;
    public enum Team
    {
        Blue = 0,
        Orange = 1
    }

    private Vector3 spawnLocation;
    private Quaternion spawnRotation;

    public Bounds areaBounds;
    public Team team;
    public GameObject Stadium;

    public void start()
    {

        team = Team.Orange;

        m_Rigidbody = GetComponent<Rigidbody>();

        spawnLocation = m_Rigidbody.transform.position;
        spawnRotation = m_Rigidbody.transform.rotation;

        areaBounds = Stadium.GetComponent<Collider>().bounds;

    }

    private void Update()
    {
        ResetCar();
    }

    public void ResetCar()
    {
        if (m_Rigidbody != null)
        {
            m_Rigidbody.velocity = Vector3.zero;
            m_Rigidbody.angularVelocity = Vector3.zero;
        }

        transform.position = GetRandomSpawnPos();
        transform.rotation = spawnRotation;
    }

    public Vector3 GetRandomSpawnPos()
    {
        var foundNewSpawnLocation = false;
        var randomSpawnPos = Vector3.zero;
        while (foundNewSpawnLocation == false)
        {
            var randomPosX = UnityEngine.Random.Range(-areaBounds.extents.x * 1,
                areaBounds.extents.x * 1);

            var randomPosZ = UnityEngine.Random.Range(-areaBounds.extents.z * 1,
                areaBounds.extents.z * 1);

            randomSpawnPos = Stadium.transform.position + new Vector3(randomPosX, 1f, randomPosZ);
            if (Physics.CheckBox(randomSpawnPos, new Vector3(2.5f, 0.01f, 2.5f)) == false)
            {
                foundNewSpawnLocation = true;
            }
        }
        return randomSpawnPos;
    }


}

