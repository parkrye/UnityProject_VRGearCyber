using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    AudioMixer audioMixer;

    void Awake()
    {
        audioMixer = GameManager.Resource.Load<AudioMixer>("Audio/AudioMixer");
    }

    public float MasterVolume
    {
        get
        {
            float volume;
            audioMixer.GetFloat("Master", out volume);
            return volume;
        }
        set
        {
            if (value == 0f)
            {
                audioMixer.SetFloat("Master", -80f);
            }
            else
            {
                audioMixer.SetFloat("Master", -40f + value * 40f);
            }
        }
    }
    public float BGMVolume
    {
        get
        {
            float volume;
            audioMixer.GetFloat("BGM", out volume);
            return volume;
        }
        set
        {
            if (value == 0f)
            {
                audioMixer.SetFloat("BGM", -80f);
            }
            else
            {
                audioMixer.SetFloat("BGM", -40f + value * 40f);
            }
        }
    }
    public float SFXVolume
    {
        get
        {
            float volume;
            audioMixer.GetFloat("SFX", out volume);
            return volume;
        }
        set
        {
            if (value == 0f)
            {
                audioMixer.SetFloat("SFX", -80f);
            }
            else
            {
                audioMixer.SetFloat("SFX", -40f + value * 40f);
            }
        }
    }
}