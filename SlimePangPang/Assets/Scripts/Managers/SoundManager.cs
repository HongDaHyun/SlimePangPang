using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum SFXType { Pop, Drop, Button, Over, Unlock };

public class SoundManager : Singleton<SoundManager>
{
    public AudioSource bgmPlayer;
    public AudioSource[] sfxPlayer;
    public SFXClip[] sfxClip;

    private int sfxCursor;

    private void Start()
    {
        GameManager gm = GameManager.Instance;

        bgmPlayer.volume = gm.bgmVolume;
        foreach(AudioSource source in sfxPlayer)
        {
            source.volume = gm.sfxVolume;
        }

        bgmPlayer.Play();
    }

    public void SFXPlay(SFXType type, int i)
    {
        AudioClip[] clips = FindSFXClip(type);

        // i가 0일시 해당 타입의 랜덤한 클립 재생
        if (i == 0)
            sfxPlayer[sfxCursor].clip = clips[UnityEngine.Random.Range(0, clips.Length)];

        // i가 1이상 일시 배열의 해당 index값이 i-1인 클립 재생
        else
            sfxPlayer[sfxCursor].clip = clips[i - 1];

        sfxPlayer[sfxCursor].Play();
        sfxCursor = (sfxCursor + 1) % sfxPlayer.Length;
    }

    private AudioClip[] FindSFXClip(SFXType type)
    {
        return Array.Find(sfxClip, clip => clip.type == type).sfxClip;
    }
}

[Serializable]
public struct SFXClip
{
    public SFXType type;
    public AudioClip[] sfxClip;
}
