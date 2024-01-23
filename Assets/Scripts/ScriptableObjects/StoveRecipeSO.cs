using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class StoveRecipeSO : ScriptableObject {
    public KitchenObjectsSO input;
    public KitchenObjectsSO output;
    public float cookingTimerMax;
}
