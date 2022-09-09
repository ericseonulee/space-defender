using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Wave Config Single", fileName = "Wave Config Single")]
public class WaveConfigSingle : ScriptableObject {
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] Transform pathPrefab;
    [SerializeField] float moveSpeed = 10f;

    [SerializeField] bool isFlipped = false;

    public Transform GetStartingWaypoint() {
        return pathPrefab.GetChild(0);
    }

    public List<Transform> GetWaypoints() {
        List<Transform> waypoints = new List<Transform>();

        foreach (Transform waypoint in pathPrefab) {
            waypoints.Add(waypoint);
        }

        return waypoints;
    }

    public float GetMoveSpeed() {
        return moveSpeed;
    }

    public bool GetIsFlipped() {
        return isFlipped;
    }

    public GameObject GetEnemyPrefab() {
        return enemyPrefab;
    }
}
