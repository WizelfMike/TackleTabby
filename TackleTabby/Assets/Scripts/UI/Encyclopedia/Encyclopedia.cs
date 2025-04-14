using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Encyclopedia : MonoBehaviour, IOverlayMenu
{
    [Header("UI")]
    [SerializeField]
    private GameObject Container;
    [SerializeField]
    private TextMeshProUGUI FishNameDisplay;
    [SerializeField]
    private TextMeshProUGUI FishSizeDisplay;
    [SerializeField]
    private TextMeshProUGUI DescriptionText;
    [SerializeField]
    private Image FishDisplayImage;
    [SerializeField]
    private Image[] BaitDisplayImages;
    [SerializeField]
    private TextMeshProUGUI[] BaitDisplayLockedText;
    [SerializeField]
    private EncyclopediaFishButton[] FishButtons;
    
    [Header("Animation")]
    [SerializeField]
    private Animator OpenCloseAnimator;
    
    [Header("Settings")]
    [SerializeField]
    private bool KeepInfoOpenOnClose;
    [SerializeField]
    private float ReOpenTimeoutSeconds = 1f;

    [Header("Events")]
    public UnityEvent<IOverlayMenu> OnOpened;
    public UnityEvent<IOverlayMenu> OnClosed;

    private Dictionary<FishDefinition, CaughtFish> _fishProgress = new();
    private BaitDefinition[] _baitProgress = Array.Empty<BaitDefinition>();
    private EncyclopediaFishButton _lastOpenedFishButton;

    private DateTime _lastClosedTime = DateTime.MinValue;
    private DateTime _lastOpenedTime = DateTime.MinValue;

    private void Start()
    {
        OpenFishInfo(false);
        
        ReadOnlySpan<FishDefinition> allFishes = CentralFishStorage.Instance.GetAllFish();
        int length = allFishes.Length;
        if (length != FishButtons.Length)
            Debug.LogWarning("The amount of buttons in the encyclopedia is not equal to the amount of fishes");

        for (int i = 0; i < length; i++)
        {
            FishButtons[i].FishType = allFishes[i];
            FishButtons[i].OnPressed.AddListener(OnFishButtonPressed);
        }
    }

    public void OnFishCaught(CaughtFish fish)
    {
        int length = FishButtons.Length;
        int index = -1;
        for (int i = 0; i < length; i++)
        {
            if (FishButtons[i].FishType != fish.FishType)
                continue;

            index = i;
            break;
        }

        if (index == -1)
            return;
        
        TryAddCatchProgress(fish);
        FishButtons[index].Unlock();
        UpdateBaitProgress();
    }

    private void OnFishButtonPressed(EncyclopediaFishButton fishButton)
    {
        _lastOpenedFishButton = fishButton;
        foreach (EncyclopediaFishButton button in FishButtons)
            button.Exit();

        DescriptionText.enabled = false;
        
        if (_fishProgress.TryGetValue(fishButton.FishType, out CaughtFish caught))
        {
            DisplayAlreadyCaughtFish(caught);
        }
        else
        {
            DisplayLockedFish(fishButton.FishType);
        }
    }

    private void DisplayAlreadyCaughtFish(CaughtFish fish)
    {
        OpenFishInfo();
        
        FishNameDisplay.SetText(fish.FishType.DisplayName);
        FishDisplayImage.color = Color.white;
        FishDisplayImage.sprite = fish.FishType.FishSprite;
        FishSizeDisplay.SetText($"{fish.CaughtSize:F1} inch");

        for (int i = 0; i < BaitDisplayImages.Length; i++)
        {
            BaitDisplayLockedText[i].enabled = false;
            
            BaitDisplayImages[i].color = Color.white;
            BaitDisplayImages[i].sprite = fish.FishType.RequiredBaitCombination[i].BaitSprite;
        }
    }

    private void DisplayLockedFish(FishDefinition fishType)
    {
        OpenFishInfo();
        
        FishDisplayImage.color = Color.black;
        FishSizeDisplay.enabled = false;
        
        FishNameDisplay.SetText(fishType.DisplayName.Select(x => x == ' ' ? x : '?').ToArray());
        FishDisplayImage.sprite = fishType.FishSprite;

        for (int i = 0; i < BaitDisplayImages.Length; i++)
        {
            BaitDefinition baitType = fishType.RequiredBaitCombination[i];
            bool isUnlocked = _baitProgress.Contains(baitType);
            
            BaitDisplayImages[i].color = Color.black;
            BaitDisplayImages[i].sprite = fishType.RequiredBaitCombination[i].BaitSprite;
            BaitDisplayImages[i].enabled = isUnlocked;
            BaitDisplayLockedText[i].enabled = !isUnlocked;
        }
    }

    private void OpenFishInfo(bool enabledState = true)
    {
        FishNameDisplay.enabled = enabledState;
        FishDisplayImage.enabled = enabledState;
        FishSizeDisplay.enabled = enabledState;
        foreach (Image baitImage in BaitDisplayImages)
            baitImage.enabled = enabledState;
        foreach (TextMeshProUGUI baitLockedText in BaitDisplayLockedText)
            baitLockedText.enabled = enabledState;
    }

    private bool TryAddCatchProgress(CaughtFish fish)
    {
        if (!_fishProgress.TryGetValue(fish.FishType, out CaughtFish alreadyCaught))
        {
            _fishProgress.Add(fish.FishType, fish);
            return true;
        }

        if (alreadyCaught.CaughtSize > fish.CaughtSize)
            return false;

        _fishProgress[fish.FishType] = fish;
        return true;
    }

    public void OpenOverlay()
    {
        DateTime now = DateTime.Now;
        if ((now - _lastClosedTime).Seconds < ReOpenTimeoutSeconds ||
            (now - _lastOpenedTime).Seconds < ReOpenTimeoutSeconds)
            return;
        _lastOpenedTime = DateTime.Now;
        
        if (MenuCommunicator.Instance.HasMenuOpen)
            return;

        OpenCloseAnimator.SetTrigger("OpenTrigger");
        
        if (KeepInfoOpenOnClose && _lastOpenedFishButton != null)
        {
            _lastOpenedFishButton.OnButtonPressed();
        }
        else
        {
            if (_lastOpenedFishButton)
                _lastOpenedFishButton.Exit();
            
            OpenFishInfo(false);
        }

        DescriptionText.enabled = !(KeepInfoOpenOnClose && _lastOpenedFishButton != null);
        OnOpened.Invoke(this);
        MenuCommunicator.Instance.OpenedMenu(this);
    }

    public void CloseOverlay()
    {
        DateTime now = DateTime.Now;
        if ((now - _lastClosedTime).Seconds < ReOpenTimeoutSeconds ||
            (now - _lastOpenedTime).Seconds < ReOpenTimeoutSeconds)
            return;
        _lastClosedTime = DateTime.Now;
        
        OpenCloseAnimator.SetTrigger("CloseTrigger");

        if (!KeepInfoOpenOnClose)
        {
            OpenFishInfo(false);
            foreach (EncyclopediaFishButton button in FishButtons)
                button.Exit();
        }

        OnClosed.Invoke(this);
        MenuCommunicator.Instance.ClosedMenu(this);
    }

    public void ListenToOpen(UnityAction<IOverlayMenu> callback)
    {
        OnOpened.AddListener(callback);
    }

    public void StopListenToOpen(UnityAction<IOverlayMenu> callback)
    {
        OnOpened.RemoveListener(callback);
    }

    public void ListenToClose(UnityAction<IOverlayMenu> callback)
    {
        OnClosed.AddListener(callback);
    }

    public void StopListenToClose(UnityAction<IOverlayMenu> callback)
    {
        OnClosed.RemoveListener(callback);
    }

    private void UpdateBaitProgress()
    {
        List<BaitDefinition> intermediate = new();
        ReadOnlySpan<FishDefinition> keys = _fishProgress.Keys.ToArray();
        int keyCount = _fishProgress.Keys.Count;
        for (int i = 0; i < keyCount; i++)
            intermediate.AddRange(keys[i].RequiredBaitCombination);

        _baitProgress = intermediate.Distinct().ToArray();
    }
    
#if UNITY_EDITOR

    [ContextMenu("Unlocking/Unlock all")]
    private void UnlockAllFish()
    {
        int length = FishButtons.Length;
        for (int i = 0; i < length; i++)
            FishButtons[i].Unlock();
    }

    [ContextMenu("Unlocking/Relock uncaught")]
    private void RelockUncaught()
    {
        int length = FishButtons.Length;
        for (int i = 0; i < length; i++)
        {
            if (_fishProgress.ContainsKey(FishButtons[i].FishType))
                continue;
            
            FishButtons[i].Lock();
        }
    }
    
    #endif // UNITY_EDITOR
}
