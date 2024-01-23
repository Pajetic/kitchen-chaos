using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {

    private const string PLAYER_PREFS_MUSIC_VOLUME = "MusicVolume";

    public static MusicManager Instance { get; private set; }

    private AudioSource audioSource;
    private int volume = 2;
    private int maxVolume = 10;

    public void ChangeVolume() {
        volume += 1;
        if (volume > maxVolume) {
            volume = 0;
        }
        audioSource.volume = GetVolumeNormalized();
        PlayerPrefs.SetInt(PLAYER_PREFS_MUSIC_VOLUME, volume);
    }

    public float GetVolume() {
        return volume;
    }

    private void Awake() {
        if (Instance != null) {
            Debug.LogError("Attempting to create multiple instances of MusicManager.");
            Destroy(this);
        } else {
            Instance = this;
        }

        audioSource = GetComponent<AudioSource>();
        volume = PlayerPrefs.GetInt(PLAYER_PREFS_MUSIC_VOLUME, volume);
        audioSource.volume = GetVolumeNormalized();
    }

    private void Start() {
        audioSource.volume = GetVolumeNormalized();
    }

    private float GetVolumeNormalized() {
        return (float)volume / maxVolume;
    }
}
