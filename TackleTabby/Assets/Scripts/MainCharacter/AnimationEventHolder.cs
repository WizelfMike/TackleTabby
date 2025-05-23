using UnityEngine;

public class AnimationEventHolder : MonoBehaviour
{
    [SerializeField]
    private CaughtFishPopup FishPopUp;
    [SerializeField]
    private AudioHandler AudioHandler;
    public void PopUpFish()
    {
        PlayAnimationAudio("event:/Fish Catch");
        FishPopUp.OpenOverlay();
    }

    public void PlayAnimationAudio(string fmodEvent)
    {
        AudioHandler.PlayFmodOneShot(fmodEvent);
    }
}
