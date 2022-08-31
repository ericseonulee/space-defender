using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {
    [SerializeField] int health = 50;
    [SerializeField] bool isDead;
    Animator animator;
    
    void Awake() {
        animator = gameObject.GetComponent<Animator>();

        if (gameObject.tag == "Enemy" && animator == null) {
            Debug.LogError("Animator is null");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        DamageDealer damageDealer = collision.GetComponent<DamageDealer>();

        if (damageDealer != null) {
            TakeDamage(damageDealer.GetDamage());
            damageDealer.Hit(gameObject);
        }
    }

    void TakeDamage(int damageDealt) {
        health -= damageDealt;
        if (health <= 0) {
            Debug.Log("GameObject name: " + gameObject.name);
            Debug.Log("GameObject tag: " + gameObject.tag);
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
}
