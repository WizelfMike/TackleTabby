using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour, IOverlayMenu
{
    [SerializeField]
    private bool OpenedAtStart = true;

    [Header("UI")]
    [SerializeField]
    private Button StartGameButton;
    [SerializeField]
    private Button QuitGameButton;
    
    [Header("Animation")]
    [SerializeField]
    private Animator OpenCloseAnimator;
    
    [Header("Events")]
    public UnityEvent<IOverlayMenu> OnOpened;
    public UnityEvent<IOverlayMenu> OnClosed;

    private bool _isOpen;

    private void Start()
    {
        _isOpen = OpenedAtStart;
        StartGameButton.onClick.AddListener(OnStartPressed);
        QuitGameButton.onClick.AddListener(OnQuitPressed);

        if (_isOpen)
            MenuCommunicator.Instance.OpenedMenu(this);
    }

    private void OnStartPressed()
    {
        CloseOverlay();
    }

    private void OnQuitPressed()
    {
        Application.Quit(0);
    }
    
    public void OpenOverlay()
    {
        if (MenuCommunicator.Instance.HasMenuOpen || _isOpen)
            return;
        
        _isOpen = true;
        OpenCloseAnimator.SetTrigger("OpenTrigger");
        OnOpened.Invoke(this);
        MenuCommunicator.Instance.OpenedMenu(this);
    }

    public void CloseOverlay()
    {
        if (!_isOpen)
            return;

        _isOpen = false;
        OpenCloseAnimator.SetTrigger("CloseTrigger");
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
}