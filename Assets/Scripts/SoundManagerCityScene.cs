using System.Collections;
using UnityEngine;

public class SoundManagerCityScene : MonoBehaviour
{
    [SerializeField]
    private AudioClip singleRevolverShot;
    [SerializeField]
    private AudioClip multipleRevolverShot;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        StartCoroutine(PlaySounds());
    }

    IEnumerator PlaySounds()
    {
        yield return new WaitForSeconds(5f);

        while (true)
        {
            int randomIndex = Random.Range(0, 3);
            AudioClip selectedClip = randomIndex < 2 ? singleRevolverShot : multipleRevolverShot;

            if (selectedClip == singleRevolverShot)
            {
                audioSource.clip = singleRevolverShot;
                audioSource.Play();
                yield return new WaitForSeconds(audioSource.clip.length);

                audioSource.clip = singleRevolverShot;
                audioSource.Play();
                yield return new WaitForSeconds(audioSource.clip.length);
            }
            else
            {
                audioSource.clip = selectedClip;
                audioSource.Play();
                yield return new WaitForSeconds(audioSource.clip.length);
            }

            float waitTime = Random.Range(5f, 6f);
            yield return new WaitForSeconds(waitTime);
        }
    }
}
