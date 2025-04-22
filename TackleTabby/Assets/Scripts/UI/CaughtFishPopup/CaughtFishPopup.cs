using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CaughtFishPopup : MonoBehaviour, IOverlayMenu
{
    [Header("UI")]
    [SerializeField]
    private Image CaughtDisplay;
    [SerializeField]
    private TextMeshProUGUI TextMessage;

    [Header("Animation")]
    [SerializeField]
    private Animator OpenCloseAnimator;

    [Header("System")]
    [SerializeField]
    private FishManager FishManager;
    
    [Header("Events")]
    public UnityEvent<IOverlayMenu> OnOpened;
    public UnityEvent<IOverlayMenu> OnClosed;

    private readonly Queue<CaughtFish> _fishCaughtQueue = new();
    private readonly Queue<TrashDefinition> _trashCaughtQueue = new();
    private bool _isOpen = false;

    private void Start()
    {
        FishManager.OnFishCaught.AddListener(OnCaughtFish);
        // FishManager.OnTrashCaught.AddListener(OnCaughtTrash);
    }

    private void OnCaughtFish(CaughtFish fish)
    {
        _fishCaughtQueue.Enqueue(fish);

        if (_isOpen)
            return;
        
        OpenOverlay();
    }

    private void OnCaughtTrash(TrashDefinition trash)
    {
        _trashCaughtQueue.Enqueue(trash);

        if (_isOpen)
            return;
        
        OpenOverlay();
    }

    public void NextInQueue()
    {
        if (!_isOpen)
            return;

        if (GetFromFishQueue(out CaughtFish fish))
        {
            DisplayFish(fish);
            return;
        }

        if (GetFromTrashQueue(out TrashDefinition trash))
        {
            DisplayTrash(trash);
            return;
        }
        
        CloseOverlay();
    }
    
    public void OpenOverlay()
    {
        if (MenuCommunicator.Instance.HasMenuOpen && MenuCommunicator.Instance.CurrentMenu != this)
            MenuCommunicator.Instance.ForceCloseCurrentMenu();
        
        OpenCloseAnimator.SetTrigger("OpenTrigger");
        _isOpen = true;
        NextInQueue();
        OnOpened.Invoke(this);
        MenuCommunicator.Instance.OpenedMenu(this);
    }

    public void CloseOverlay()
    {
        OpenCloseAnimator.SetTrigger("CloseTrigger");
        _isOpen = false;
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

    private bool GetFromFishQueue(out CaughtFish fish)
    {
        return _fishCaughtQueue.TryDequeue(out fish);
    }

    private bool GetFromTrashQueue(out TrashDefinition trash)
    {
        return _trashCaughtQueue.TryDequeue(out trash);
    }

    private void DisplayFish(CaughtFish fish)
    {
        FishDefinition fishType = fish.FishType.Expand();
        CaughtDisplay.sprite = fishType.FishSprite;
        TextMessage.SetText($"You caugth a(n) {fishType.DisplayName} of {fish.CaughtSize:F1} inch");
    }

    private void DisplayTrash(TrashDefinition trash)
    {
        CaughtDisplay.sprite = trash.TrashSprite;
        TextMessage.SetText($"You caught some trash: {trash.DisplayName}. :(");
    }
}