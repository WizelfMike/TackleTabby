using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class WinLoseBehaviour : MonoBehaviour, IOverlayMenu
{
    [SerializeField]
    private GameObject EndPanel;
    [SerializeField]
    private Animator EndPanelAnimator;

    [Header("Events")]
    public UnityEvent<IOverlayMenu> OnOpened;
    public UnityEvent<IOverlayMenu> OnClosed;

    private bool _isOpen = false;

    [ContextMenu("UI Tests/Open UI")]
    public void OpenOverlay()
    {
        if (!enabled || !gameObject.activeSelf)
            return;
        
        if (_isOpen)
            return;

        StartCoroutine(OpenOverlayCoroutine());
        _isOpen = true;
    }

    [ContextMenu("UI Tests/Close UI")]
    public void CloseOverlay()
    {
        EndPanelAnimator.SetTrigger("ExitWin");
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

    private IEnumerator OpenOverlayCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        while (MenuCommunicator.Instance.HasMenuOpen)
            yield return new WaitForSeconds(0.5f);

        EndPanel.SetActive(true);

        EndPanelAnimator.SetTrigger("PlayWin");

        OnOpened.Invoke(this);

        MenuCommunicator.Instance.OpenedMenu(this);
        yield break;
    }
}
