using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    private const string PLAYER_PREFS_SOUND_EFFECT_VOLUME = "SoundEffectVolume";

    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioClipListSO audioClipListSO;
    private int volume = 10;
    private int maxVolume = 10;

    public void PlayFootstepSound(Vector3 position, float volume = 1f) {
        PlaySound(audioClipListSO.footsteps, position, volume);
    }

    public void PlayCountdownSound() {
        PlaySound(audioClipListSO.warning, Vector3.zero);
    }

    public void PlayWarningSound(Vector3 position) {
        PlaySound(audioClipListSO.warning, position);
    }

    public void ChangeVolume() {
        volume += 1;
        if (volume > 10) {
            volume = 0;
        }
        PlayerPrefs.SetInt(PLAYER_PREFS_SOUND_EFFECT_VOLUME, volume);
    }

    public int GetVolume() {
        return volume;
    }

    private float GetVolumeNormalized() {
        return (float)volume / maxVolume;
    } 

    private void Awake() {
        if (Instance != null) {
            Debug.LogError("Attempting to create multiple instances of SoundManager");
            Destroy(this);
        } else {
            Instance = this;
        }

        volume = PlayerPrefs.GetInt(PLAYER_PREFS_SOUND_EFFECT_VOLUME, volume);
    }

    private void Start() {
        DeliveryManager.Instance.OnRecipeSuccess += OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed += OnRecipeFailed;
        CounterCutting.OnAnyCut += OnCutting;
        Player.Instance.OnPickup += Player_OnPickup;
        CounterBase.OnItemPlaced += Counter_OnItemPlaced;
        CounterTrash.OnItemTrashed += Counter_OnItemTrashed;
    }

    private void Counter_OnItemTrashed(object sender, EventArgs e) {
        PlaySound(audioClipListSO.trash, (sender as CounterTrash).transform.position);
    }

    private void Counter_OnItemPlaced(object sender, EventArgs e) {
        PlaySound(audioClipListSO.objectDrop, (sender as CounterBase).transform.position);
    }

    private void Player_OnPickup(object sender, EventArgs e) {
        PlaySound(audioClipListSO.objectPickup, Player.Instance.transform.position);
    }

    private void OnCutting(object sender, EventArgs e) {
        PlaySound(audioClipListSO.chop, (sender as CounterCutting).transform.position);
    }

    private void OnRecipeFailed(object sender, System.EventArgs e) {
        PlaySound(audioClipListSO.deliveryFail, DeliveryCounter.Instance.transform.position);
    }

    private void OnRecipeSuccess(object sender, System.EventArgs e) {
        PlaySound(audioClipListSO.deliverySuccess, DeliveryCounter.Instance.transform.position);
    }

    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volumeMultiplier = 1f) {
        PlaySound(audioClipArray[UnityEngine.Random.Range(0, audioClipArray.Length)], position, volumeMultiplier);
    }

    private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f) {
        AudioSource.PlayClipAtPoint(audioClip, position, volumeMultiplier * GetVolumeNormalized());
    }

    private void OnDestroy() {
        DeliveryManager.Instance.OnRecipeSuccess -= OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed -= OnRecipeFailed;
        CounterCutting.OnAnyCut -= OnCutting;
        Player.Instance.OnPickup -= Player_OnPickup;
        CounterBase.OnItemPlaced -= Counter_OnItemPlaced;
        CounterTrash.OnItemTrashed -= Counter_OnItemTrashed;
    }
}
