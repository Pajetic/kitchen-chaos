using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterStoveVisual : MonoBehaviour {

    [SerializeField] CounterStove counterStove;
    [SerializeField] private GameObject stoveOnGameObject;
    [SerializeField] private GameObject stoveOnParticlesGameObject;

    private void Start() {
        counterStove.OnCookingStateChanged += OnCookingStateChanged;
    }

    private void OnCookingStateChanged(object sender, CounterStove.OnCookingStateChangedEventArgs e) {
        if (e.isCooking) {
            stoveOnGameObject.SetActive(true);
            stoveOnParticlesGameObject.SetActive(true);
        } else {
            stoveOnGameObject.SetActive(false);
            stoveOnParticlesGameObject.SetActive(false);
        }
    }
}

