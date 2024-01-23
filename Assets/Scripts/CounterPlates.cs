using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterPlates : CounterBase {

    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;

    [SerializeField] private KitchenObjectsSO kitchenObjectsSO;
    private float plateSpawnTimer;
    private float plateSpawnTimerMax = 4f;
    private int plateSpawnCount = 0;
    private int plateSpawnCountMax = 5;

    private void Update() {
        if (KitchenGameManager.Instance.IsGamePlaying()) {
            plateSpawnTimer += Time.deltaTime;
            if (plateSpawnTimer >= plateSpawnTimerMax) {
                plateSpawnTimer = 0f;
                if (plateSpawnCount < plateSpawnCountMax) {
                    plateSpawnCount++;
                    OnPlateSpawned?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }

    public override void Interact(Player player) {
        if (!player.HasKitchenObject() && plateSpawnCount > 0) {
            plateSpawnCount--;
            KitchenObject.SpawnObject(kitchenObjectsSO, player);
            OnPlateRemoved?.Invoke(this, EventArgs.Empty);
        }
    }
}

