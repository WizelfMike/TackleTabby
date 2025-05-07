using UnityEngine;

public class AnimationEventHolder : MonoBehaviour
{
    [SerializeField]
    private CaughtFishPopup fishPopUp;

    [SerializeField]
    private AudioHandler audioHandler;

    public void PopUpFish()
    {
        PlayAnimationAudio("event:/Fish Catch");
        fishPopUp.OpenOverlay();
    }

    public void PlayAnimationAudio(string fmodEvent)
    {
        audioHandler.PlayFmodOneShot(fmodEvent);
    }
}
