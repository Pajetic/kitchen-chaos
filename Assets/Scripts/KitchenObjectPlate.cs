using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObjectPlate : KitchenObject {

    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs : EventArgs {
        public KitchenObjectsSO ingredient;
    }

    [SerializeField] private List<KitchenObjectsSO> validIngredients;
    private List<KitchenObjectsSO> plateContents = new List<KitchenObjectsSO>();

    public bool TryAddIngredient(KitchenObjectsSO kitchenObjectSO) {
        if (plateContents.Contains(kitchenObjectSO)) {
            //Debug.Log(kitchenObjectSO.name + " already on plate");
            return false;
        }
        if (!validIngredients.Contains(kitchenObjectSO)) {
            //Debug.Log(kitchenObjectSO.name + " is not valid ingredient");
            return false;
        }

        //Debug.Log("Added " + kitchenObjectSO.name);
        plateContents.Add(kitchenObjectSO);
        OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs {
            ingredient = kitchenObjectSO
        });
        return true;
    }

    public List<KitchenObjectsSO> GetIngredientsOnPlate() {
        return plateContents;
    }
}

