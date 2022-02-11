using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager: MonoBehaviour
{
    [SerializeField]
    private SoundAudioClip[] soundAudioClipArray;

    private GameObject oneShotGO;
    private AudioSource oneShotAudioSource;
    private void PlaySound(Sound sound) //Play sound in a single game object
    {
        if(oneShotGO == null)
        {
            oneShotGO = new GameObject("Sound");
            oneShotAudioSource = oneShotGO.AddComponent<AudioSource>();
        }
        oneShotAudioSource.PlayOneShot(GetAudioClip(sound));
    }

    private AudioClip GetAudioClip(Sound sound) //Play sound for given enum Sound
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
    public void PlayEatCheese()
    {
        PlaySound(Sound.EatCheese);
    }

    public enum Sound //All possible game sounds
    {
        CubeCollected,
        ObstacleHit,
        LostLevel,
        FinishedLevel,
        ButtonClick,
        EatCheese
    }

    [System.Serializable]
    public class SoundAudioClip
    {
        public Sound sound;
        public AudioClip audioClip;
    }
}
