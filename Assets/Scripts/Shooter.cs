using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Shooter : MonoBehaviour {
    enum ShooterType {
        Player,
        UFO,
        MiniUFO,
        TinyUFOTypeA,
        TinyUFOTypeB
    }

    [Header("General Variables")]
    [SerializeField] GameObject projectilePrefab;

    [Header("Player Variables")]
    [SerializeField] float playerFireRate = 0.01f;
    [SerializeField] float playerBasicAttackSpeed = 20f;
    
    [Header("Enemy Variables")]
    [SerializeField] float tinyUFOProjectileSpeed = 5f;
    [SerializeField] bool useAI;
    [SerializeField] ShooterType shooterType;
    float timeToNextProjectile = 4f;
    public bool isDead;

    public bool isFiring;
    private int playerBasicAttackOffsetIndex = 0;
    Vector2 minBounds;
    Vector2 maxBounds;
    float padding = 0.9f;
    Coroutine firingCoroutine;
    Animator shooterAnimator;
    Health health;

    void Start() {
        health = gameObject.transform.parent.gameObject.tag == "Enemy" ? gameObject.transform.parent.GetComponent<Health>() :
                                                                        gameObject.transform.parent.parent.GetComponent<Health>();
        shooterAnimator = transform.GetComponent<Animator>();

        if (shooterAnimator == null) {
            Debug.LogError("Shooter Animator is null.");
        }
        if (health == null) {
            Debug.LogError("Health is null");
        }

        if (useAI) {
            isFiring = true;
            InitBounds();
            InvokeRepeating("EnemyFire", 0f, timeToNextProjectile);
        }
    }

    void Update() {
        if (!useAI) {
            PlayerFire();
        }
    }

    void PlayerFire() {
        if (isFiring && firingCoroutine == null && !health.IsDead()) {
            shooterAnimator.SetBool("isShooting", isFiring);
            firingCoroutine = StartCoroutine(PlayerFireBasicAttack());
        }
        else if (!isFiring && firingCoroutine != null || health.IsDead()) {
            shooterAnimator.SetBool("isShooting", isFiring);
            StopCoroutine(firingCoroutine);
            firingCoroutine = null;
        }
    }

    void EnemyFire() {
        switch (shooterType) {
            case ShooterType.Player:
                break;
            case ShooterType.UFO:
                break;
            case ShooterType.MiniUFO:

                break;
            case ShooterType.TinyUFOTypeA:
                StartCoroutine(EnemyStartCharging());
                break;
            case ShooterType.TinyUFOTypeB:
                break;

            default:
                break;
        }
    }

    void InitBounds() {
        Camera mainCamera = Camera.main;
        minBounds = mainCamera.ViewportToWorldPoint(new Vector2(0, 0));
        maxBounds = mainCamera.ViewportToWorldPoint(new Vector2(1, 1));
    }

    bool isOnScreen() {
        if (Mathf.Clamp(transform.position.x, minBounds.x + padding, maxBounds.x - padding) == transform.position.x &&
            Mathf.Clamp(transform.position.y, minBounds.y + padding, maxBounds.y - padding) == transform.position.y) {
            return true;
        }
        return false;
    }

    IEnumerator PlayerFireBasicAttack() {
        while (!useAI) {
            float HorizontalOffset = 1.5f;
            int rounds = 4;
            float roundSpeed = 0.025f;

            while (rounds --> 0) {
                Vector3 firePosition = new Vector3(transform.position.x + GetPlayerBasicAttackVerticalOffset(),
                                                   transform.position.y + HorizontalOffset,
                                                   transform.position.z);

                InstantiatePlayerBasicAttack(firePosition);
                yield return new WaitForSeconds(roundSpeed);
            }
            yield return new WaitForSeconds(playerFireRate);
        }
    }

    /**
     * Returns 3 different x position offset for basic attack starting point in rotation of 3 positions.
     */
    float GetPlayerBasicAttackVerticalOffset() {
        float[] positionOffsets = { -0.15f, 0f, 0.2f };
        
        if (playerBasicAttackOffsetIndex > 2) {
            playerBasicAttackOffsetIndex = 0;
        }

        return positionOffsets[playerBasicAttackOffsetIndex++];
    }

    void InstantiatePlayerBasicAttack(Vector3 firePosition) {
        GameObject instance = Instantiate(projectilePrefab,
                                              firePosition,
                                              projectilePrefab.transform.rotation);

        Rigidbody2D rb = instance.GetComponent<Rigidbody2D>();

        if (rb != null) {
            rb.velocity = transform.up * playerBasicAttackSpeed;
        }
    }

    IEnumerator EnemyStartCharging() {
        while (!isOnScreen()) {
            yield return new WaitForEndOfFrame();
        }
        if (isOnScreen()) {
            if (health.IsDead()) {
                shooterAnimator.ResetTrigger("isShooting");
                yield return null;
            }
            
            shooterAnimator.SetTrigger("isShooting");
        }
    }

    /**
     * Animation Event will call this function at the end of enemy charging animation.
     */
    public void EnemyReadyToShoot() {
        GameObject instance = Instantiate(projectilePrefab,
                                          transform.position,
                                          projectilePrefab.transform.rotation);
        Rigidbody2D rb = instance.GetComponent<Rigidbody2D>();

        if (rb != null) {
            Player player = FindObjectOfType<Player>();

            if (player != null && (shooterType == ShooterType.TinyUFOTypeA || shooterType == ShooterType.TinyUFOTypeB)) {
                Vector3 playerPos = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);

                rb.velocity = (player.transform.position - instance.transform.position).normalized * tinyUFOProjectileSpeed;
            }
            else {
                rb.velocity = -transform.up * tinyUFOProjectileSpeed;
            }
        }
    }
}
