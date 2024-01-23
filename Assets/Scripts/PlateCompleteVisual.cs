using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour {

    [SerializeField] KitchenObjectPlate kitchenObjectPlate;
    [SerializeField] List<KitchenObjectSOToGameObjectBinding> ingredientGameObjectBindingList;

    private void Start() {
        kitchenObjectPlate.OnIngredientAdded += OnIngredientAdded;

        foreach (KitchenObjectSOToGameObjectBinding ingredientBinding in ingredientGameObjectBindingList) {
            ingredientBinding.gameObject.SetActive(false);
        }
    }

    private void OnIngredientAdded(object sender, KitchenObjectPlate.OnIngredientAddedEventArgs e) {
        foreach (KitchenObjectSOToGameObjectBinding ingredientBinding in ingredientGameObjectBindingList) {
            if (ingredientBinding.kitchenObjectSO == e.ingredient) {
                ingredientBinding.gameObject.SetActive(true);
            }
        }
    }

    [Serializable]
    public struct KitchenObjectSOToGameObjectBinding {
        public KitchenObjectsSO kitchenObjectSO;
        public GameObject gameObject;
    }
}
