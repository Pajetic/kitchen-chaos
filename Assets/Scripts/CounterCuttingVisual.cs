using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterCuttingVisual : MonoBehaviour {

    private const string CUT = "Cut";

    [SerializeField] private CounterCutting counterCutting;
    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();    
    }

    private void Start() {
        counterCutting.OnCut += AnimateKnife;
    }

    private void AnimateKnife(object sender, EventArgs e) {
        animator.SetTrigger(CUT);
    }
}

