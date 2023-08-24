using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; }
    private AudioSource Soundsource;
    private AudioSource Musicsource;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        Soundsource = GetComponent<AudioSource>();
        Musicsource = transform.GetChild(0).GetComponent<AudioSource>();

        // Keep this object even when we go to new scene
        if (instance != this )
        {
            DontDestroyOnLoad(gameObject);
        }
        // Destroy duplicate gameobjects
        else 
        {
            Destroy(gameObject);
        }

        // Assign initial volumes
        ChangeSoundVolume(0);
        ChangeMusicVolume(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Playsound(AudioClip _sound)
    {
        Soundsource.PlayOneShot(_sound);
    }

    public void ChangeSoundVolume(float _change)
    {
        ChangeSourceVolume(1, "soundVolume", _change, Soundsource);
    }

    public void ChangeMusicVolume(float _change)
    {
        ChangeSourceVolume(0.3f, "musicVolume", _change, Musicsource);
    }

    private void ChangeSourceVolume(float baseVolume, string volumeName, float change, AudioSource source)
    {
        // Get initial value of volume and change it
        float currentVolume = PlayerPrefs.GetFloat(volumeName, 1);
        currentVolume += change;

        // Check if we reached the maximum or minimum value
        if (currentVolume > 1)
            currentVolume = 0;
        else if (currentVolume < 0)
            currentVolume = 1;

        // Assign final value
        float finalVolume = currentVolume * baseVolume;
        source.volume = finalVolume;

        // Save final value to player prefs
        PlayerPrefs.SetFloat(volumeName, currentVolume);
    }
}


