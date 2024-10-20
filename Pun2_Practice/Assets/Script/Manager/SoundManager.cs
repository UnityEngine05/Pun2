using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SoundClip
{
    public AudioClip[] _BGMClip, _EffectClip;
}
public class SoundManager : MonoBehaviour
{
    public AudioSource _BGMAudioSource, _EffectAudioSource, _EffectAudioSource2;
    public Slider _BGMSlider, _EffectSlider;

    public SoundClip _SoundClip;
    // Start is called before the first frame update
    void Start()
    {
        _BGMSlider.value = 0.25f;
        _EffectSlider.value = 1;
        BGMSoundPlay(0);
    }

    public void LateUpdate()
    {
        if(Input.GetMouseButtonDown(0))
        {
            EffectSoundPlay(6);
        }
    }


    public void BGMSoundVolumeSetting()
    {
        _BGMAudioSource.volume = _BGMSlider.value;
    }

    public void EffectSoundVolumeSetting()
    {
        _EffectAudioSource.volume = _EffectSlider.value;
        _EffectAudioSource2.volume = _EffectSlider.value;
    }

    public void BGMSoundPlay(int num)
    {
        _BGMAudioSource.Stop();
        _BGMAudioSource.clip = _SoundClip._BGMClip[num];
        _BGMAudioSource.Play();
    }

    public void EffectSoundPlay(int num)
    {
        _EffectAudioSource2.PlayOneShot(_SoundClip._EffectClip[num]);
    }

    public void EffectSoundPlayStop(bool playSound)
    {
        if(playSound)
        {
            _EffectAudioSource.Play();
        }
        else
        {
            _EffectAudioSource.Stop();
        }
    }
}
