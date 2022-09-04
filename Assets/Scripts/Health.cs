using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Health : MonoBehaviour {
    [SerializeField] bool isPlayer;
    [SerializeField] int health = 3;
    [SerializeField] bool isDead;
    Animator animator;

    [SerializeField] bool applyCameraShake;
    CameraShake cameraShake;

    [Header("Enemy")]
    [SerializeField] AudioClip explosionClip;
    [SerializeField][Range(0f, 1f)] float volume = 0.15f;
    AudioSource source { get { return GetComponent<AudioSource>(); } }
    UIDisplay UI;

    [Header("Health Damage Tint")]
    SpriteRenderer spriteRenderer;
    public Material defaultMaterial;
    Material tintMaterial;
    Color materialBaseColor;
    Color materialTintColor;
    float tintFadeSpeed = 3f;

    void Awake() {
        animator = gameObject.GetComponent<Animator>();
        cameraShake = Camera.main.GetComponent<CameraShake>();
        UI = FindObjectOfType<UIDisplay>();

        if (gameObject.tag == "Enemy") {
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

            if (animator == null) {
                Debug.LogError("Animator is null");
            }

            if (spriteRenderer == null) {
                Debug.LogError("SpriteRenderer is null.");
            }

            tintMaterial = spriteRenderer.material;
        }
        
        if (cameraShake == null) {
            Debug.LogError("CameraShake is null.");
        }
        
        if (UI == null) {
            Debug.LogError("UI is null.");
        }
    }

    void Start() {
        AddAudioSource();
    }

    void Update() {
        if (source != null) {
            source.volume = volume;
        }

        if (transform.tag == "Enemy" && (materialTintColor.a > 0 || materialBaseColor != Color.white)) {
            ResetColor(tintFadeSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        DamageDealer damageDealer = collision.GetComponent<DamageDealer>();

        if (damageDealer != null) {
            TakeDamage(damageDealer.GetDamage());
            damageDealer.Hit(gameObject);

            if (isPlayer) {
                ShakeCamera();
                UI.UpdateSlider(damageDealer.GetDamage());
            }
        }
    }

    void AddAudioSource() {
        gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
    }

    void TakeDamage(int damageDealt) {
        health -= damageDealt;
        materialBaseColor = new Color(1, 1, 0.5f, 1);
        materialTintColor = new Color(1, 0, 0, 1f);

        if (health <= 0) {
            Collider2D collider = GetComponent<Collider2D>();

            collider.enabled = false;
            if (gameObject.tag == "Enemy") {
                ResetMaterial();
                animator.SetTrigger("OnDeath");
                isDead = true;
                source.PlayOneShot(explosionClip, volume);
            }
            Destroy(gameObject, 1f);
        }
        else {
            if (gameObject.tag == "Enemy") {
                SetTintColor(materialBaseColor, materialTintColor);
            }
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

    void ResetColor(float fadeSpeed = 0) {
        materialBaseColor.r = Mathf.Clamp01(materialBaseColor.r + tintFadeSpeed * Time.deltaTime);
        materialBaseColor.g = Mathf.Clamp01(materialBaseColor.g + tintFadeSpeed * Time.deltaTime);
        materialBaseColor.b = Mathf.Clamp01(materialBaseColor.b + tintFadeSpeed * Time.deltaTime);
        materialBaseColor.a = Mathf.Clamp01(materialBaseColor.a + tintFadeSpeed * Time.deltaTime);
        materialTintColor.a = Mathf.Clamp01(materialTintColor.a - tintFadeSpeed * Time.deltaTime);

        tintMaterial.SetColor("_Color", materialBaseColor);
        tintMaterial.SetColor("_Tint", materialTintColor);
    }

    void SetTintColor(Color baseColor, Color tintColor) {
        materialBaseColor = baseColor;
        materialTintColor = tintColor;

        tintMaterial.SetColor("_Color", materialBaseColor);
        tintMaterial.SetColor("_Tint", materialTintColor);
    }

    void ResetMaterial() {
        ResetColor();
        spriteRenderer.material = defaultMaterial;
    }
}
