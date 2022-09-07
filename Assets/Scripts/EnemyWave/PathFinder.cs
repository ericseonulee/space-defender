using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class PathFinder : MonoBehaviour {
    EnemySpawner enemySpawner;
    List<Transform> waypoints;
    int waypointIndex = 0;

    void Awake() {
        enemySpawner = FindObjectOfType<EnemySpawner>();
    }

    void Start() {
        waypoints = enemySpawner.GetCurrentWave().GetWaypoints();
        transform.position = waypoints[waypointIndex].position;
    }

    void Update() {
        FollowPath();
    }

    void FollowPath() {
        if (waypointIndex < waypoints.Count) {
            Vector3 targetPosition = waypoints[waypointIndex].position;
            float delta = enemySpawner.GetCurrentWave().GetMoveSpeed() * Time.deltaTime;

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, delta);

            if (transform.position == targetPosition) {
                waypointIndex++;
            }
        }
        else {
            Destroy(gameObject);
        }


    }
}
