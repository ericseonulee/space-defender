using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour {
    [Header("Player")]
    [SerializeField] AudioClip slugDamageClip;

    AudioSource source { get { return GetComponent<AudioSource>(); } }

    void Start() {
        AddAudioSource();
    }

    void AddAudioSource() {
        gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
    }

    public void PlaySlugDamageClipOneShot() {
        AudioSource.PlayClipAtPoint(slugDamageClip, Camera.main.transform.position, 0.05f);
    }
}
