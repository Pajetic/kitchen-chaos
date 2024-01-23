using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour {

    [SerializeField] private Transform deliveryListContainer;
    [SerializeField] private Transform deliveryItemTemplate;


    private void Awake() {
        deliveryItemTemplate.gameObject.SetActive(false);
    }

    private void Start() {
        DeliveryManager.Instance.OnPendingRecipesChanged += OnItemsChanged;
        UpdateVisual();
    }

    private void OnItemsChanged(object sender, EventArgs e) {
        UpdateVisual();
    }
    
    private void UpdateVisual() {
        foreach (Transform child in deliveryListContainer) {
            if (child != deliveryItemTemplate) {
                Destroy(child.gameObject);
            }
        }

        foreach (RecipeSO recipeSO in DeliveryManager.Instance.PendingRecipes) {
            Transform deliveryItem = Instantiate(deliveryItemTemplate, deliveryListContainer);
            deliveryItem.GetComponent<DeliveryManagerSingleItem>().SetRecipe(recipeSO);
            deliveryItem.gameObject.SetActive(true);
        }
    }
}
