using UnityEngine.Events;

public interface IOverlayMenu
{
    public void OpenOverlay();
    public void CloseOverlay();

    public void ListenToOpen(UnityAction<IOverlayMenu> callback);
    public void StopListenToOpen(UnityAction<IOverlayMenu> callback);

    public void ListenToClose(UnityAction<IOverlayMenu> callback);
    public void StopListenToClose(UnityAction<IOverlayMenu> callback);
}