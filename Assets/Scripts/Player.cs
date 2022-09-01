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
    AudioPlayer audioPlayer;

    void Awake() {
        audioPlayer = FindObjectOfType<AudioPlayer>();

        if (audioPlayer == null) {
            Debug.LogError("audioPlayer is null.");
        }
    }

    void Start() {
        InitBounds();
        playerRigidbody = GetComponent<Rigidbody2D>();

        if (playerRigidbody == null) {
            Debug.LogError("playerRigidBody is null.");
        }
    }

    void Update() {
        Move();

        if (Input.GetKeyDown(KeyCode.Space)) {
            Debug.Log("Shooting");
            StartCoroutine(playAudio());
        }
        else if (Input.GetKeyUp(KeyCode.Space)) {
            Debug.Log("Shooting End");
            StopCoroutine(playAudio());
            audioPlayer.PlayShootingEnd();
        }
    }

    void InitBounds() {
        Camera mainCamera = Camera.main;
        minBounds = mainCamera.ViewportToWorldPoint(new Vector2(0, 0));
        maxBounds = mainCamera.ViewportToWorldPoint(new Vector2(1, 1));
    }

    void OnFire(InputValue value) {
        if (shooter != null) {
            shooter.isFiring = value.isPressed;
        }
    }

    void OnMove(InputValue value) {
        rawInput = value.Get<Vector2>();
    }

    void Move() {
        Vector2 playerVelocity = new Vector2(rawInput.x * moveSpeed, rawInput.y * moveSpeed);// * Time.deltaTime;
        playerRigidbody.velocity = playerVelocity;

        Vector2 newPos = new Vector2();

        newPos.x = Mathf.Clamp(transform.position.x, minBounds.x + paddingLeft, maxBounds.x - paddingRight);
        newPos.y = Mathf.Clamp(transform.position.y, minBounds.y + paddingBottom, maxBounds.y - paddingTop);

        transform.position = newPos;
    }

    /**
     * Returns 1 if moving right, -1 if moving left, 0 if not moving
     */
    public int PlayerMovingHorizontal() {
        if (Mathf.Abs(playerRigidbody.velocity.x) > Mathf.Epsilon) {

            if (playerRigidbody.velocity.x > 0) {
                return 1;
            }
            else {
                return -1;
            }
        }
        else {
            return 0;
        }

    }

    IEnumerator playAudio() {
        if (!audioPlayer.IsPlaying()) {
            audioPlayer.PlayShootingClip();
            yield return null;
        }

        while (shooter.isFiring) {
            yield return new WaitForSeconds(audioPlayer.audioReplayDelay);
            if (!shooter.isFiring) break;

            audioPlayer.PlayShootingClip();
        }
    }
}
