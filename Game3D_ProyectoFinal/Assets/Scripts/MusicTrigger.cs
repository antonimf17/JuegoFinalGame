using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTrigger : MonoBehaviour
{
    [SerializeField] int musicToPlay;

    private void Start()
    {
        AudioManager.Instance.PlayMusic(musicToPlay);
    }
}
