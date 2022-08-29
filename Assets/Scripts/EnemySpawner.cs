using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    [SerializeField] List<WaveConfigSO> waveConfigs;
    [SerializeField] float timeBetweenWaves = 2f;
    [SerializeField] bool isLooping;
    WaveConfigSO currentWave;


    void Start() {
        StartCoroutine(SpawnEnemyWaves());
    }

    void Update() {

    }

    public WaveConfigSO GetCurrentWave() {
        return currentWave;
    }

    Vector3 flipLocalScale(GameObject obj) {
        Vector3 newScale = obj.transform.localScale;
        newScale.x *= -1;
        
        return newScale;
    }

    IEnumerator SpawnEnemyWaves() {
        do {
            yield return new WaitForSeconds(timeBetweenWaves);
            foreach (WaveConfigSO wave in waveConfigs) {
                currentWave = wave;

                for (int i = 0; i < currentWave.GetEnemyCount(); i++) {
                    GameObject enemyInstantiated = Instantiate(currentWave.GetEnemyPrefab(i),
                                                            currentWave.GetStartingWaypoint().position,
                                                            Quaternion.identity,
                                                            transform);
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
