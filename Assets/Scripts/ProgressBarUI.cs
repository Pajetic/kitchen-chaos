using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour {

    [SerializeField] private Image barFill;
    [SerializeField] private GameObject hasProgressGameObject;
    private IHasProgress hasProgress;

    private void Start() {
        hasProgress = hasProgressGameObject.GetComponent<IHasProgress>();
        if (hasProgress == null) {
            Debug.LogError(hasProgressGameObject + " does not have a component that implements IHasProgress.");
        }
        hasProgress.OnProgressChanged += UpdateProgressBar;
        barFill.fillAmount = 0f;
        Hide();
    }

    private void UpdateProgressBar(object sender, IHasProgress.OnProgressChangedEventArgs e) {
        barFill.fillAmount = e.progressNormalized;
        if (e.progressNormalized == 0f || e.progressNormalized == 1f) {
            Hide();
        } else {
            Show();
        }
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}
