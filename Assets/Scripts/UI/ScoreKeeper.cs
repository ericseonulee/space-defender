using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour {
    NumberRenderer numberRendering;
    public int score;
    public int maxNumber = 99999999;

    void Start() {
        numberRendering = GetComponent<NumberRenderer>();

        if (numberRendering == null) {
            Debug.LogError("Number Renderer is null.");
        }
        numberRendering.RenderNumber(0);
    }

    public int GetScore() {
        return score;
    }

    public void ModifyScore(int value) {
        if (score >= maxNumber) { return; }
        score += value;
        Mathf.Clamp(score, 0, int.MaxValue);
        numberRendering.RenderNumber(score);
    }

    public void ResetScore() {
        score = 0;
    }
}
