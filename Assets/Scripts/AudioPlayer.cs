using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AudioPlayer : MonoBehaviour {
    [Header("Shooting")]
    [SerializeField] AudioClip shootingClip;
    [SerializeField][Range(0f, 1f)] float shootingVolume = 1f;
    [SerializeField] public float audioReplayDelay;
    AudioSource source { get { return GetComponent<AudioSource>(); } }
    static AudioPlayer instance;
    static float audioOffset = 0.78547253f;
    static float shootingEndOffset = 1.23234066f;

    void Awake() {
        ManageSingleton();
    }

    void ManageSingleton() {
        if (instance != null) {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }



    void Start() {
        AddAudioSource();
    }

    void Update() {
        if (source != null) {
            source.volume = shootingVolume;
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

    public void StopPlaying() {
        source.Stop();
    }
    public void PlayShootingEnd() {
        source.time = shootingEndOffset;
        source.Play();
    }

    public void AddAudioSource() {
        gameObject.AddComponent<AudioSource>();
        source.clip = shootingClip;
        source.playOnAwake = false;
        source.time = audioOffset;
    }
}
