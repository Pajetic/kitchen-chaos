using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterTrash : CounterBase {

    public static event EventHandler OnItemTrashed;

    public override void Interact(Player player) {
        if (player.HasKitchenObject()) {
            KitchenObject kitchenObject = player.GetKitchenObject();
            kitchenObject.DestroySelf();
            OnItemTrashed?.Invoke(this, EventArgs.Empty);
        }
    }
}

