using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Health : MonoBehaviour {
    [SerializeField] bool isPlayer;
    [SerializeField] int health = 3;
    [SerializeField] bool isDead;
    bool isInvinsible;
    Animator animator;

    [SerializeField] bool applyCameraShake;
    CameraShake cameraShake;

    PlayerAudio playerAudio;
    EnemyAudio enemyAudio;
    UIDisplay UI;
    GameManager gameManager;

    Tint tint;
    //Tint[] tints;

    void Awake() {
        gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        animator = gameObject.GetComponent<Animator>();
        cameraShake = Camera.main.GetComponent<CameraShake>();
        UI = FindObjectOfType<UIDisplay>();

        if (gameManager == null) {
            Debug.LogError("GameManage is NULL.");
        }

        if (isPlayer) {
            //tints = gameObject.GetComponentsInChildren<Tint>();
        }
        else {
            tint = gameObject.GetComponent<Tint>();
        }


        if (gameObject.tag == "Player") {
            playerAudio = gameObject.GetComponent<PlayerAudio>();

            if (playerAudio == null) {
                Debug.LogError("PlayerAudio is null.");
            }
        }
        if (gameObject.tag == "Enemy") {
            enemyAudio = gameObject.GetComponent<EnemyAudio>();

            if (enemyAudio == null) {
                Debug.LogError("enemyAudio is null");
            }

            if (animator == null) {
                Debug.LogError("Animator is null");
            }
        }
        
        if (cameraShake == null) {
            Debug.LogError("CameraShake is null.");
        }
        
        if (UI == null) {
            Debug.LogError("UI is null.");
        }

        if (gameObject.tag == "enemy" && tint == null) {
            Debug.LogError("Tint is null.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        DamageDealer damageDealer = collision.GetComponent<DamageDealer>();

        if (damageDealer != null) {
            damageDealer.Hit(gameObject);

            if (isInvinsible) { return; }

            TakeDamage(damageDealer.GetDamage());

            if (isPlayer) {
                isInvinsible = true;
                ShakeCamera();
                UI.UpdateSlider(damageDealer.GetDamage());
                Invoke("ResetVulnerability", 1f);
            }
        }
    }

    void TakeDamage(int damageDealt) {
        health -= damageDealt;

        if (isPlayer) {
            playerAudio.PlaySlugDamageClipOneShot();
            //foreach (Tint t in tints) {
            //    t.SetTintColor(Tint.playerBaseColor, Tint.playerBaseColor);
            //}
        }
        else if (gameObject.tag == "Enemy") {
            tint.SetTintColor(Tint.enemyBaseColor, Tint.enemyTintColor);
        }

        if (health <= 0) {
            Collider2D collider = GetComponent<Collider2D>();

            collider.enabled = false;

            if (gameObject.tag == "Player") {
                gameManager.GameOver();
            }
            else if (gameObject.tag == "Enemy") {
                tint.ResetMaterial();
                animator.SetTrigger("OnDeath");
                isDead = true;
                enemyAudio.PlayExplosionClipOneShot();
            }
            Destroy(gameObject, 1f);
        }
    }

    public bool IsDead() {
        return isDead;
    }

    public void ShakeCamera() {
        if (cameraShake != null && applyCameraShake) {
            cameraShake.Play();
        }
    }

    public int GetHealth() {
        return health;
    }

    void ResetVulnerability() {
        isInvinsible = false;
    }
}
