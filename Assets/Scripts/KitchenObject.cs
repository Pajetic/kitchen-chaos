using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour {

    public KitchenObjectsSO KitchenObjectSO => kitchenObjectSO;

    [SerializeField] private KitchenObjectsSO kitchenObjectSO;
    private IKitchenObjectParent kitchenObjectParent;


    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent) {
        if (kitchenObjectParent.HasKitchenObject() == true) {
            Debug.LogError("Target already has a KitchenObject.");
            return;
        }
        
        if (this.kitchenObjectParent != null) {
            this.kitchenObjectParent.ClearKitchenObject();
        }
        this.kitchenObjectParent = kitchenObjectParent;
        this.kitchenObjectParent.SetKitchenObject(this);
        transform.parent = this.kitchenObjectParent.GetKitchenObjectTargetTransform();
        transform.localPosition = Vector3.zero;
    }

    public void DestroySelf() {
        kitchenObjectParent.ClearKitchenObject();
        Destroy(gameObject);
    }

    public static void SpawnObject(KitchenObjectsSO kitchenObjectsSO, IKitchenObjectParent parent) {
        Transform kitchenObjectTransform = Instantiate(kitchenObjectsSO.prefab);
        kitchenObjectTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(parent);
    }

}