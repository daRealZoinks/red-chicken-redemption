using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerBar : MonoBehaviour
{
    [SerializeField]
    private AudioClip iWillDrinkFromYourSkull;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.clip = iWillDrinkFromYourSkull;
        audioSource.Play();
    }
}
