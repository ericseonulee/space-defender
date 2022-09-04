using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudio : MonoBehaviour {
    [Header("Enemy")]
    [SerializeField] AudioClip explosionClip;
    [SerializeField] [Range(0f, 1f)] float enemyVolume = 0.2f;
    AudioSource source { get { return GetComponent<AudioSource>(); } }

    void Start() {
        AddAudioSource();
    }


    void Update() {
        if (source != null) {
            source.volume = enemyVolume;
        }
    }

    void AddAudioSource() {
        gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
    }
    
    public void PlayExplosionClipOneShot() {
        source.PlayOneShot(explosionClip, enemyVolume);
    }
}
