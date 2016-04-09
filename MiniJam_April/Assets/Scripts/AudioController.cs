using UnityEngine;
using System.Collections;

public class AudioController : MonoBehaviour
{
    //This is certainly not efficient :) yay 3 hour game jams!

    public static AudioController controller;

    public AudioClip wrongSound, rightSound, explodeSound;

    AudioSource audioSource;

    void Awake()
    {
        if (controller == null)
        {
            controller = this;
        }
        else if (controller != this)
        {
            Destroy(gameObject);
        }
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayWrongSound()
    {
        audioSource.PlayOneShot(wrongSound);
    }

    public void PlayRightSound()
    {
        audioSource.PlayOneShot(rightSound);
    }

    public void PlayExplodeSound()
    {
        audioSource.PlayOneShot(explodeSound);
    }
}
