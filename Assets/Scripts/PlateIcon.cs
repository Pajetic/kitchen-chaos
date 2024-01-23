using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlateIcon : MonoBehaviour {

    [SerializeField] private Image icon;

    public void SetIcon(KitchenObjectsSO kitchenObjectsSO) {
        icon.sprite = kitchenObjectsSO.sprite;
    }
}
