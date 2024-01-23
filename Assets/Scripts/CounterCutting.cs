using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class CounterCutting : CounterBase, IHasProgress {

    public static event EventHandler OnAnyCut;

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler OnCut;

    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;
    private int cuttingProgress;

    public override void Interact(Player player) {
        if (HasKitchenObject()) {
            if (player.HasKitchenObject()) {
                if (player.GetKitchenObject() is KitchenObjectPlate) {
                    // Put on plate
                    KitchenObjectPlate plate = player.GetKitchenObject() as KitchenObjectPlate;
                    if (plate.TryAddIngredient(GetKitchenObject().KitchenObjectSO)) {
                        GetKitchenObject().DestroySelf();
                    }
                }
            } else {
                GetKitchenObject().SetKitchenObjectParent(player);
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                    progressNormalized = 0
                });
            }
        } else {
            if (player.HasKitchenObject()) {
                KitchenObject playerKitchenObject = player.GetKitchenObject();
                if (IsRecipeInput(playerKitchenObject.KitchenObjectSO)) {
                    cuttingProgress = 0;
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                        progressNormalized = 0
                    });
                }
            }
        }
    }

    public override void InteractAlternate() {
        if (HasKitchenObject()) {
            CuttingRecipeSO cuttingSO = GetCuttingRecipeSOForInput(GetKitchenObject().KitchenObjectSO);
            if (cuttingSO != null) {
                cuttingProgress++;
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                    progressNormalized = (float)cuttingProgress / cuttingSO.cuttingProgressMax
                });
                OnCut?.Invoke(this, EventArgs.Empty);
                Debug.Log(OnAnyCut.GetInvocationList().Length);
                OnAnyCut?.Invoke(this, EventArgs.Empty);
                if (cuttingProgress >= cuttingSO.cuttingProgressMax) {
                    GetKitchenObject().DestroySelf();
                    KitchenObject.SpawnObject(cuttingSO.output, this);
                }
            }
        }
    }

    private bool IsRecipeInput(KitchenObjectsSO inputSO) {
        return GetCuttingRecipeSOForInput(inputSO) != null;
    }

    private KitchenObjectsSO GetOutputSO(KitchenObjectsSO inputSO) {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOForInput(inputSO);
        if (cuttingRecipeSO != null) {
            return cuttingRecipeSO.output;
        }
        return null;
    }

    private CuttingRecipeSO GetCuttingRecipeSOForInput(KitchenObjectsSO inputSO) {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray) {
            if (cuttingRecipeSO.input == inputSO) {
                return cuttingRecipeSO;
            }
        }
        return null;
    }

}
