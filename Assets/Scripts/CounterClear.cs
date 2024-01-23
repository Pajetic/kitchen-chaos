using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterClear : CounterBase {

    public override void Interact(Player player) {
        if (HasKitchenObject()) {
            if (player.HasKitchenObject()) {
                if (player.GetKitchenObject() is KitchenObjectPlate) {
                    // Put on plate
                    KitchenObjectPlate plate = player.GetKitchenObject() as KitchenObjectPlate;
                    if (plate.TryAddIngredient(GetKitchenObject().KitchenObjectSO)) {
                        GetKitchenObject().DestroySelf();
                    }
                } else if (GetKitchenObject() is KitchenObjectPlate) {
                    // Put on plate on counter
                    KitchenObjectPlate plate = GetKitchenObject() as KitchenObjectPlate;
                    if (plate.TryAddIngredient(player.GetKitchenObject().KitchenObjectSO)) {
                        player.GetKitchenObject().DestroySelf();
                    }
                }
            } else {
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        } else {
            if (player.HasKitchenObject()) {
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
        }
    }
}
