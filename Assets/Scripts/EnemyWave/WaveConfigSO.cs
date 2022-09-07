using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Wave Config", fileName = "New Wave Config")]
public class WaveConfigSO : ScriptableObject {
    [SerializeField] List<GameObject> enemyPrefabs;
    [SerializeField] Transform pathPrefab;
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float timeBetweenEnemySpawns = 0.3f;
    [SerializeField] float spawnTimeVariance = 0f; // randomness
    [SerializeField] float miniumSpawnTime = 0.2f;

    [SerializeField] bool isFlipped = false;

    public Transform GetStartingWaypoint() {
        return pathPrefab.GetChild(0);
    }

    public List<Transform> GetWaypoints() {
        List<Transform> waypoints = new List<Transform>();

        foreach(Transform waypoint in pathPrefab) {
            waypoints.Add(waypoint);
        }

        return waypoints;
    }

    public float GetMoveSpeed() {
        return moveSpeed;
    }

    public int GetEnemyCount() {
        return enemyPrefabs.Count;
    }

    public GameObject GetEnemyPrefab(int index) {
        return enemyPrefabs[index];
    }

    public bool GetIsFlipped() {
        return isFlipped;
    }

    public float GetRandomSpawnTime() {
        float spawnTime = Random.Range(timeBetweenEnemySpawns - spawnTimeVariance, timeBetweenEnemySpawns + spawnTimeVariance);
        return Mathf.Clamp(spawnTime, miniumSpawnTime, float.MaxValue);
    }
}
