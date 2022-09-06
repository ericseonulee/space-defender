using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour {
    [Header("Player")]
    [SerializeField] AudioClip slugDamageClip;

    public void PlaySlugDamageClipOneShot() {
        AudioSource.PlayClipAtPoint(slugDamageClip, Camera.main.transform.position, 0.05f);
    }
}
