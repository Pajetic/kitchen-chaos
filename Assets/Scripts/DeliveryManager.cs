using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour {

    public event EventHandler OnPendingRecipesChanged;
    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFailed;

    public static DeliveryManager Instance { get; private set; }

    public List<RecipeSO> PendingRecipes => pendingRecipes;

    [SerializeField] private RecipeListSO recipeListSO;
    private List<RecipeSO> pendingRecipes;
    [SerializeField] private int maxPendingRecipes = 4;
    [SerializeField] private float recipeTimerMax = 4f;
    private WaitForSeconds recipeTimerMaxWait;
    private int successfulRecipeCount = 0;
    private IEnumerator deliveryGenerator;

    public void DeliverFood(KitchenObjectPlate kitchenObjectPlate) {
        // Check each recipe
        foreach (RecipeSO recipeSO in recipeListSO.recipeList) {
            // Check ingredient count
            if (recipeSO.ingredients.Count != kitchenObjectPlate.GetIngredientsOnPlate().Count) {
                continue;
            }
            // Check each ingredient
            bool incorrectIngredient = false;
            foreach (KitchenObjectsSO recipeIngredient in recipeSO.ingredients) {
                List<KitchenObjectsSO> ingredientsOnPlate = kitchenObjectPlate.GetIngredientsOnPlate();
                if (!ingredientsOnPlate.Contains(recipeIngredient)) {
                    incorrectIngredient = true;
                    break;
                }
            }

            if (!incorrectIngredient) {
                // Correct recipt
                //Debug.Log("Correct recipe delivered.");
                successfulRecipeCount++;
                pendingRecipes.Remove(recipeSO);
                OnPendingRecipesChanged?.Invoke(this, EventArgs.Empty);
                OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
                return;
            }
        }

        //Debug.Log("Incorrect recipe delivered.");
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
    }

    public int GetSuccessfulRecipeCount() {
        return successfulRecipeCount;
    }

    private void Awake() {
        if (Instance != null) {
            Debug.LogError("Trying to create more than one instance of DeliveryManager.");
            Destroy(gameObject);
        } else {
            Instance = this;
        }
        recipeTimerMaxWait = new WaitForSeconds(recipeTimerMax);
        pendingRecipes = new List<RecipeSO>();
    }

    private void Start() {
        KitchenGameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
    }

    private void GameManager_OnStateChanged(object sender, EventArgs e) {
        if (KitchenGameManager.Instance.IsGamePlaying()) {
            StartDeliveryGeneration();
        } else {
            StopDeliveryGeneration();
        }
    }

    private void StartDeliveryGeneration() {
        deliveryGenerator = GeneratePendingRecipe();
        StartCoroutine(deliveryGenerator);
    }

    private void StopDeliveryGeneration() {
        if (deliveryGenerator != null) {
            StopCoroutine(deliveryGenerator);
        }
    }

    private IEnumerator GeneratePendingRecipe() {
        while (true) {
            if (pendingRecipes.Count < maxPendingRecipes) {
                RecipeSO nextRecipe = recipeListSO.recipeList[UnityEngine.Random.Range(0, recipeListSO.recipeList.Count)];
                pendingRecipes.Add(nextRecipe);
                OnPendingRecipesChanged?.Invoke(this, EventArgs.Empty);
            }
            yield return recipeTimerMaxWait;
        }
    }
}
