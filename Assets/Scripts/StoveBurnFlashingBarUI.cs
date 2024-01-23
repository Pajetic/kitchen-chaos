using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveBurnFlashingBarUI : MonoBehaviour {

    private const string IS_FLASHING = "IsFlashing";

    [SerializeField] private CounterStove counterStove;
    Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void Start() {
        counterStove.OnProgressChanged += CounterStove_OnProgressChanged;
        animator.SetBool(IS_FLASHING, false);
    }

    private void CounterStove_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e) {
        bool showWarning = counterStove.IsFried() && e.progressNormalized >= counterStove.GetBurnWarningThreshold();

        animator.SetBool(IS_FLASHING, showWarning);
    }


}
