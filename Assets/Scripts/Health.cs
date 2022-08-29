using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {
    [SerializeField] int health = 50;

    private void OnTriggerEnter2D(Collider2D collision) {
        DamageDealer damageDealer = collision.GetComponent<DamageDealer>();

        if (damageDealer != null) {
            TakeDamage(damageDealer.GetDamage());
            damageDealer.Hit();
        }
    }

    void TakeDamage(int damageDealt) {
        health -= damageDealt;
        if (health <= 0) {
            Destroy(gameObject);
        }
    }
}
