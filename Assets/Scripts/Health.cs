using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {
    [SerializeField] int health = 50;
    [SerializeField] bool isDead;
    Animator animator;

    [SerializeField] bool applyCameraShake;
    CameraShake cameraShake;

    [Header("Enemy")]
    [SerializeField] AudioClip explosionClip;
    [SerializeField][Range(0f, 1f)] float volume = 0.15f;
    AudioSource source { get { return GetComponent<AudioSource>(); } }

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

    void Start() {
        AddAudioSource();
    }

    void Update() {
        if (source != null) {
            source.volume = volume;
        }
    }
    void AddAudioSource() {
        gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
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

                source.PlayOneShot(explosionClip, volume);
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
