using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour {

    public static OptionsUI Instance {  get; private set; }

    [SerializeField] private Button soundEffectsButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button moveUpButton;
    [SerializeField] private Button moveDownButton;
    [SerializeField] private Button moveLeftButton;
    [SerializeField] private Button moveRightButton;
    [SerializeField] private Button interactButton;
    [SerializeField] private Button interactAltButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button gamepadInteractButton;
    [SerializeField] private Button gamepadInteractAltButton;
    [SerializeField] private Button gamepadPauseButton;
    [SerializeField] private TextMeshProUGUI soundEffectsText;
    [SerializeField] private TextMeshProUGUI musicText;
    [SerializeField] private TextMeshProUGUI moveUpText;
    [SerializeField] private TextMeshProUGUI moveDownText;
    [SerializeField] private TextMeshProUGUI moveLeftText;
    [SerializeField] private TextMeshProUGUI moveRightText;
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private TextMeshProUGUI interactAltText;
    [SerializeField] private TextMeshProUGUI pauseText;
    [SerializeField] private TextMeshProUGUI gamepadInteractText;
    [SerializeField] private TextMeshProUGUI gamepadInteractAltText;
    [SerializeField] private TextMeshProUGUI gamepadPauseText;
    [SerializeField] private Transform rebindKeyUI;
    private GameInput gameInput;
    private Action onCloseButtonAction;



    private void Awake() {
        if (Instance != null) {
            Debug.LogError("Attempting to create multiple instances of OptionsUI.");
            Destroy(this);
        } else {
            Instance = this;
        }

        soundEffectsButton.onClick.AddListener(() => {
            SoundManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        musicButton.onClick.AddListener(() => {
            MusicManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        closeButton.onClick.AddListener(() => {
            Hide();
            onCloseButtonAction?.Invoke();
        });

        moveUpButton.onClick.AddListener(() => {
            RebindKey(GameInput.Binding.MoveUp);
        });
        moveDownButton.onClick.AddListener(() => {
            RebindKey(GameInput.Binding.MoveDown);
        });
        moveLeftButton.onClick.AddListener(() => {
            RebindKey(GameInput.Binding.MoveLeft);
        });
        moveRightButton.onClick.AddListener(() => {
            RebindKey(GameInput.Binding.MoveRight);
        });
        interactButton.onClick.AddListener(() => {
            RebindKey(GameInput.Binding.Interact);
        });
        interactAltButton.onClick.AddListener(() => {
            RebindKey(GameInput.Binding.InteractAlt);
        });
        pauseButton.onClick.AddListener(() => {
            RebindKey(GameInput.Binding.Pause);
        });
        gamepadInteractButton.onClick.AddListener(() => {
            RebindKey(GameInput.Binding.Gamepad_Interact);
        });
        gamepadInteractAltButton.onClick.AddListener(() => {
            RebindKey(GameInput.Binding.Gamepad_InteractAlt);
        });
        gamepadPauseButton.onClick.AddListener(() => {
            RebindKey(GameInput.Binding.Gamepad_Pause);
        });
    }

    private void Start() {
        KitchenGameManager.Instance.OnGameUnpause += GameManager_OnGameUnpause;
        gameInput = GameInput.Instance;

        UpdateVisual();
        Hide();
        HideRebindUI();
    }

    private void GameManager_OnGameUnpause(object sender, System.EventArgs e) {
        Hide();
    }

    private void UpdateVisual() {
        soundEffectsText.SetText("Sound Effects: " + SoundManager.Instance.GetVolume().ToString());
        musicText.SetText("Music: " + MusicManager.Instance.GetVolume().ToString());

        moveUpText.SetText(gameInput.GetBindingText(GameInput.Binding.MoveUp));
        moveDownText.SetText(gameInput.GetBindingText(GameInput.Binding.MoveDown));
        moveLeftText.SetText(gameInput.GetBindingText(GameInput.Binding.MoveLeft));
        moveRightText.SetText(gameInput.GetBindingText(GameInput.Binding.MoveRight));
        interactText.SetText(gameInput.GetBindingText(GameInput.Binding.Interact));
        interactAltText.SetText(gameInput.GetBindingText(GameInput.Binding.InteractAlt));
        pauseText.SetText(gameInput.GetBindingText(GameInput.Binding.Pause));
        gamepadInteractText.SetText(gameInput.GetBindingText(GameInput.Binding.Gamepad_Interact));
        gamepadInteractAltText.SetText(gameInput.GetBindingText(GameInput.Binding.Gamepad_InteractAlt));
        gamepadPauseText.SetText(gameInput.GetBindingText(GameInput.Binding.Gamepad_Pause));
    }

    public void Show(Action onCloseButtonAction) {

        this.onCloseButtonAction = onCloseButtonAction;
        gameObject.SetActive(true);
        closeButton.Select();
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

    private void ShowRebindUI() {
        rebindKeyUI.gameObject.SetActive(true);
    }

    private void HideRebindUI() {
        rebindKeyUI.gameObject.SetActive(false);
    }

    private void RebindKey(GameInput.Binding binding) {
        ShowRebindUI();
        GameInput.Instance.Rebind(binding, () => {
            HideRebindUI();
            UpdateVisual();
        });
    }
}

