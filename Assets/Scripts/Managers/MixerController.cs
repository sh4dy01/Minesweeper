using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MixerController : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private Slider _soundSlider;
    [SerializeField] private Slider _musicSlider;

    private void Awake()
    {
        _soundSlider.value = _audioMixer.GetFloat("SoundVolume", out var soundVolume) ? soundVolume : 50;
        _soundSlider.value = _audioMixer.GetFloat("MusicVolume", out var musicVolume) ? musicVolume : 50;
    }

    public void SetSoundVolume(float sliderValue)
    {
        _audioMixer.SetFloat("SoundVolume", Mathf.Log10(sliderValue) * 20);
    }
    
    public void SetMusicVolume(float sliderValue)
    {
        _audioMixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
    }
}
