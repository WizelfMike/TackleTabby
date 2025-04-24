using UnityEngine;
using UnityEngine.Events;

public class SettingsPanel : MonoBehaviour, IOverlayMenu
{

    [Header("MenuReference")]
    [SerializeField]
    private GameObject SettingsMenu;
    [SerializeField]
    private Animator SettingsAnimation;

    [Header("Events")]
    public UnityEvent<IOverlayMenu> OnOpened;
    public UnityEvent<IOverlayMenu> OnClosed;

    private bool _isOpen = false;

    public void CloseOverlay()
    {
        if (!_isOpen)
            return;

        OnClosed.Invoke(this);

        SettingsAnimation.SetTrigger("PopDown");

        MenuCommunicator.Instance.ClosedMenu(this);

        _isOpen = false;
    }

    public void ListenToClose(UnityAction<IOverlayMenu> callback)
    {
        OnClosed.AddListener(callback);
    }

    public void ListenToOpen(UnityAction<IOverlayMenu> callback)
    {
        OnOpened.AddListener(callback);
    }

    public void OpenOverlay()
    {
        if (_isOpen)
            return;

        OnOpened.Invoke(this);

        SettingsAnimation.SetTrigger("PopUp");

        MenuCommunicator.Instance.OpenedMenu(this);

        _isOpen = true;
    }

    public void StopListenToClose(UnityAction<IOverlayMenu> callback)
    {
        OnClosed.RemoveListener(callback);
    }

    public void StopListenToOpen(UnityAction<IOverlayMenu> callback)
    {
        OnOpened.RemoveListener(callback);
    }

    public void CallButtonOnOff()
    {
        if (!_isOpen)
            OpenOverlay();
        else
            CloseOverlay();
    }
}
