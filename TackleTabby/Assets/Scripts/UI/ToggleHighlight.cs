using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
public class ToggleHighlight : MonoBehaviour , IOverlayMenu
{
    [SerializeField]
    private Canvas CatalogueCanvas;
    [SerializeField] 
    private Canvas HungerBarCanvas;

    [SerializeField] 
    private GameObject FieldGrayout;
    [SerializeField] 
    private GameObject CatalogueHighlight;
    [SerializeField]
    private GameObject HungerBarHighlight;
    
    [SerializeField]
    private float WaitingTime = 0.5f;

    [SerializeField] 
    private Animator Animator;
    
    public UnityEvent<IOverlayMenu> OnOpened;
    public UnityEvent<IOverlayMenu> OnClosed;

    private GameObject _currentHighlight;
    private Canvas _currentCanvas;
    private bool _mayCheck = true;
    private bool _alreadyCaughtFish;
    private bool _alreadyCaughtTrash;
    private bool _isOpen;
    
    public void HasClosed()
    {
        _mayCheck = true;
    }

    public void CheckCaught(bool caughtAFish)
    {
        if (!_mayCheck)
            return;
        
        if (caughtAFish && !_alreadyCaughtFish)
        {
            _mayCheck = false;
            _alreadyCaughtFish = true;
            _currentCanvas = CatalogueCanvas;
            _currentHighlight = CatalogueHighlight;
            Animator.SetBool("IsCatalogue", true);
            OpenOverlay();
            return;
        }
        
        if (_alreadyCaughtTrash)
            return;
        
        _mayCheck = false;
        _alreadyCaughtTrash = true;
        _currentCanvas = HungerBarCanvas;
        _currentHighlight = HungerBarHighlight;
        Animator.SetBool("IsCatalogue", false);
        OpenOverlay();
    }
    
    public void OpenOverlay()
    {
        if (_isOpen)
            return;

        StartCoroutine(OpenOverlayCoroutine());
        _isOpen = true;
    }

    public void CloseOverlay()
    {
        if (_currentHighlight == null)
            return;
        
        CatalogueCanvas.sortingOrder = -1;
        HungerBarCanvas.sortingOrder = -1;
        Animator.SetTrigger("CloseTrigger");
        _isOpen = false;
        MenuCommunicator.Instance.ClosedMenu(this);
    }

    private IEnumerator OpenOverlayCoroutine()
    {
        yield return new WaitForSeconds(WaitingTime);
        while (MenuCommunicator.Instance.HasMenuOpen)
            yield return new WaitForSeconds(WaitingTime);
        
        _currentCanvas.sortingOrder = 1;
        Animator.SetTrigger("OpenTrigger");
        MenuCommunicator.Instance.OpenedMenu(this);
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
