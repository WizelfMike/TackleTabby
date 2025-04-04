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
    }

    public void PlayBaitDropSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/BaitLand");
    }

    public void PlayBaitPop()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/BaitPop");
    }

    public void PlayBaitSwap()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Swap");
    }
}
