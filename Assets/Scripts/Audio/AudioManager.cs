using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;

public class AudioManager : MonoBehaviour
{
    [Header("事件监听")]
    public PlayAudioEventSO SFXEvent;
    public PlayAudioEventSO BGMEvent;
    public FloatEventSO masterVolumeChange;
    public FloatEventSO BGMVolumeChange;
    public FloatEventSO SFXVolumeChange;


    [Header("获取组件")]
    public AudioSource BGM;
    public AudioSource SFX;
    public AudioMixer mixer;
    public Slider MasterVolumeSlider;
    public Slider BGMVolumeSlider;
    public Slider SFXVolumeSlider;

    private string masterVolume;
    private string bgmVolume;
    private string sfxVolume;

    private void OnEnable() {
        SFXEvent.OnEventRaised += OnSFXEvent;
        BGMEvent.OnEventRaised += OnBGMEvent;
        masterVolumeChange.OnEventRaised += OnMasterVolumeChange;
        BGMVolumeChange.OnEventRaised += OnBGMVolumeChange;
        SFXVolumeChange.OnEventRaised += OnSFXVolumeChange;
    }

    private void OnDisable() {
        SFXEvent.OnEventRaised -= OnSFXEvent;
        BGMEvent.OnEventRaised -= OnBGMEvent;
        masterVolumeChange.OnEventRaised -= OnMasterVolumeChange;
        BGMVolumeChange.OnEventRaised -= OnBGMVolumeChange;
        SFXVolumeChange.OnEventRaised -= OnSFXVolumeChange;
    }

    private void OnBGMVolumeChange(float arg0)
    {
        mixer.SetFloat("BGMVolume",arg0 * 100 - 80);
    }

    private void OnSFXVolumeChange(float arg0)
    {
        mixer.SetFloat("SFXVolume",arg0 * 100 - 80);
    }

    private void OnMasterVolumeChange(float arg0)
    {
        mixer.SetFloat("MasterVolume",arg0 * 100 - 80);
    }
    public void SetPausePanelSlider()
    {
        mixer.GetFloat("MasterVolume", out float masterVolume);
        mixer.GetFloat("BGMVolume", out float bgmVolume);
        mixer.GetFloat("SFXVolume", out float sfxVolume);
        MasterVolumeSlider.value = (masterVolume + 80) / 100;
        BGMVolumeSlider.value = (bgmVolume + 80) / 100;
        SFXVolumeSlider.value = (sfxVolume + 80) / 100;
    }

    private void OnBGMEvent(AudioClip clip)
    {
        BGM.clip = clip;
        BGM.Play();
    }

    private void OnSFXEvent(AudioClip clip)
    {
        SFX.clip = clip;
        SFX.Play();
    }
}
