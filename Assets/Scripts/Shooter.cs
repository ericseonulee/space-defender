using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Shooter : MonoBehaviour {
    enum EnemyType {
        UFO,
        MiniUFO,
        TinyUFOTypeA,
        TinyUFOTypeB
    }

    [Header("General Variables")]
    [SerializeField] GameObject projectilePrefab;

    [Header("Player Variables")]
    [SerializeField] float playerFireRate = 0.2f;
    [SerializeField] float playerBasicAttackSpeed = 20f;
    
    [Header("Enemy Variables")]
    [SerializeField] float tinyUFOProjectileSpeed = 5f;
    [SerializeField] bool useAI;
    [SerializeField] EnemyType enemyType;

    public bool isFiring;
    private int positionIndex = 0;
    Vector2 minBounds;
    Vector2 maxBounds;
    float padding = 0.9f;
    Coroutine firingCoroutine;
    Animator shooterAnimator;

    void Start() {
        shooterAnimator = transform.GetComponent<Animator>();

        if (shooterAnimator == null) {
            Debug.Log("this object: " + gameObject.transform.parent.gameObject.name);
            Debug.LogError("Shooter Animator is null.");
        }

        if (useAI) {
            isFiring = true;
            InitBounds();
        }
    }

    void Update() {
        Fire();
    }

    void Fire() {
        if (isFiring && firingCoroutine == null) {
            if (useAI) {
                switch (enemyType) {
                    case EnemyType.UFO:
                        break;
                    case EnemyType.MiniUFO:

                        break;
                    case EnemyType.TinyUFOTypeA:
                        firingCoroutine = StartCoroutine(EnemyStartFiring(enemyType));
                        break;
                    case EnemyType.TinyUFOTypeB:
                        break;

                    default:
                        break;
                }

            }
            else {
                shooterAnimator.SetBool("isShooting", isFiring);
                firingCoroutine = StartCoroutine(PlayerFireBasicAttack());
            }
        }
        else if (!isFiring && firingCoroutine != null){
            shooterAnimator.SetBool("isShooting", isFiring);
            StopCoroutine(firingCoroutine);
            firingCoroutine = null;
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
            float[] positionOffsets = { -0.15f, 0f, 0.2f };

            if (positionIndex > 2) { positionIndex = 0; }
            
            Vector3 firePosition = new Vector3(transform.position.x + positionOffsets[positionIndex++],
                                               transform.position.y + 1.5f,
                                               transform.position.z);
            GameObject instance = Instantiate(projectilePrefab,
                                              firePosition,
                                              projectilePrefab.transform.rotation);
            Rigidbody2D rb = instance.GetComponent<Rigidbody2D>();

            if (rb != null) {
                rb.velocity = transform.up * playerBasicAttackSpeed;
            }
            yield return new WaitForSeconds(playerFireRate / 4);
        }
    }

    IEnumerator EnemyStartFiring(EnemyType enemyType) {
        Debug.Log(enemyType);
        while (isOnScreen() && useAI) {
            shooterAnimator.SetBool("isShooting", true);

            yield return new WaitForSeconds(0.7f);
            shooterAnimator.SetBool("isShooting", false);
            GameObject instance = Instantiate(projectilePrefab,
                                              transform.position,
                                              projectilePrefab.transform.rotation);
            Rigidbody2D rb = instance.GetComponent<Rigidbody2D>();

            if (rb != null) {
                if (enemyType == EnemyType.TinyUFOTypeA || enemyType == EnemyType.TinyUFOTypeB) {
                    Player player = FindObjectOfType<Player>();
                    Vector3 playerPos = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);

                    rb.velocity = (player.transform.position - instance.transform.position).normalized * tinyUFOProjectileSpeed;
                }
                else {
                    rb.velocity = -transform.up * tinyUFOProjectileSpeed;
                }
            }
            yield return new WaitForSeconds(2);
        }
    }
}