using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKitchenObjectParent {

    public void SetKitchenObject(KitchenObject kitchenObject);

    public KitchenObject GetKitchenObject();

    public Transform GetKitchenObjectTargetTransform();

    public void ClearKitchenObject();

    public bool HasKitchenObject();
}
