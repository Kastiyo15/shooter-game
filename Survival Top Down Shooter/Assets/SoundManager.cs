using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private AudioSource _effectsSourcePlayer, _effectsSourcePlayer2, _effectsSourceEnemy, _effectsSourceDeath, _effectsSourceDeath2, _effectsSourceImpact, _effectsSourceImpact2, _musicSource;
    public AudioClip[] PlayerShootClips;
    public AudioClip[] EnemyShootClips;
    public AudioClip[] DeathClips;
    public AudioClip[] ImpactClips;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void PlaySoundPlayer(AudioClip clip)
    {
        // player
        if (!_effectsSourcePlayer.isPlaying)
        {
            _effectsSourcePlayer.clip = clip;
            _effectsSourcePlayer.pitch = Random.Range(0.85f, 1.05f);
            _effectsSourcePlayer.volume = 0.9f;
            _effectsSourcePlayer.Play();
        }
        else
        {
            _effectsSourcePlayer2.clip = clip;
            _effectsSourcePlayer2.pitch = Random.Range(0.95f, 1.15f);
            _effectsSourcePlayer2.volume = 1f;
            _effectsSourcePlayer2.Play();
        }

        //_effectsSourcePlayer.PlayOneShot(clip);
    }


    public void PlaySoundEnemy(AudioClip clip)
    {
        // Enemy
        _effectsSourceEnemy.clip = clip;
        _effectsSourceEnemy.pitch = Random.Range(0.8f, 1.05f);
        _effectsSourceEnemy.volume = 0.35f;
        _effectsSourceEnemy.Play();
        //_effectsSourceEnemy.PlayOneShot(clip);
    }


    public void PlaySoundImpact(AudioClip clip)
    {
        // Impact
        if (!_effectsSourceImpact.isPlaying)
        {
            _effectsSourceImpact.clip = clip;
            _effectsSourceImpact.pitch = Random.Range(0.8f, 1f);
            _effectsSourceImpact.volume = 0.45f;
            _effectsSourceImpact.Play();
        }
        else
        {
            _effectsSourceImpact2.clip = clip;
            _effectsSourceImpact2.pitch = Random.Range(0.9f, 1.1f);
            _effectsSourceImpact2.volume = 0.5f;
            _effectsSourceImpact2.Play();
        }
        //_effectsSourceImpact.PlayOneShot(clip);
    }


    public void PlaySoundDeath(AudioClip clip)
    {
        // Death
        if (!_effectsSourceDeath.isPlaying)
        {
            _effectsSourceImpact.Stop();
            _effectsSourceDeath.clip = clip;
            _effectsSourceDeath.pitch = Random.Range(0.95f, 0.97f);
            _effectsSourceDeath.volume = 0.9f;
            _effectsSourceDeath.Play();
        }
        else
        {
            _effectsSourceImpact2.Stop();
            _effectsSourceDeath2.clip = clip;
            _effectsSourceDeath2.pitch = Random.Range(0.95f, 0.97f);
            _effectsSourceDeath2.volume = 1f;
            _effectsSourceDeath2.Play();
            //_effectsSourceDeath.PlayOneShot(clip);
        }
    }
}
