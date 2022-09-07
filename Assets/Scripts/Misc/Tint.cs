using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tint : MonoBehaviour {
    [Header("Health Damage Tint")]
    SpriteRenderer spriteRenderer;
    public Material defaultMaterial;
    Material tintMaterial;
    Color materialBaseColor;
    Color materialTintColor;
    float tintFadeSpeed = 12f;

    [Header("Player Tint")]
    public static Color playerBaseColor = new Color(1, 1, 1, 1);
    public static Color playerTintColor = new Color(1, 1, 1, 0.25f);

    [Header("Enemy Tint")]
    public static Color enemyBaseColor = new Color(1, 1, 0.5f, 1);
    public static Color enemyTintColor = new Color(1, 0, 0, 1f);

    void Awake() {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        if (spriteRenderer == null) {
            Debug.LogError("SpriteRenderer is null.");
        }

        tintMaterial = spriteRenderer.material;
    }

    // Update is called once per frame
    void Update() {
        if (materialTintColor.a > 0 || materialBaseColor != Color.white) {
            ResetColor(tintFadeSpeed);
        }
    }

    void ResetColor(float fadeSpeed = 0) {
        materialBaseColor.r = Mathf.Clamp01(materialBaseColor.r + tintFadeSpeed * Time.deltaTime);
        materialBaseColor.g = Mathf.Clamp01(materialBaseColor.g + tintFadeSpeed * Time.deltaTime);
        materialBaseColor.b = Mathf.Clamp01(materialBaseColor.b + tintFadeSpeed * Time.deltaTime);
        materialBaseColor.a = Mathf.Clamp01(materialBaseColor.a + tintFadeSpeed * Time.deltaTime);
        materialTintColor.a = Mathf.Clamp01(materialTintColor.a - tintFadeSpeed * Time.deltaTime);

        tintMaterial.SetColor("_Color", materialBaseColor);
        tintMaterial.SetColor("_Tint", materialTintColor);
    }

    public void SetTintColor(Color baseColor, Color tintColor) {
        materialBaseColor = baseColor;
        materialTintColor = tintColor;

        tintMaterial.SetColor("_Color", materialBaseColor);
        tintMaterial.SetColor("_Tint", materialTintColor);
    }

    public void ResetMaterial() {
        ResetColor();
        spriteRenderer.material = defaultMaterial;
    }

    public void FlickerTint(Color baseColor, Color tintColor, float seconds) {
        while (seconds > 0) {
            SetTintColor(baseColor, tintColor);

            while (materialTintColor.a > 0 || materialBaseColor != Color.white) {
                ResetColor(tintFadeSpeed);
            }
            seconds -= Time.deltaTime;
        }
    }
}
