using FMODUnity;
using UnityEngine;

public class AudioHandler : MonoBehaviour
{

    private FMOD.Studio.EventInstance _backgroundMusic;

    private void Start()
    {
        FMODUnity.RuntimeManager.WaitForAllSampleLoading();

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
}
