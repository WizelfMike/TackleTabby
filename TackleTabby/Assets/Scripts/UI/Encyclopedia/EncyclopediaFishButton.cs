using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EncyclopediaFishButton : MonoBehaviour
{
    [SerializeField]
    private Image FishThumbnail;
    [SerializeField]
    private Image ButtonImage;

    [SerializeField]
    private Sprite ButtonSpriteActive;
    [SerializeField]
    private Sprite ButtonSpriteInactive;

    public UnityEvent<FishDefinition> OnPressed = new();

    public FishDefinition FishType
    {
        get => _fishType;
        set
        {
            if (value == _fishType)
                return;
            _fishType = value;
            FishThumbnail.sprite = _fishType.ThumbnailSprite;
        }
    }
    
    private bool _isUnlocked = false;
    private FishDefinition _fishType;

    public void Unlock()
    {
        _isUnlocked = true;
        FishThumbnail.color = Color.white;
    }

    public void Exit()
    {
        ButtonImage.sprite = ButtonSpriteInactive;
    }

    public void OnButtonPressed()
    {
        if (!_isUnlocked)
            return;

        OnPressed.Invoke(_fishType);
        ButtonImage.sprite = ButtonSpriteActive;
    }
}
