using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveBurnWarningUI : MonoBehaviour {

    [SerializeField] private CounterStove counterStove;

    private void Start() {
        counterStove.OnProgressChanged += CounterStove_OnProgressChanged;
        Hide();
    }

    private void CounterStove_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e) {
        bool showWarning = counterStove.IsFried() && e.progressNormalized >= counterStove.GetBurnWarningThreshold();

        if (showWarning) {
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
