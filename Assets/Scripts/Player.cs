using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour {
    [Header("Player")]
    [SerializeField] float moveSpeed = 13f;

    [Header("Shooting Audio")]
    [SerializeField] AudioClip shootingClip;
    [SerializeField][Range(0f, 1f)] float shootingVolume = 0.15f;
    [SerializeField] public float audioReplayDelay;
    AudioSource source { get { return GetComponent<AudioSource>(); } }
    static AudioPlayer instance;
    static float audioOffset = 0.78547253f;
    static float shootingEndOffset = 1.23234066f;

    Rigidbody2D playerRigidbody;
    Vector2 rawInput;

    float paddingLeft, paddingRight, paddingTop, paddingBottom = 0.9f;
    Vector2 minBounds;
    Vector2 maxBounds;

    [SerializeField] Shooter shooter;

    void Start() {
        InitBounds();
        AddAudioSource();

        playerRigidbody = GetComponent<Rigidbody2D>();

        if (playerRigidbody == null) {
            Debug.LogError("playerRigidBody is null.");
        }
    }

    void Update() {
        Move();
        
        if (source != null) {
            source.volume = shootingVolume;
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            StartCoroutine(playAudio());
        }
        else if (Input.GetKeyUp(KeyCode.Space)) {
            StopCoroutine(playAudio());
            PlayShootingEnd();
        }
    }

    public void AddAudioSource() {
        gameObject.AddComponent<AudioSource>();
        source.clip = shootingClip;
        source.playOnAwake = false;
        source.time = audioOffset;
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

    public void PlayShootingClip() {
        if (source != null) {
            source.time = audioOffset;

            if (shootingClip != null) {
                if (source.isPlaying) {
                    source.Stop();
                }
                source.Play();
            }
        }
    }

    public bool IsPlaying() {
        return source.isPlaying;
    }

    public void PlayShootingEnd() {
        source.time = shootingEndOffset;
        source.Play();
    }

    IEnumerator playAudio() {
        if (!IsPlaying()) {
            PlayShootingClip();
            yield return null;
        }

        while (shooter.isFiring) {
            yield return new WaitForSeconds(audioReplayDelay);
            if (!shooter.isFiring) break;

            PlayShootingClip();
        }
    }
}
