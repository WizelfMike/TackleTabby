using FMODUnity;
using UnityEngine;

public class AudioHandler : MonoBehaviour
{
    [Header("Volume Controls")]
    [SerializeField]
    private float MusicVolume = 1f;
    [SerializeField]
    private float SFXVolume = 1f;
    [SerializeField]
    private float MasterVolume = 2f;

    private FMOD.Studio.Bus _musicBus;
    private FMOD.Studio.Bus _sfxBus;
    private FMOD.Studio.Bus _masterBus;

    private FMOD.Studio.EventInstance _backgroundMusic;

    private void Start()
    {
        FMODUnity.RuntimeManager.WaitForAllSampleLoading();

        _musicBus = FMODUnity.RuntimeManager.GetBus("bus:/Master/Music");
        _sfxBus = FMODUnity.RuntimeManager.GetBus("bus:/Master/Sound Effects");
        _masterBus = FMODUnity.RuntimeManager.GetBus("bus:/Master");

        _backgroundMusic = FMODUnity.RuntimeManager.CreateInstance("event:/Match3Music");
        _backgroundMusic.start();

        _backgroundMusic.setParameterByName("Catalogue Open", 0);
    }

    public void PlayFmodOneShot(string EventDirectory)
    {
        FMODUnity.RuntimeManager.PlayOneShot(EventDirectory);
    }

    public void ChangeMusicParameter(int ParameterValue)
    {
        _backgroundMusic.setParameterByName("Catalogue Open", ParameterValue);
    }

    public void SetMusicVolume(float volume)
    {
        MusicVolume = volume;

        _musicBus.setVolume(MusicVolume);
    }

    public void SetSFXVolume(float volume)
    {
        SFXVolume = volume;

        _sfxBus.setVolume(SFXVolume);
    }

    public void SetMasterVolume(float volume)
    {
        MasterVolume = volume;

        _masterBus.setVolume(MasterVolume);
    }

}
