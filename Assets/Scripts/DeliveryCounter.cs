using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : CounterBase {

    public static DeliveryCounter Instance {  get; private set; }

    public override void Interact(Player player) {
        if (player.HasKitchenObject() && player.GetKitchenObject() is KitchenObjectPlate) {
            KitchenObjectPlate foodDelivered = player.GetKitchenObject() as KitchenObjectPlate;
            DeliveryManager.Instance.DeliverFood(foodDelivered);
            foodDelivered.DestroySelf();
        }
    }

    private void Awake() {
        if (Instance != null) {
            Debug.LogError("Attempting to create more than one instance of DeliveryCounter.");
            Destroy(this);
        } else {
            Instance = this;
        }
    }

}
