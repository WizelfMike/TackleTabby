using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class WinLoseBehaviour : MonoBehaviour, IOverlayMenu
{
    [SerializeField]
    private Animator Animator;

    [Header("Events")]
    public UnityEvent<IOverlayMenu> OnOpened;
    public UnityEvent<IOverlayMenu> OnClosed;

    private bool _isOpen = false;
    private bool _isPrepared = false;
    private bool _closed = false;

    public void Prepare()
    {
        _isPrepared = true;
    }

    [ContextMenu("UI Tests/Open UI")]
    public void OpenOverlay()
    {
        if (!enabled || !gameObject.activeSelf)
            return;

        if (!_isPrepared || _closed)
            return;
        
        if (_isOpen)
            return;

        StartCoroutine(OpenOverlayCoroutine());
        _isOpen = true;
        _isPrepared = false;
    }

    [ContextMenu("UI Tests/Close UI")]
    public void CloseOverlay()
    {
        Animator.SetTrigger("ExitWin");
        _isOpen = false;
        _closed = true;

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
        yield return new WaitUntil(() => !MenuCommunicator.Instance.HasMenuOpen);

        Animator.SetTrigger("PlayWin");

        OnOpened.Invoke(this);

        MenuCommunicator.Instance.OpenedMenu(this);
        yield break;
    }
}
