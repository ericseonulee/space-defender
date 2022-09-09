using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    [SerializeField] List<WaveConfig> waveConfigs;
    [SerializeField] float timeBetweenWaves = 4f;
    [SerializeField] bool isLooping;
    WaveConfig currentWave;

    public WaveConfig GetCurrentWave() {
        return currentWave;
    }

    Vector3 flipLocalScale(GameObject obj) {
        Vector3 newScale = obj.transform.localScale;
        newScale.x *= -1;
        
        return newScale;
    }

    public void StartWave() {
        StartCoroutine(SpawnEnemyWaves());
    }

    IEnumerator SpawnEnemyWaves() {
        do {
            foreach (WaveConfig wave in waveConfigs) {
                currentWave = wave;

                for (int i = 0; i < currentWave.GetEnemyCount(); i++) {
                    GameObject enemyInstantiated = Instantiate(currentWave.GetEnemyPrefab(i),
                                                            currentWave.GetStartingWaypoint().position,
                                                            Quaternion.identity,
                                                            transform);

                    PathFinder pathFinder = enemyInstantiated.GetComponent<PathFinder>();

                    if (pathFinder == null) { Debug.LogError("PathFinder is not assigned."); }
                    else {
                        pathFinder.SetEnemySpawner(this);
                        pathFinder.SetWayPoints(GetCurrentWave().GetWaypoints());
                    }
                    if (currentWave.GetIsFlipped()) {
                        enemyInstantiated.transform.localScale = flipLocalScale(enemyInstantiated);
                    }
                    yield return new WaitForSeconds(currentWave.GetRandomSpawnTime());
                }

                yield return new WaitForSeconds(timeBetweenWaves);
            }
        }
        while (isLooping);
    }
}
