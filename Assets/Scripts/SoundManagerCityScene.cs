using System.Collections;
using UnityEngine;

public class SoundManagerCityScene : MonoBehaviour
{
    [SerializeField]
    private AudioSource singleRevolverShot;
    [SerializeField]
    private AudioSource multipleRevolverShot;

    private void Start()
    {
        StartCoroutine(PlaySounds());
    }

    private IEnumerator PlaySounds()
    {
        yield return new WaitForSeconds(5f);

        while (true)
        {
            var randomIndex = Random.Range(0, 3);
            var selectedClip = randomIndex < 2 ? singleRevolverShot : multipleRevolverShot;

            if (selectedClip == singleRevolverShot)
            {
                singleRevolverShot.Play();
                yield return new WaitForSeconds(singleRevolverShot.clip.length);

                singleRevolverShot.Play();
                yield return new WaitForSeconds(singleRevolverShot.clip.length);
            }
            else
            {
                selectedClip.Play();
                yield return new WaitForSeconds(selectedClip.clip.length);
            }

            var waitTime = Random.Range(5f, 6f);
            yield return new WaitForSeconds(waitTime);
        }
    }
}
