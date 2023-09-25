using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance;

    public AudioSource _musicSource,_musicSource2,_musicSource3, _effectsSource,_effectsSourceLowVolume; 

    public  float LowVolume = 0.05f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        _effectsSourceLowVolume.volume = LowVolume;
        _musicSource2.volume = 0;
        
    }

    public void PlaySound(AudioClip clip)
    {
        _effectsSource.PlayOneShot(clip);
    }
    public void PlaySoundLow(AudioClip clip)
    {
        _effectsSourceLowVolume.PlayOneShot(clip);
    }
    public void ChangeMusic(int A)
    {
        StartCoroutine(PaceTheVolume(A));

    }

    private IEnumerator PaceTheVolume(int a)
    {

            while (_musicSource.volume > 0)
            {
                _musicSource.volume -= 0.1f;
                yield return new WaitForSeconds(0.4f);
            }
                _musicSource.enabled = false;


        if (a == 1) {
            _musicSource2.Play();
            _musicSource2.enabled = true;
            while (_musicSource2.volume < 1)
            {
                _musicSource2.volume += 0.1f;
                yield return new WaitForSeconds(1f);
            }
        }
        else if (a == 2)
        {
            _musicSource3.Play();
            _musicSource3.enabled = true;
            while (_musicSource3.volume < 1)
            {
                _musicSource3.volume += 0.1f;
                yield return new WaitForSeconds(1f);
            }
        }


       
    }

}
