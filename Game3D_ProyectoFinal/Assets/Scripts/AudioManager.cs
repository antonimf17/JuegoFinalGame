using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //Desclaración de Singleton
    public static AudioManager Instance;

    [Header("Audio Source References")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;


    [Header("Audio Clip Arrays")]
    public AudioClip[] musiclist;
    public AudioClip[] sfxListt;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }    
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic(int musicIndex)
    {
        musicSource.clip = musiclist[musicIndex];
        musicSource.Play();
    }

    public void PlaySFX(int sfxIndex)
    {
        sfxSource.PlayOneShot(sfxListt[sfxIndex]);
    }
}
