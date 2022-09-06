using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    AudioSource source;
    public AudioClip clickSFX;
    public AudioClip SelectWallSFX;
    public Animator selectWallAniamtor;
    public Animator selectWallEffectAnimator;
    GameManager gameManager;

    void Awake() {
        gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        source = GameObject.Find("Canvas").GetComponent<AudioSource>();

        if (gameManager == null) {
            Debug.LogError("GameManage is NULL.");
        }

        if (source == null) {
            Debug.LogError("Audio Source is null.");
        }
    }

    public void LoadGame() {
        StartCoroutine(LoadGameRoutine());
    }

    public void ExitGame() {
        gameManager.ExitGame();
    }

    public void PlayClickSound() {
        source.PlayOneShot(clickSFX, 0.2f);
    }

    public void PlaySelectWallSFX() {
        AudioSource.PlayClipAtPoint(SelectWallSFX, Camera.main.transform.position, 0.2f);
    }

    public void PlaySelectWallAnim() {
        selectWallAniamtor.SetTrigger("wallDown");
    }

    public void PlaySelectWallEffect() {
        selectWallEffectAnimator.SetTrigger("isWallDown");
    }

    public void StartButtonClickRoutine() {
        StartCoroutine(StartRoutine());
    }

    public void QuitButtonClickRoutine() {
        StartCoroutine(QuitRoutine());
    }

    public void MainMenuClickRoutine() {
        StartCoroutine(BackToMainMenuRoutine());
    }

    void HideUI() {
        gameObject.GetComponent<Canvas>().enabled = false;
    }

    IEnumerator BackToMainMenuRoutine() {
        PlayClickSound();
        PlayClickSound();
        yield return new WaitForSeconds(0.5f);
        PlaySelectWallAnim();
        HideUI();

        yield return new WaitForSeconds(1);
        gameManager.BackToMainMenu();
    }

    IEnumerator StartRoutine() {
        PlayClickSound();
        yield return new WaitForSeconds(0.5f);
        PlaySelectWallAnim();
        HideUI();

        yield return new WaitForSeconds(0.8f);
        LoadGame();
    }

    IEnumerator QuitRoutine() {
        PlayClickSound();
        yield return new WaitForSeconds(0.5f);
        PlaySelectWallAnim();
        HideUI();

        yield return new WaitForSeconds(0.8f);
        ExitGame();
    }

    IEnumerator LoadGameRoutine() {
        yield return new WaitForSeconds(0.8f);
        SceneManager.LoadScene(1);
    }
}
