using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDisplay : MonoBehaviour {
    [Header("Health")]
    [SerializeField] Slider healthSlider;
    [SerializeField] Health playerHealth;

    void Start() {
        healthSlider.maxValue = playerHealth.GetHealth();
    }

    public void UpdateSlider(int damage) {
        if (playerHealth.GetHealth() > 0) {
            StartCoroutine(SliderAnimation(damage));
        }
        
        if (playerHealth.GetHealth() == 1) {
            StartCoroutine(LowHealthWarning());
        }
    }

    IEnumerator SliderAnimation(int damage) {
        float maxFluctuation = 1.6f;
        float rateOfConvergence = 0.1f;
        float flickerTime = 0.025f;
        int flickerCount = 1;

        while (maxFluctuation > 0) {            
            healthSlider.value -= damage * maxFluctuation;
            maxFluctuation -= rateOfConvergence;

            yield return new WaitForSeconds(flickerTime);

            healthSlider.value += damage * maxFluctuation;
            maxFluctuation -= rateOfConvergence;

            yield return new WaitForSeconds(flickerTime);
            flickerCount++;
        }

        healthSlider.value = playerHealth.GetHealth();
    }

    IEnumerator LowHealthWarning() {
        //DF2E03
        Transform background = transform.Find("Background");
        Transform fillArea = transform.Find("Fill Area");
        Color originalColor = new Color(1, 1, 1);
        Color warningColor = new Color(0.8745098f, 0.1803922f, 0.01176471f);


        if (background != null && fillArea != null) {
            Image backgroundImage = background.GetComponent<Image>();
            Image fillImage = fillArea.GetComponentInChildren<Image>();

            if (backgroundImage != null && fillImage != null) {
                while (playerHealth.GetHealth() > 0 && playerHealth.GetHealth() < 2) {
                    backgroundImage.color = warningColor;
                    fillImage.color = warningColor;
                    yield return new WaitForSeconds(0.2f);

                    backgroundImage.color = originalColor;
                    fillImage.color = originalColor;
                    yield return new WaitForSeconds(0.6f);
                }
            }
        }
        yield return new WaitForEndOfFrame();
    }
}
