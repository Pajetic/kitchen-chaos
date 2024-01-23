using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI recipesDeliveredText;
    KitchenGameManager gameManager;

    private void Start() {
        gameManager = KitchenGameManager.Instance;
        gameManager.OnStateChanged += GameManager_OnStateChanged;
        Hide();
    }

    private void GameManager_OnStateChanged(object sender, System.EventArgs e) {
        if (gameManager.IsGameOver()) {
            recipesDeliveredText.SetText(DeliveryManager.Instance.GetSuccessfulRecipeCount().ToString());
            Show();
        } else {
            Hide();
        }
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}

