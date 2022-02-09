using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager: MonoBehaviour
{
    [SerializeField]
    private SoundAudioClip[] soundAudioClipArray;
    private void PlaySound(Sound sound)
    {
        GameObject soundGO = new GameObject("Sound");
        AudioSource audioSource = soundGO.AddComponent<AudioSource>();
        audioSource.PlayOneShot(GetAudioClip(sound));
    }

    private AudioClip GetAudioClip(Sound sound)
    {
        foreach(SoundAudioClip soundAudioClip in soundAudioClipArray)
        {
            if (soundAudioClip.sound == sound)
                return soundAudioClip.audioClip;
        }
        Debug.LogError("Sound " + sound + " not found!");
        return null;
    }
    public void PlayCubeCollected()
    {
        PlaySound(Sound.CubeCollected);
    }
    public void PlayButtonClick()
    {
        PlaySound(Sound.ButtonClick);
    }
    public void PlayFinishedLevel()
    {
        Debug.Log(">Finished Level");
        PlaySound(Sound.FinishedLevel);
    }
    public void PlayLostLevel()
    {
        PlaySound(Sound.LostLevel);
    }
    public void PlayObstacleHit()
    {
        PlaySound(Sound.ObstacleHit);
    }

    public enum Sound
    {
        CubeCollected,
        ObstacleHit,
        LostLevel,
        FinishedLevel,
        ButtonClick
    }

    [System.Serializable]
    public class SoundAudioClip
    {
        public Sound sound;
        public AudioClip audioClip;
    }
}