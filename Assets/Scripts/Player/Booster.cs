using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : MonoBehaviour {
    Player player;
    Transform boosterResidue1;
    Transform boosterResidue2;
    Animator boosterAnimator;
    Animator residueAnimator1;
    Animator residueAnimator2;
    Vector2 residueAnimator1InitialPos;
    Vector2 residueAnimator2InitialPos;
    float playerHorizontalMovement;
    bool boosterInit;

    void Start() {
        player = FindObjectOfType<Player>();
        boosterAnimator = GetComponent<Animator>();

        boosterResidue1 = gameObject.transform.GetChild(0);
        boosterResidue2 = gameObject.transform.GetChild(1);
        residueAnimator1 = boosterResidue1.GetComponent<Animator>();
        residueAnimator2 = boosterResidue2.GetComponent<Animator>();

        if (player == null) {
            Debug.LogError("player is null.");
        }

        if (boosterAnimator == null) {
            Debug.LogError("boosterAnimator is null.");
        }

        if (residueAnimator1 == null) {
            Debug.LogError("residueAnimator1 is null.");
        }

        if (residueAnimator2 == null) {
            Debug.LogError("residueAnimator2 is null.");
        }

        residueAnimator1InitialPos = boosterResidue1.transform.position;
        residueAnimator2InitialPos = boosterResidue2.transform.position;
        Invoke("BoosterInit", 4f);
    }

    void Update() {
        playerHorizontalMovement = player.PlayerMovingHorizontal();

        if (boosterInit) {
            UpdateBoosterAnimation();
        }
    }

    void UpdateBoosterAnimation() {
        if (playerHorizontalMovement == 1) {
            boosterAnimator.SetBool("playerMoveRight", true);
        }
        else if (playerHorizontalMovement == -1) {
            boosterAnimator.SetBool("playerMoveLeft", true);
        }
        else if (playerHorizontalMovement == 0) {

            boosterAnimator.SetBool("playerMoveLeft", false);
            boosterAnimator.SetBool("playerMoveRight", false);
        }
    }

    void BoosterInit() {
        boosterAnimator.SetTrigger("boosterInit");
        player.SetPlayerMobility(true);
        boosterInit = true;

        

        if (residueAnimator1 != null && residueAnimator2 != null) {
            residueAnimator1.SetTrigger("init");
            residueAnimator2.SetTrigger("init");
        }
    }
}
