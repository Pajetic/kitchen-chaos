using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Player;

public class SelectedCounterVisual : MonoBehaviour {

    [SerializeField] private CounterBase thisCounter;
    [SerializeField] private GameObject[] selectedCounterVisuals;

    private void Start() {
        Player.Instance.OnSelectedCounterChanged += PlayerOnSelectedCounterChanged;
    }

    private void PlayerOnSelectedCounterChanged(object sender, OnSelectedCounterChangedEventArgs e) {
        foreach (GameObject obj in selectedCounterVisuals) {
            obj.SetActive(e.selectedCounter == thisCounter);
        }
    }
}
