using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour {
    [SerializeField] float moveSpeed = 13f;

    Rigidbody2D playerRigidbody;
    Vector2 rawInput;

    [SerializeField] float paddingLeft;
    [SerializeField] float paddingRight;
    [SerializeField] float paddingTop;
    [SerializeField] float paddingBottom;
    Vector2 minBounds;
    Vector2 maxBounds;

    [SerializeField] Shooter shooter;

    void Start() {
        InitBounds();
    }

    void Update() {
        Move();
    }

    void InitBounds() {
        Camera mainCamera = Camera.main;
        minBounds = mainCamera.ViewportToWorldPoint(new Vector2(0, 0));
        maxBounds = mainCamera.ViewportToWorldPoint(new Vector2(1, 1));
    }

    void OnFire(InputValue value) {
        if (shooter != null) {
            shooter.isPlayerFiring = value.isPressed;
            
        }
    }

    void OnMove(InputValue value) {
        rawInput = value.Get<Vector2>();
    }

    void Move() {
        Vector2 delta = rawInput * moveSpeed * Time.deltaTime; // * Time.deltaTime make the movement framerate independent
        Vector2 newPos = new Vector2();

        newPos.x = Mathf.Clamp(transform.position.x + delta.x, minBounds.x + paddingLeft, maxBounds.x - paddingRight);
        newPos.y = Mathf.Clamp(transform.position.y + delta.y, minBounds.y + paddingBottom, maxBounds.y - paddingTop);

        transform.position = newPos;
    }
}
