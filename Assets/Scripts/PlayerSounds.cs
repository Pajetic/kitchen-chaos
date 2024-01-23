using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour {

    [SerializeField] private Player player;
    private float footstepTimer = 0f;
    private float footstepTimerMax = 0.1f;


    private void Awake() {
        player = GetComponent<Player>();
    }

    private void Update() {
        footstepTimer += Time.deltaTime;
        if (footstepTimer >= footstepTimerMax ) {
            footstepTimer = 0f;
            if (player.IsWalking) {
                SoundManager.Instance.PlayFootstepSound(player.transform.position);
            }
        }
    }

}
