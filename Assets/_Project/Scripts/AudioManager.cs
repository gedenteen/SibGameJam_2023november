using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioSource audioSourceMusic;
    [SerializeField] private AudioSource audioSourceSounds;
    [SerializeField] private AudioClip clipMainMenu;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void StopMusic()
    {
        audioSourceMusic.Stop();
    }

    public void PlaySound(AudioClip audioClip)
    {
        audioSourceSounds.clip = audioClip;
        audioSourceSounds.Play();
    }
}
