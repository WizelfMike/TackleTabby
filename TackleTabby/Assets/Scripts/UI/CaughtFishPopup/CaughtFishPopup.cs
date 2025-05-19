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
    [SerializeField]
    private string DisplayBoolName;

    [Header("System")]
    [SerializeField]
    private FishManager FishManager;
    
    [Header("Events")]
    public UnityEvent<IOverlayMenu> OnOpened;
    public UnityEvent<IOverlayMenu> OnClosed;

    private bool IsOpen
    {
        get => _isOpen;
        set
        {
            if (value == _isOpen)
                return;

            _isOpen = value;
            OpenCloseAnimator.SetBool(DisplayBoolName, _isOpen);
        }
    }

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
    }

    private void OnCaughtTrash(TrashDefinition trash)
    {
        _trashCaughtQueue.Enqueue(trash);

        if (_isOpen)
            return;
        
        OpenOverlay();
    }

    private bool TestQueue()
    {
        return _fishCaughtQueue.Count + _trashCaughtQueue.Count > 0;
    }

    private bool NextInQueue()
    {
        if (!_isOpen || !TestQueue())
            return false;

        if (GetFromFishQueue(out CaughtFish fish))
        {
            DisplayFish(fish);
            return true;
        }

        if (GetFromTrashQueue(out TrashDefinition trash))
        {
            DisplayTrash(trash);
            return true;
        }
        
        return false;
    }

    public void ConfirmToNext()
    {
        if (!NextInQueue())
            CloseOverlay();
    }
    
    [ContextMenu("Overlay/Open")]
    public void OpenOverlay()
    {
        if (!enabled || !gameObject.activeSelf)
            return;
        
        if (!TestQueue())
            return;
        
        if (MenuCommunicator.Instance.HasMenuOpen && !ReferenceEquals(MenuCommunicator.Instance.CurrentMenu, this))
            MenuCommunicator.Instance.ForceCloseCurrentMenu();

        bool previousOpen = IsOpen;
        
        IsOpen = true;
        
        if (!previousOpen)
        {
            NextInQueue();
            OnOpened.Invoke(this);
        }
        
        MenuCommunicator.Instance.OpenedMenu(this);
    }

    [ContextMenu("Overlay/Close")]
    public void CloseOverlay()
    {
        IsOpen = false;
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