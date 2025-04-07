using TMPro;
using Unity.Burst;
using UnityEngine;
using UnityEngine.UI;

public class Encyclopedia : MonoBehaviour
{
    [Header("UI")]
    [SerializeField]
    private GameObject Container;
    [SerializeField]
    private TextMeshProUGUI FishNameDisplay;
    [SerializeField]
    private Image FishDisplayImage;
    [SerializeField]
    private Image[] BaitDisplayImages;
    [SerializeField]
    private EncyclopediaFishButton[] FishButtons;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        var allFishes = CentralFishStorage.Instance.GetAllFish();
        var length = allFishes.Length;
        if (length != FishButtons.Length)
            Debug.LogWarning("The amount of buttons in the encyclopedia is not equal to the amount of fishes");

        for (int i = 0; i < length; i++)
        {
            FishButtons[i].FishType = allFishes[i];
            FishButtons[i].OnPressed.AddListener(OnFishButtonPressed);
            FishButtons[i].Unlock();
        }
    }

    public void OpenEncyclopedia()
    {
        if (MenuCommunicator.Instance.HasMenuOpen)
            return;
        
        Container.SetActive(true);
        MenuCommunicator.Instance.OpenMenu();
    }

    public void CloseEncyclopedia()
    {
        OpenFishInfo(false);
        Container.SetActive(false);
        MenuCommunicator.Instance.CloseMenu();
    }

    private void OnFishButtonPressed(FishDefinition fishType)
    {
        foreach (EncyclopediaFishButton button in FishButtons)
            button.Exit();
        
        OpenFishInfo();
        FishNameDisplay.SetText(fishType.DisplayName);
        FishDisplayImage.sprite = fishType.FishSprite;
        for (int i = 0; i < BaitDisplayImages.Length; i++)
            BaitDisplayImages[i].sprite = fishType.RequiredBaitCombination[i].BaitSprite;
    }

    public void OpenFishInfo(bool enabledState = true)
    {
        FishNameDisplay.enabled = enabledState;
        FishDisplayImage.enabled = enabledState;
        foreach (Image baitImage in BaitDisplayImages)
            baitImage.enabled = enabledState;
    }
}
