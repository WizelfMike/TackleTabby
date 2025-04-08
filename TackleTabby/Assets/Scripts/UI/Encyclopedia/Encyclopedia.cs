using System;
using System.Collections.Generic;
using TMPro;
using Unity.Burst;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Encyclopedia : MonoBehaviour
{
    [Header("UI")]
    [SerializeField]
    private GameObject Container;
    [SerializeField]
    private TextMeshProUGUI FishNameDisplay;
    [SerializeField]
    private TextMeshProUGUI FishSizeDisplay;
    [SerializeField]
    private Image FishDisplayImage;
    [SerializeField]
    private Image[] BaitDisplayImages;
    [SerializeField]
    private EncyclopediaFishButton[] FishButtons;
    [SerializeField]
    private Animator OpenCloseAnimator;
    [Header("Settings")]
    [SerializeField]
    private bool KeepInfoOpenOnClose;

    private Dictionary<FishDefinition, CaughtFish> _fishProgress = new();
    private EncyclopediaFishButton _lastOpenedFishButton;

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
    }

    public void OpenEncyclopedia()
    {
        if (MenuCommunicator.Instance.HasMenuOpen)
            return;
        
        if (KeepInfoOpenOnClose && _lastOpenedFishButton != null)
            _lastOpenedFishButton.OnButtonPressed();
        
        OpenCloseAnimator.SetTrigger("OpenTrigger");
        MenuCommunicator.Instance.OpenMenu();
    }

    public void CloseEncyclopedia()
    {
        OpenCloseAnimator.SetTrigger("CloseTrigger");

        if (!KeepInfoOpenOnClose)
        {
            OpenFishInfo(false);
            foreach (EncyclopediaFishButton button in FishButtons)
                button.Exit();
        }

        MenuCommunicator.Instance.CloseMenu();
    }

    private void OnFishButtonPressed(EncyclopediaFishButton fishButton)
    {
        _lastOpenedFishButton = fishButton;
        foreach (EncyclopediaFishButton button in FishButtons)
            button.Exit();

        CaughtFish caught = _fishProgress[fishButton.FishType];
        
        OpenFishInfo();
        FishNameDisplay.SetText(caught.FishType.DisplayName);
        FishDisplayImage.sprite = caught.FishType.FishSprite;
        FishSizeDisplay.SetText($"{caught.CaughtSize:F1} inch");
        for (int i = 0; i < BaitDisplayImages.Length; i++)
            BaitDisplayImages[i].sprite = caught.FishType.RequiredBaitCombination[i].BaitSprite;
    }

    public void OpenFishInfo(bool enabledState = true)
    {
        FishNameDisplay.enabled = enabledState;
        FishDisplayImage.enabled = enabledState;
        FishSizeDisplay.enabled = enabledState;
        foreach (Image baitImage in BaitDisplayImages)
            baitImage.enabled = enabledState;
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
}
