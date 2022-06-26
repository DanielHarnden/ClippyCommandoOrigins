using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public int track;
    private int tempInt = -100;
    // 0 = Main Menu / World Map / Shop
    // 1 = Normal Level
    // 2 = Intense Level

    public AudioClip zero;
    public AudioClip one;
    public AudioClip two;

    private AudioSource thisAudio;

    void Awake()
    {
        thisAudio = this.gameObject.GetComponent<AudioSource>();
        DontDestroyOnLoad(this.gameObject);
    }

    void Update() 
    {
        if (track != tempInt)
        {
            tempInt = track;
            switch(track)
            {
                case 0:
                    thisAudio.clip = zero;
                    break;

                case 1:
                    thisAudio.clip = one;
                    break;

                case 2:
                    thisAudio.clip = two;
                    break;

                default:
                    break;
            }
            thisAudio.Play();
        }
    }
}
