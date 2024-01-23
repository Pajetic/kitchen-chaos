using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterStoveSound : MonoBehaviour {

    [SerializeField] private CounterStove counterStove;
    private AudioSource audioSource;
    private bool playWarningSound = false;
    private float warningSoundTimer = 0f;
    private float warningSoundTimerMax = 0.2f;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start() {
        counterStove.OnCookingStateChanged += Stove_OnCookingStateChanged;
        counterStove.OnProgressChanged += Stove_OnProgressChanged;
    }

    private void Update() {
        if (playWarningSound) {
            warningSoundTimer += Time.deltaTime;
            if (warningSoundTimer > warningSoundTimerMax) {
                warningSoundTimer = 0f;
                SoundManager.Instance.PlayWarningSound(counterStove.transform.position);
            }
        }
    }

    private void Stove_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e) {
        playWarningSound = counterStove.IsFried() && e.progressNormalized >= counterStove.GetBurnWarningThreshold();
    }

    private void Stove_OnCookingStateChanged(object sender, CounterStove.OnCookingStateChangedEventArgs e) {
        if (e.isCooking) {
            audioSource.Play();
        } else {
            audioSource.Stop();
        }
    }
}
