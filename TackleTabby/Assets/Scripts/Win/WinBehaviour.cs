using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class WinBehaviour : MonoBehaviour, IOverlayMenu
{
    [SerializeField]
    private GameObject WinPanel;
    [SerializeField]
    private Animator WinPanelAnimator;

    [Header("Events")]
    public UnityEvent<IOverlayMenu> OnOpened;
    public UnityEvent<IOverlayMenu> OnClosed;

    private bool _isOpen = false;

    [ContextMenu("UI Tests/Open UI")]
    public void OpenOverlay()
    {
        if (_isOpen)
            return;

        StartCoroutine(OpenOverlayCoroutine());
        _isOpen = true;
    }

    [ContextMenu("UI Tests/Close UI")]
    public void CloseOverlay()
    {
        WinPanelAnimator.SetTrigger("ExitWin");
        _isOpen = false;

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

    private IEnumerator OpenOverlayCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        while (MenuCommunicator.Instance.HasMenuOpen)
            yield return new WaitForSeconds(0.5f);

        WinPanel.SetActive(true);

        WinPanelAnimator.SetTrigger("PlayWin");

        MenuCommunicator.Instance.OpenedMenu(this);
        yield break;
    }
}
