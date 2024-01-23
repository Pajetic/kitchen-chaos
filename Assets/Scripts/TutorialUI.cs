using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TutorialUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI keyMoveUpText;
    [SerializeField] private TextMeshProUGUI keyMoveDownText;
    [SerializeField] private TextMeshProUGUI keyMoveLeftText;
    [SerializeField] private TextMeshProUGUI keyMoveRightText;
    [SerializeField] private TextMeshProUGUI keyInteractText;
    [SerializeField] private TextMeshProUGUI keyInteractAlternateText;
    [SerializeField] private TextMeshProUGUI keyPauseText;
    [SerializeField] private TextMeshProUGUI keyGamepadInteractText;
    [SerializeField] private TextMeshProUGUI keyGamepadInteractAlternateText;
    [SerializeField] private TextMeshProUGUI keyGamepadPauseText;

    private void Start() {
        GameInput.Instance.OnRebind += GameInput_OnRebind;
        KitchenGameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
        UpdateVisual();
        Show();
    }

    private void GameManager_OnStateChanged(object sender, System.EventArgs e) {
        if (KitchenGameManager.Instance.IsCountingDownToStart()) {
            Hide();
        }
    }

    private void GameInput_OnRebind(object sender, System.EventArgs e) {
        UpdateVisual();
    }

    private void UpdateVisual() {
        keyMoveUpText.SetText(GameInput.Instance.GetBindingText(GameInput.Binding.MoveUp));
        keyMoveDownText.SetText(GameInput.Instance.GetBindingText(GameInput.Binding.MoveDown));
        keyMoveLeftText.SetText(GameInput.Instance.GetBindingText(GameInput.Binding.MoveLeft));
        keyMoveRightText.SetText(GameInput.Instance.GetBindingText(GameInput.Binding.MoveRight));
        keyInteractText.SetText(GameInput.Instance.GetBindingText(GameInput.Binding.Interact));
        keyInteractAlternateText.SetText(GameInput.Instance.GetBindingText(GameInput.Binding.InteractAlt));
        keyPauseText.SetText(GameInput.Instance.GetBindingText(GameInput.Binding.Pause));
        keyGamepadInteractText.SetText(GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_Interact));
        keyGamepadInteractAlternateText.SetText(GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_InteractAlt));
        keyGamepadPauseText.SetText(GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_Pause));
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}
