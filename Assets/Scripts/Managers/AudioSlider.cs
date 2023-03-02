using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Managers
{
    public class AudioSlider : MonoBehaviour
    {
        [SerializeField] private AudioMixer _mixer;
        [SerializeField] private Slider _slider;
        [SerializeField] private AudioMixParameter _volumeParameter;
        
        private readonly Dictionary<AudioMixParameter, string> _volumeParameters = new()
        {
            //{AudioMixParameter.MasterVolume, "MasterVolume"},
            {AudioMixParameter.MusicVolume, "MusicVolume"},
            {AudioMixParameter.SoundVolume, "SoundVolume"}
        };
        
        private string _currentVolumeParameterName;

        private void Awake()
        {
            _currentVolumeParameterName = _volumeParameters[_volumeParameter];
            _slider.onValueChanged.AddListener(OnChangeSlider);
        }

        private void Start()
        {
            _slider.value = PlayerPrefs.GetFloat(_currentVolumeParameterName, 0.01f);
        }

        private void OnChangeSlider(float value)
        {
            _mixer.SetFloat(_currentVolumeParameterName, Mathf.Log10(value) * 20);
        }

        private void OnDisable()
        {
            PlayerPrefs.SetFloat(_currentVolumeParameterName, _slider.value);
        }

        private enum AudioMixParameter
        {
            //MasterVolume //NOT USED
            MusicVolume,
            SoundVolume
        }
    }
}
