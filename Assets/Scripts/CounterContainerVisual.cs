using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterContainerVisual : MonoBehaviour {

    private const string OPEN_CLOSE = "OpenClose";

    [SerializeField] private CounterContainer counterContainer;
    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();    
    }

    private void Start() {
        counterContainer.OnPlayerGrabbedObject += AnimateLid;
    }

    private void AnimateLid(object sender, EventArgs e) {
        animator.SetTrigger(OPEN_CLOSE);
    }
}

