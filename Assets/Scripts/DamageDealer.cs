using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour {
    [SerializeField] int damage = 10;

    public int GetDamage() {
        return damage;
    }

    public void Hit(string tag) {
        if (tag == "Enemy") {
            CapsuleCollider2D collider = GetComponent<CapsuleCollider2D>();
            if (collider != null) {
                collider.enabled = false;
                RandomBasicAttackHitAnimator();
            }
        }
        else {
            Destroy(gameObject);
        }
    }

    /** 
     * Animation Event in BasicAttackHit will call this function at the end of animation.
     */
    public void DestroyAtTheEndOfAnim() {
        Destroy(gameObject);
    }

    void RandomBasicAttackHitAnimator() {
        Animator basicAttackHit = GetComponent<Animator>();

        if (basicAttackHit != null) {
            basicAttackHit.SetTrigger("BasicAttackHit" + Random.Range(1, 3).ToString());
        }
    }
}
