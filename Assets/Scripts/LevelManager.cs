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
        enemySpawner_1.StartWave(waveSingle_0);
        yield return new WaitForSeconds(3f);
        enemySpawner_1.StartWave(waveSingle_1);
        yield return new WaitForSeconds(0.5f);
        enemySpawner_1.StartWave(waveSingle_2);
        yield return StartCoroutine(LevelTwoWaves());
    }

    IEnumerator LevelTwoWaves() {
        yield return new WaitForSeconds(3);
        enemySpawner_2.StartWave();
        yield return new WaitForEndOfFrame();
    }
}
