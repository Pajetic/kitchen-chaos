using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent {

    public static Player Instance { get; private set; }

    public bool IsWalking => isWalking;
    public event EventHandler OnPickup;

    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private Transform kitchenObjectHoldLocation;

    private bool isWalking = false;
    private float playerHeight = 2f;
    private float playerRadius = 0.7f;
    private float playerInteractDistance = 2f;
    private CounterBase selectedCounter;
    private KitchenObject kitchenObject;

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs {
        public CounterBase selectedCounter;
    }


    private void Awake() {
        if (Instance != null) {
            Debug.LogError("Trying to create more than one instance of Player.");
            Destroy(gameObject);
        } else {
            Instance = this;
        }
    }

    private void Start() {
        gameInput.OnInteractAction += OnInteract;
        gameInput.OnInteractAlternateAction += OnInteractAlternate;
    }

    private void Update() {
        HandleMovement();
        HandleInteraction();
    }

    private void HandleMovement() {
        Vector3 moveVector = gameInput.GetMovementVectorNormalized();
        Vector3 finalVector = new Vector3(moveVector.x, moveVector.y, moveVector.z);
        float moveDistance = Time.deltaTime * moveSpeed;

        // If collide
        if (Physics.CapsuleCast(transform.position, transform.position + new Vector3(0f, playerHeight, 0f), playerRadius, moveVector, moveDistance)) {
            Vector3 partialMoveVector = new Vector3(0f, 0f, moveVector.z);
            if (Physics.CapsuleCast(transform.position, transform.position + new Vector3(0f, playerHeight, 0f), playerRadius, partialMoveVector, moveDistance * partialMoveVector.magnitude)) {
                finalVector.z = 0f;
            }
            partialMoveVector = new Vector3(moveVector.x, 0f, 0f);
            if (Physics.CapsuleCast(transform.position, transform.position + new Vector3(0f, playerHeight, 0f), playerRadius, partialMoveVector, moveDistance * partialMoveVector.magnitude)) {
                finalVector.x = 0f;
            }
        }

        isWalking = moveVector.magnitude > 0;
        if (finalVector.magnitude < 0.5f) {
            finalVector = Vector3.zero;
        }
        transform.position += finalVector * moveDistance;
        transform.forward = Vector3.Slerp(transform.forward, moveVector, Time.deltaTime * rotationSpeed);
    }

    private void HandleInteraction() {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit raycastHit, playerInteractDistance, countersLayerMask)) {
            if (raycastHit.transform.TryGetComponent<CounterBase>(out CounterBase counter)) {
                if (selectedCounter != counter) {
                    SetSelectedCounter(counter);
                }
            } else {
                if (selectedCounter != null) {
                    SetSelectedCounter(null);
                }
            }
        } else {
            if (selectedCounter != null) {
                SetSelectedCounter(null);
            }
        }
    }

    private void OnInteract(object sender, EventArgs e) {
        if (!KitchenGameManager.Instance.IsGamePlaying()) return;
        if (selectedCounter != null) {
            selectedCounter.Interact(this);
        }
    } 

    private void OnInteractAlternate(object sender, EventArgs e) {
        if (!KitchenGameManager.Instance.IsGamePlaying()) return;
        if (selectedCounter != null) {
            selectedCounter.InteractAlternate();
        }
    }

    private void SetSelectedCounter(CounterBase selectedCounter) {
        this.selectedCounter = selectedCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs {
            selectedCounter = this.selectedCounter
        });
    }

    public void SetKitchenObject(KitchenObject kitchenObject) {
        this.kitchenObject = kitchenObject;
        if (this.kitchenObject != null) {
            OnPickup?.Invoke(this, EventArgs.Empty);
        }
    }

    public KitchenObject GetKitchenObject() {
        return kitchenObject;
    }

    public Transform GetKitchenObjectTargetTransform() {
        return kitchenObjectHoldLocation;
    }

    public void ClearKitchenObject() {
        kitchenObject = null;
    }

    public bool HasKitchenObject() {
        return kitchenObject != null;
    }
}

