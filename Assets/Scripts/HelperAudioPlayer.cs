using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperAudioPlayer : MonoBehaviour
{
    [SerializeField]
    private AudioClip helpMessageAudio;
    [SerializeField]
    private AudioClip storyMessageAudio;
    [SerializeField]
    private AudioClip goKillBanditsAudio;
    [SerializeField]
    private AudioClip goFindKFCAudio;
    [SerializeField]
    private AudioClip killedKFCMessage;

    private AudioSource audioSource;

    private bool finishedPlayingCurrentMessage = true;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        
    }

    public void PlayHelpMessage()
    {
        audioSource.clip = helpMessageAudio;
        finishedPlayingCurrentMessage = false;
        audioSource.Play();
        StartCoroutine(WaitForAudioToPlay(helpMessageAudio.length));
    }

    public void PlayStoryMessage()
    {
        audioSource.clip = storyMessageAudio;
        finishedPlayingCurrentMessage = false;
        audioSource.Play();
        StartCoroutine(WaitForAudioToPlay(storyMessageAudio.length));
    }

    public void PlayGoKillBanditsMessage()
    {
        audioSource.clip = goKillBanditsAudio;
        finishedPlayingCurrentMessage = false;
        audioSource.Play();
        StartCoroutine(WaitForAudioToPlay(goKillBanditsAudio.length));

    }

    public void PlayGoFindKFCAudio()
    {
        audioSource.clip = goFindKFCAudio;
        finishedPlayingCurrentMessage = false;
        audioSource.Play();
        StartCoroutine(WaitForAudioToPlay(goFindKFCAudio.length));
    }

    public void PlayKilledKFCAudio()
    {
        audioSource.clip = killedKFCMessage;
        finishedPlayingCurrentMessage = false;
        audioSource.Play();
        StartCoroutine(WaitForAudioToPlay(killedKFCMessage.length));
    }

    public bool FinishedPlayingCurrentMessage()
    {
        return finishedPlayingCurrentMessage;
    }

    private IEnumerator WaitForAudioToPlay(float time)
    {
        yield return new WaitForSeconds(time + 0.5f);
        finishedPlayingCurrentMessage = true;
    }
}
