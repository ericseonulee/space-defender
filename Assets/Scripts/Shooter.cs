using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour {
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float projectileSpeed = 20f;
    [SerializeField] float projectileLifetime = 1f;
    [SerializeField] float fireRate = 0.2f;
    [SerializeField] bool isPlayer;

    public bool isPlayerFiring;
    private int positionIndex = 0;

    Coroutine firingCoroutine;
    Animator shooterAnimator;

    void Start() {
        shooterAnimator = transform.GetComponent<Animator>();

        if (shooterAnimator == null) {
            Debug.LogError("Shooter Animator is null.");
        }
    }

    void Update() {
        Fire();
    }

    //void enemyFire() {
    //    if (isFiring && Time.time > nextFire) {
    //        Instantiate(projectilePrefab, transform.position, Quaternion.identity);
    //        Destroy(gameObject, projectLifetime);
    //    }
    //}

    //void playerFire(Transform[] fireFrom) {
    //    for (int i = 0; i < fireFrom.Length; i++) {
    //        if (isFiring && Time.time > nextFire) {
    //            nextFire = Time.time + fireRate;
    //            Instantiate(projectilePrefab, fireFrom[i].position, Quaternion.identity);
    //            Destroy(gameObject, projectLifetime);
    //        }
    //    }
    //}

    void Fire() {
        Debug.Log(isPlayerFiring);
        if (isPlayerFiring && firingCoroutine == null) {
            shooterAnimator.SetBool("isShooting", isPlayerFiring);
            firingCoroutine = StartCoroutine(PlayerFireBasicAttack());
        }
        else if (!isPlayerFiring && firingCoroutine != null){
            shooterAnimator.SetBool("isShooting", isPlayerFiring);
            StopCoroutine(firingCoroutine);
            firingCoroutine = null;
        }
    }

    IEnumerator PlayerFireBasicAttack() {
        while (isPlayer) {
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
                rb.velocity = transform.up * projectileSpeed;
            }
            Destroy(instance, projectileLifetime);
            yield return new WaitForSeconds(fireRate / 4);
        }
    }
}
