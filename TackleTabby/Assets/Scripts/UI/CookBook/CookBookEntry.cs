using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct CookBookSetting
{
    public bool HideLockedBait;
    public bool HideLockedFish;
}

public class CookBookEntry : MonoBehaviour
{
    [Header("Display References")]
    [SerializeField]
    private Image FishImage;
    [SerializeField]
    private Image BaitImage;

    [Header("Animation")]
    [SerializeField]
    private Animator Animator;

    private Sprite _displayingFishSprite;
    private Sprite _displayingBaitSprite;
    private CookBookSetting _settings;
    private bool _isOpen;

    public void SetSettings(CookBookSetting settings)
    {
        _settings = settings;
    }

    public void DisplayWith(FishDefinition fishType, BaitDefinition bait, bool isFishLocked, bool isBaitLocked)
    {
        DisplayFish(fishType, isFishLocked);
        DisplayBait(bait, isBaitLocked);
        
        if (!_isOpen)
            Animator.SetTrigger("OpenTrigger");

        _isOpen = true;
    }

    public void Close()
    {
        _displayingFishSprite = null;
        _displayingBaitSprite = null;
        
        if (_isOpen)
            Animator.SetTrigger("CloseTrigger");
        
        _isOpen = false;
    }

    private void DisplayFish(FishDefinition fishType, bool isLocked)
    {
        _displayingFishSprite = fishType.ThumbnailSprite;
        FishImage.sprite = _displayingFishSprite;
        FishImage.color = isLocked && _settings.HideLockedFish ? Color.black : Color.white;
    }

    private void DisplayBait(BaitDefinition bait, bool isLocked)
    {
        _displayingBaitSprite = bait.BaitSprite;
        BaitImage.sprite = _displayingBaitSprite;
        BaitImage.color = isLocked && _settings.HideLockedBait ? Color.black : Color.white;
    }
}