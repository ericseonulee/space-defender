using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    Vector3 minBounds;
    Vector3 maxBounds;
    Camera mainCamera;

    void Start() {
        mainCamera = Camera.main;
        InitBounds();   
    }

    void Update() {
        DestroyOnOutOfBound();
    }

    void InitBounds() {
        minBounds = mainCamera.ViewportToWorldPoint(new Vector2(0, 0));
        maxBounds = mainCamera.ViewportToWorldPoint(new Vector2(1, 1));
    }

    void DestroyOnOutOfBound() {
        Vector2 screenPosition = mainCamera.WorldToScreenPoint(transform.position);
        
        if (Mathf.Clamp(transform.position.x, minBounds.x, maxBounds.x) != transform.position.x ||
            Mathf.Clamp(transform.position.y, minBounds.y, maxBounds.y) != transform.position.y) {

            Destroy(gameObject);
        }
    }
}
