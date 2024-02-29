using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class SoundsController : MonoBehaviour
{
    public AudioSource[] Audios;
    public AudioMixer mixer;
    public Slider musicSlider;
    const string MIXER_MUSIC = "Volume";
    public float valueTest;


    private void Start()
    {
        musicSlider.value = PlayerPrefs.GetFloat(MIXER_MUSIC);
    }
    public void Awake()
    {

        musicSlider.onValueChanged.AddListener(SetMusicValume);
    }

    private void SetMusicValume(float value)
    {
        
        mixer.SetFloat(MIXER_MUSIC, Mathf.Log10(value) * 20);


    }
 
    private void OnDisable()
    {
        PlayerPrefs.SetFloat(MIXER_MUSIC, musicSlider.value);
    }

}
