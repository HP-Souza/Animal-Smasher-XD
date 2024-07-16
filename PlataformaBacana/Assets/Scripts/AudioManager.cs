using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public AudioSource SFX;
    public AudioClip deathSFX;
    public AudioClip colectableHPSFX;
    public AudioClip colectableAmmoSFX;
    public AudioClip damagedSFX;
    public AudioClip enemyDeathSFX;
    public AudioClip coinSFX;
    public AudioClip secretDeathSFX;
    public AudioClip winSFX;


    public void SFXmanager(AudioClip SFXclip, float volume)
    {
        SFX.PlayOneShot(SFXclip, volume);
    }
}
