using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioLoopPlayer : MonoBehaviour
{
    public AudioSource audioSource;
    public List<AudioClip> audioClips;

    private bool isAudioPlaying = false;
    private int audioIndex = 0;
    private Coroutine playAudioCorourine;

    public void PlayAudio(){
        playAudioCorourine = StartCoroutine(StartPlayAudio());
    }

    public void StopAudio(){
        if(playAudioCorourine != null){
            StopCoroutine(playAudioCorourine);
        }
    }

    private void Start() {
        PlayAudio();
    }

    IEnumerator StartPlayAudio() {
        PlayAudioClip(audioIndex = RandomAudioIndex());

        while(isAudioPlaying){
            float currentClipLength = CurrentClip().length;
            yield return new WaitForSeconds(currentClipLength);

            PlayNextClip();
        }
    }

    private int RandomAudioIndex(){
        return Random.Range(0, audioClips.Count);
    }

    private void PlayAudioClip(int index){
        isAudioPlaying = true;
        SetAudioClip(audioClips[audioIndex]);
        audioSource.Play();
    }

    private void PlayNextClip(){
        audioIndex += 1;
        if(audioIndex > audioClips.Count){
            audioIndex = 0;
        }
        PlayAudioClip(audioIndex);
    }

    private AudioClip CurrentClip(){
        return audioClips[audioIndex];
    }

    private void SetAudioClip(AudioClip clip){
        audioSource.clip = clip;
    }
}
