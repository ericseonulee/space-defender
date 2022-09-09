using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
    Player player;
    Rigidbody2D playerRigidbody;

    [Header("Enemy Spawner 1")]
    public EnemySpawnerSingle enemySpawner_1;
    public WaveConfigSingle waveSingle_0;
    public WaveConfigSingle waveSingle_1;
    public WaveConfigSingle waveSingle_2;
    public WaveConfigSingle waveSingle_3;
    public WaveConfigSingle waveSingle_4;
    public WaveConfigSingle waveSingle_5;
    public WaveConfigSingle waveSingle_6;
    public WaveConfigSingle waveSingle_7;
    public WaveConfigSingle waveSingle_8;
    public WaveConfigSingle waveSingle_9;
    public WaveConfigSingle waveSingle_10;
    public WaveConfigSingle waveSingle_11;
    public WaveConfigSingle waveSingle_12;
    public WaveConfigSingle waveSingle_13;
    public WaveConfigSingle waveSingle_14;
    public WaveConfigSingle waveSingle_15;



    [Header("Enemy Spawner 2")]
    public EnemySpawner enemySpawner_2;
    
    AudioSource source { get { return GetComponent<AudioSource>(); } }
    public AudioClip astroSlugInitBoosterClip;

    void Awake () {
        player = FindObjectOfType<Player>();

        if (player == null) {
            Debug.LogError("player is null.");
        }

        playerRigidbody = player.GetComponent<Rigidbody2D>();

        if (playerRigidbody == null) {
            Debug.LogError("Player Rigidbody is null.");
        }
    }

    void Start() {
        StartCoroutine(PlayerInitialAnimation());
    }

    IEnumerator PlayerInitialAnimation() {
        float speed = 0f;
        float fixedSpeed = 5f;
        float acceleration = 90f;
        float waitTime = 0.3f;
        float timer = 0;

        while (timer < waitTime) {
            speed += acceleration * Time.deltaTime;
            playerRigidbody.velocity = new Vector2(0, speed);
            timer += Time.deltaTime;

            yield return null;
        }

        while (speed > 0) {
            speed -= acceleration / 2 * Time.deltaTime;
            playerRigidbody.velocity = new Vector2(0, speed);

            yield return null;
        }

        playerRigidbody.velocity = Vector3.zero;
        
        yield return new WaitForSeconds(0.4f);

        while (player.transform.position.y > -7) {
            playerRigidbody.velocity = new Vector2(0, -fixedSpeed);
            yield return null;
        }

        playerRigidbody.velocity = Vector3.zero;

        yield return new WaitForSeconds(4.5f);

        StartWaves();
    }

    void StartWaves() {
        StartCoroutine(LevelOneWaves());
    }

    IEnumerator LevelOneWaves() {
        // 1 
        enemySpawner_1.StartWave(waveSingle_0);
        yield return new WaitForSeconds(3f);

        // 2
        enemySpawner_1.StartWave(waveSingle_1);
        yield return new WaitForSeconds(0.4f);

        // 3
        enemySpawner_1.StartWave(waveSingle_2);
        yield return new WaitForSeconds(3);
        yield return StartCoroutine(LevelTwoWaves());
    }

    IEnumerator LevelTwoWaves() {
        enemySpawner_2.StartWave();
        yield return new WaitForSeconds(15);

        yield return StartCoroutine(LevelThreeWaves());
    }

    IEnumerator LevelThreeWaves() {
        // 1
        enemySpawner_1.StartWave(waveSingle_0);
        yield return new WaitForSeconds(3f);

        // 2
        enemySpawner_1.StartWave(waveSingle_1);
        yield return new WaitForSeconds(0.4f);

        enemySpawner_1.StartWave(waveSingle_2);
        yield return new WaitForSeconds(1.5f);

        // 3
        enemySpawner_1.StartWave(waveSingle_3);
        yield return new WaitForSeconds(0.1f);

        enemySpawner_1.StartWave(waveSingle_4);
        yield return new WaitForSeconds(1.5f);

        // 4
        enemySpawner_1.StartWave(waveSingle_5);
        yield return new WaitForSeconds(0.1f);

        enemySpawner_1.StartWave(waveSingle_6);
        yield return new WaitForSeconds(1.5f);

        // 5
        enemySpawner_1.StartWave(waveSingle_7);
        yield return new WaitForSeconds(0.1f);

        enemySpawner_1.StartWave(waveSingle_8);
        yield return new WaitForSeconds(1f);

        // 6
        enemySpawner_1.StartWave(waveSingle_9);
        yield return new WaitForSeconds(0.1f);

        enemySpawner_1.StartWave(waveSingle_10);
        yield return new WaitForSeconds(2f);

        // 7
        enemySpawner_1.StartWave(waveSingle_11);
        enemySpawner_1.StartWave(waveSingle_12);
        enemySpawner_1.StartWave(waveSingle_13);
        enemySpawner_1.StartWave(waveSingle_14);
        enemySpawner_1.StartWave(waveSingle_15);
        yield return new WaitForSeconds(1f);
    }
}
