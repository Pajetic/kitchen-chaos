using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayingClockUI : MonoBehaviour {

    [SerializeField] private Image timerImage;
    KitchenGameManager gameManager;
    private bool timerActive = false;

    private void Start() {
        gameManager = KitchenGameManager.Instance;
        timerImage.fillAmount = 1f;
        gameManager.OnStateChanged += GameManager_OnStateChanged;
    }

    private void GameManager_OnStateChanged(object sender, System.EventArgs e) {
        timerActive = gameManager.IsGamePlaying();
    }

    private void Update() {
        if (timerActive) {
            timerImage.fillAmount = gameManager.GetGamePlayingTimerNormalized();
        }
    }

}

