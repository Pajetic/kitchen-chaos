using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CounterCutting;

public class CounterStove : CounterBase, IHasProgress {

    public event EventHandler<OnCookingStateChangedEventArgs> OnCookingStateChanged;
    public class OnCookingStateChangedEventArgs : EventArgs {
        public bool isCooking;
    }
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    private enum State {
        Idle,
        Frying,
        Fried,
        Burned
    }

    [SerializeField] private StoveRecipeSO[] stoveRecipeSOArray;
    [SerializeField] private StoveBurningRecipeSO[] stoveBurningRecipeSOArray;
    private State state;
    private float cookingTimer;
    private float burningTimer;
    private StoveRecipeSO currentRecipeSO;
    private StoveBurningRecipeSO currentBurningRecipeSO;
    private float burnWarningThreshold = 0.5f;

    public override void Interact(Player player) {
        if (HasKitchenObject()) {
            if (player.HasKitchenObject()) {
                if (player.GetKitchenObject() is KitchenObjectPlate) {
                    // Put on plate
                    KitchenObjectPlate plate = player.GetKitchenObject() as KitchenObjectPlate;
                    if (plate.TryAddIngredient(GetKitchenObject().KitchenObjectSO)) {
                        GetKitchenObject().DestroySelf();
                        OnCookingStateChanged?.Invoke(this, new OnCookingStateChangedEventArgs {
                            isCooking = false
                        });
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                            progressNormalized = 0f
                        });
                    }
                }
            } else {
                GetKitchenObject().SetKitchenObjectParent(player);
                state = State.Idle;
                OnCookingStateChanged?.Invoke(this, new OnCookingStateChangedEventArgs {
                    isCooking = false
                });
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                    progressNormalized = 0f
                });
            }
        } else {
            if (player.HasKitchenObject()) {
                KitchenObject playerKitchenObject = player.GetKitchenObject();
                if (IsRecipeInput(playerKitchenObject.KitchenObjectSO)) {
                    cookingTimer = 0f;
                    playerKitchenObject.SetKitchenObjectParent(this);
                    currentRecipeSO = GetStoveRecipeSOForInput(GetKitchenObject().KitchenObjectSO);
                    state = State.Frying;
                    OnCookingStateChanged?.Invoke(this, new OnCookingStateChangedEventArgs {
                        isCooking = true
                    });
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                        progressNormalized = cookingTimer / currentRecipeSO.cookingTimerMax
                    });
                }
            }
        }
    }

    public bool IsFried() {
        return state == State.Fried;
    }

    public float GetBurnWarningThreshold() {
        return burnWarningThreshold;
    }

    private void Start() {
        state = State.Idle;
    }

    private void Update() {
        switch(state) {
            case State.Idle:
                break;
            case State.Frying:
                break;
            case State.Fried:
                break;
            case State.Burned:
                break;
        }


        if (HasKitchenObject() && currentRecipeSO != null) {
            switch (state) {
                case State.Idle:
                    break;
                case State.Frying:
                    cookingTimer += Time.deltaTime;
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                        progressNormalized = cookingTimer / currentRecipeSO.cookingTimerMax
                    });
                    if (cookingTimer >= currentRecipeSO.cookingTimerMax) {
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnObject(currentRecipeSO.output, this);
                        currentBurningRecipeSO = GetStoveBurningRecipeSOForInput(GetKitchenObject().KitchenObjectSO);
                        state = State.Fried;
                        burningTimer = 0f;
                        OnCookingStateChanged?.Invoke(this, new OnCookingStateChangedEventArgs {
                            isCooking = true
                        });
                    }
                    
                    break;
                case State.Fried:
                    burningTimer += Time.deltaTime;
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                        progressNormalized = burningTimer / currentBurningRecipeSO.burningTimerMax
                    });
                    if (burningTimer >= currentBurningRecipeSO.burningTimerMax) {
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnObject(currentBurningRecipeSO.output, this);
                        state = State.Burned;
                        OnCookingStateChanged?.Invoke(this, new OnCookingStateChangedEventArgs {
                            isCooking = false
                        });
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                            progressNormalized = 0f
                        });
                    }
                    
                    break;
                case State.Burned:
                    break;
            }
        }
    }

    private bool IsRecipeInput(KitchenObjectsSO inputSO) {
        return GetStoveRecipeSOForInput(inputSO) != null;
    }

    private KitchenObjectsSO GetOutputSO(KitchenObjectsSO inputSO) {
        StoveRecipeSO stoveRecipeSO = GetStoveRecipeSOForInput(inputSO);
        if (stoveRecipeSO != null) {
            return stoveRecipeSO.output;
        }
        return null;
    }

    private StoveRecipeSO GetStoveRecipeSOForInput(KitchenObjectsSO inputSO) {
        foreach (StoveRecipeSO stoveRecipeSO in stoveRecipeSOArray) {
            if (stoveRecipeSO.input == inputSO) {
                return stoveRecipeSO;
            }
        }
        return null;
    }

    private StoveBurningRecipeSO GetStoveBurningRecipeSOForInput(KitchenObjectsSO inputSO) {
        foreach (StoveBurningRecipeSO stoveBurningRecipeSO in stoveBurningRecipeSOArray) {
            if (stoveBurningRecipeSO.input == inputSO) {
                return stoveBurningRecipeSO;
            }
        }
        return null;
    }
}

