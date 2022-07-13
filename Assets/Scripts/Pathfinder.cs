using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    EnemySpawner enemySpawner;
    WaveConfigSO waveConfigSO;
    List<Transform> waypoints;
    int waypointsIndex = 0;

    private void Awake()
    {
        enemySpawner = FindObjectOfType<EnemySpawner>();
    }

    void Start()
    {
        waveConfigSO = enemySpawner.GetCurrentWave();
        waypoints = waveConfigSO.GetWaypoints();
        transform.position = waypoints[waypointsIndex].position;
    }

    void Update()
    {
        FollowPath();
    }

    void FollowPath()
    {
        if (waypointsIndex < waypoints.Count)
        {
            Vector3 targetPosition = waypoints[waypointsIndex].position;
            float delta = waveConfigSO.GetMoveSpeed() * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, delta);
            if (transform.position == targetPosition)
            {
                waypointsIndex++;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
