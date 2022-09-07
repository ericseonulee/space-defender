using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScroller : MonoBehaviour {
    [SerializeField] Vector2 speed;
    
    Vector2 offset;
    Material material;


    void Awake() {
        material = GetComponent<SpriteRenderer>().material;

        if (material == null) {
            Debug.LogError("material is null.");
        }
    }

    void Update() {
        offset = speed * Time.deltaTime;
        material.mainTextureOffset += offset;
    }
}
