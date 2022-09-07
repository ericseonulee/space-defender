using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
    Player player;
    Rigidbody2D playerRigidbody;

    EnemySpawner enemySpawner;
    AudioSource source { get { return GetComponent<AudioSource>(); } }
    public AudioClip astroSlugInitBoosterClip;

    void Awake () {
        player = FindObjectOfType<Player>();
        enemySpawner = FindObjectOfType<EnemySpawner>();

        if (player == null) {
            Debug.LogError("player is null.");
        }
        if (enemySpawner == null) {
            Debug.LogError("EnemySpawner is null.");
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

        source.PlayOneShot(astroSlugInitBoosterClip, 0.15f);
        while (player.transform.position.y < -6) {
            speed += acceleration * Time.deltaTime;
            playerRigidbody.velocity = new Vector2(0, speed);

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

        yield return new WaitForSeconds(5f);

        enemySpawner.StartWave();
    }
}
