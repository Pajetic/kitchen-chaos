using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterPlatesVisual : MonoBehaviour {

    [SerializeField] private CounterPlates counterPlates;
    [SerializeField] private GameObject plateVisualPrefab;
    [SerializeField] private Transform counterTop;
    private Stack<GameObject> plateStack;
    private float plateStackYOffset = 0.1f;

    private void Awake() {
        plateStack = new Stack<GameObject>();
    }

    private void Start() {
        counterPlates.OnPlateSpawned += AddDummyPlate;
        counterPlates.OnPlateRemoved += RemoveDummyPlate;
    }

    private void RemoveDummyPlate(object sender, EventArgs e) {
        Destroy(plateStack.Pop());
    }

    private void AddDummyPlate(object sender, EventArgs e) {
        GameObject plate = Instantiate(plateVisualPrefab, counterTop);
        plate.transform.localPosition = new Vector3(0, plateStackYOffset * plateStack.Count, 0);
        plateStack.Push(plate);
    }
}
