using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance { get { return instance; } }

    public AudioSource musicAudioSource;
    public AudioSource placeDrinkAudioSource;
    public AudioSource gulpDrinkAudioSource;
    public AudioSource countdownAudioSource;
    public AudioSource bellDingAudioSource;
    public AudioSource dishesAudioSource;
    public AudioSource smashPlateAudioSource;
    public AudioSource clappingAudioSource;

    private void Awake()
    {
        instance = this;
    }


}
