using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryManagerSingleItem : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI recipeNameText;
    [SerializeField] private Transform iconContainer;
    [SerializeField] private Transform iconTemplate;

    public void SetRecipe(RecipeSO recipe) {
        recipeNameText.SetText(recipe.recipeName);

        foreach (Transform child in iconContainer) {
            if (child != iconTemplate) {
                Destroy(child.gameObject);
            }
        }

        foreach (KitchenObjectsSO ingredient in recipe.ingredients) {
            Transform ingredientIcon = Instantiate(iconTemplate, iconContainer);
            ingredientIcon.GetComponent<Image>().sprite = ingredient.sprite;
            ingredientIcon.gameObject.SetActive(true);
        }
    }

    private void Awake() {
        iconTemplate.gameObject.SetActive(false);
    }

}
