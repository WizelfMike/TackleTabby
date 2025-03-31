using UnityEngine;

public class AudioHandler : MonoBehaviour
{
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
