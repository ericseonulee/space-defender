using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class PathFinder : MonoBehaviour {
    EnemySpawner enemySpawner;
    List<Transform> waypoints;
    int waypointIndex = 0;

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

    public void SetEnemySpawner(EnemySpawner enemySpawner) {
        this.enemySpawner = enemySpawner;
    }

    public void SetWayPoints(List<Transform> waypoints) {
        this.waypoints = waypoints;
        transform.position = this.waypoints[waypointIndex].position;
    }
}
