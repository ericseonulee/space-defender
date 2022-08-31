using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {
    [SerializeField] int health = 50;
    [SerializeField] bool isDead;
    Animator animator;

    [SerializeField] bool applyCameraShake;
    CameraShake cameraShake;

    void Awake() {
        animator = gameObject.GetComponent<Animator>();
        cameraShake = Camera.main.GetComponent<CameraShake>();

        if (gameObject.tag == "Enemy" && animator == null) {
            Debug.LogError("Animator is null");
        }
        if (cameraShake == null) {
            Debug.LogError("CameraShake is null.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        DamageDealer damageDealer = collision.GetComponent<DamageDealer>();

        if (damageDealer != null) {
            TakeDamage(damageDealer.GetDamage());
            ShakeCamera();
            damageDealer.Hit(gameObject);
        }
    }

    void TakeDamage(int damageDealt) {
        health -= damageDealt;
        if (health <= 0) {
            Collider2D collider = GetComponent<Collider2D>();

            collider.enabled = false;
            if (gameObject.tag == "Enemy") {
                animator.SetTrigger("OnDeath");
                isDead = true;
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
}
