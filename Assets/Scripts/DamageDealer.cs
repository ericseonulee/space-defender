using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour {
    [SerializeField] int damage = 10;

    public int GetDamage() {
        return damage;
    }

    public void Hit(GameObject target) {
        if (target.tag == "Enemy") {
            CapsuleCollider2D collider = GetComponent<CapsuleCollider2D>();
            Rigidbody2D rigidBody2D = GetComponent<Rigidbody2D>();
            SpriteRenderer spriteRenderer = target.GetComponent<SpriteRenderer>();

            if (collider != null) {
                collider.enabled = false;
            }
            if (rigidBody2D != null) {
                rigidBody2D.velocity = Vector3.zero;
            }

            // Positioning the projectile within the sprite so it will look natural.
            if (spriteRenderer != null) {
                float spriteHeightInUnityUnit = spriteRenderer.sprite.rect.height / 30f; // Pixel per unit is 30.
                float midPointYStart;
                float midPointYEnd;

                midPointYStart = spriteHeightInUnityUnit / 4 * 2;
                midPointYEnd = spriteHeightInUnityUnit / 4 * 3;

                gameObject.transform.position = new Vector2(transform.position.x,
                                                            transform.position.y
                                                                + Random.Range(midPointYStart, midPointYEnd + Mathf.Epsilon));
            }
            RandomBasicAttackHitAnimator();
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
