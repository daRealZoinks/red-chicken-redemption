using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManagerCityAttack : MonoBehaviour
{
    private AudioSource audioSource;

    private bool playingMusic = false;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (FindObjectsOfType<KFCScript>().Length == 0 && SceneManager.GetActiveScene().name == "CityAttack" && !playingMusic)
        {
            playingMusic = true;
            audioSource.Play();
        }
    }
}
