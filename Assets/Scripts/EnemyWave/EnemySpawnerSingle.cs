using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerSingle : MonoBehaviour {
    [SerializeField] WaveConfigSingle waveConfig;
    [SerializeField] float timeBetweenWaves = 4f;
    [SerializeField] bool isLooping;
    WaveConfigSingle currentWave;

    public WaveConfigSingle GetCurrentWave() {
        return currentWave;
    }

    Vector3 flipLocalScale(GameObject obj) {
        Vector3 newScale = obj.transform.localScale;
        newScale.x *= -1;

        return newScale;
    }

    public void StartWave() {
        currentWave = waveConfig;
        StartCoroutine(SpawnEnemyWaves());
    }

    public void StartWave(WaveConfigSingle waveSingle) {
        currentWave = waveSingle;
        StartCoroutine(SpawnEnemyWaves());
    }

    IEnumerator SpawnEnemyWaves() {
        do {
            GameObject enemyInstantiated = Instantiate(currentWave.GetEnemyPrefab(),
                                                    currentWave.GetStartingWaypoint().position,
                                                    Quaternion.identity,
                                                    transform);

            PathFinderSingle pathFinder = enemyInstantiated.GetComponent<PathFinderSingle>();

            if (pathFinder == null) { Debug.LogError("PathFinder is not assigned."); }
            else {
                pathFinder.SetEnemySpawner(this);
                pathFinder.SetWayPoints(GetCurrentWave().GetWaypoints());
            }
            if (currentWave.GetIsFlipped()) {
                enemyInstantiated.transform.localScale = flipLocalScale(enemyInstantiated);
            }

            yield return new WaitForSeconds(timeBetweenWaves);
        }
        while (isLooping);
    }
}
