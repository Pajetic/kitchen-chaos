using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class CounterBase : MonoBehaviour, IKitchenObjectParent {

    public static event EventHandler OnItemPlaced;

    [SerializeField] private Transform counterTop;
    private KitchenObject kitchenObject;

    public virtual void Interact(Player player) { }

    public virtual void InteractAlternate() { }

    public Transform GetKitchenObjectTargetTransform() {
        return counterTop;
    }

    public void SetKitchenObject(KitchenObject kitchenObject) {
        this.kitchenObject = kitchenObject;
        if (this.kitchenObject != null ) {
            OnItemPlaced?.Invoke(this, EventArgs.Empty);
        }
    }

    public KitchenObject GetKitchenObject() {
        return kitchenObject;
    }

    public void ClearKitchenObject() {
        kitchenObject = null;
    }

    public bool HasKitchenObject() {
        return kitchenObject != null;
    }
}

