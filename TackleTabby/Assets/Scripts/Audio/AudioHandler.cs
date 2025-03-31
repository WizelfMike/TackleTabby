using FMODUnity;
using UnityEngine;

public class AudioHandler : MonoBehaviour
{
    private void Start()
    {
        FMODUnity.RuntimeManager.WaitForAllSampleLoading();    
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
