using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateIconsUI : MonoBehaviour {

    [SerializeField] private KitchenObjectPlate kitchenObjectPlate;
    [SerializeField] private Transform iconTemplate;

    private void Awake() {
        iconTemplate.gameObject.SetActive(false);
    }

    private void Start() {
        kitchenObjectPlate.OnIngredientAdded += OnIngredientAdded;
    }

    private void OnIngredientAdded(object sender, KitchenObjectPlate.OnIngredientAddedEventArgs e) {
        foreach (Transform child in transform) {
            if (child != iconTemplate) {
                Destroy(child.gameObject);
            }
        }
        foreach (KitchenObjectsSO kitchenObjectsSO in kitchenObjectPlate.GetIngredientsOnPlate()) {
            Transform iconTransform = Instantiate(iconTemplate, transform);
            iconTransform.GetComponent<PlateIcon>().SetIcon(kitchenObjectsSO);
            iconTransform.gameObject.SetActive(true);
        }
    }


}

