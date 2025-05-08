using System;
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
        _displayingFishSprite = fishType.ThumbnailSprite;
        _displayingBaitSprite = bait.BaitSprite;

        FishImage.sprite = _displayingFishSprite;
        BaitImage.sprite = _displayingBaitSprite;
        
        
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
}