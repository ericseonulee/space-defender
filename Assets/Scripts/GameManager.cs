using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public bool isGameOver;
    [SerializeField] private GameObject _pauseMenuPanel;

    AudioSource source;
    public AudioSource audioPlayer;
    public AudioClip gameOverClip;
    public GameObject enemies;
    public GameObject projectiles;
    void Awake() {
        source = gameObject.GetComponent<AudioSource>();

        if (source == null) {
            Debug.LogError("Audio Source is null.");
        }
    }

    private void Update() {
        if (isGameOver && Input.GetKeyDown(KeyCode.R)) {
            RestartGame();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && _pauseMenuPanel != null) {
            if (_pauseMenuPanel.activeSelf) {
                ResumeGame();
            }
            else {
                _pauseMenuPanel.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }

    public void GameOver() {
        isGameOver = true;
        StartCoroutine(GameOverRoutine());
    }

    public void RestartGame() {
        SceneManager.LoadScene(1);
    }

    public void ResumeGame() {
        _pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
        //AudioSource.PlayClipAtPoint(window_downSFX, new Vector3(0, 0, -10), 0.5f);
    }

    public void BackToMainMenu() {
        Time.timeScale = 1f;
        //AudioSource.PlayClipAtPoint(window_downSFX, new Vector3(0, 0, -10), 0.5f);
        SceneManager.LoadScene(0);
    }

    public void ExitGame() {
        StartCoroutine(ExitGameRoutine());
    }

    IEnumerator GameOverRoutine() {
        if (audioPlayer != null) {
            audioPlayer.Stop();
        }
        if (enemies != null) {
            enemies.SetActive(false);
        }
        if (projectiles != null) {
            projectiles.SetActive(false);
        }
        source.PlayOneShot(gameOverClip, 0.2f);
        yield return new WaitForSeconds(9);
        SceneManager.LoadScene(2);
    }

    public IEnumerator ExitGameRoutine() {
        Time.timeScale = 1f;
        yield return new WaitForSeconds(0.8f);
        Application.Quit();
    }
}
