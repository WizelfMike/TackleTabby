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
    private Canvas CookBookCanvas;

    
    [SerializeField] 
    private GameObject CatalogueHighlight;
    [SerializeField]
    private GameObject HungerBarHighlight;
    [SerializeField]
    private GameObject CookBookHighlight;
    
    [SerializeField]
    private float WaitingTime = 0.5f;

    [SerializeField] 
    private Animator Animator;

    public UnityEvent<IOverlayMenu> OnOpened;
    public UnityEvent<IOverlayMenu> OnClosed;

    public bool AlreadyCaughtFish => _alreadyCaughtFish;
    public bool AlreadyCaughtTrash => _alreadyCaughtTrash;
    public bool AlreadyShowedCookBook => _alreadyShowedCookBook;
    
    private GameObject _currentHighlight;
    private Canvas _currentCanvas;
    private bool _mayCheck = true;
    private bool _alreadyCaughtFish;
    private bool _alreadyCaughtTrash;
    private bool _alreadyShowedCookBook;
    private bool _isOpen;
    private string _currentAnimation;

    public void SetTutorialProgress(bool alreadyCaughtFish, bool alreadyCaughtTrash)
    {
        _alreadyCaughtFish = alreadyCaughtFish;
        _alreadyCaughtTrash = alreadyCaughtTrash;
    }
    
    public void HasClosed()
    {
        _mayCheck = true;
    }

    public void CheckCaught(string tutorialName) 
    {
        if (!_mayCheck)
            return;

        switch (tutorialName)
        {
            case "Fish":
                if (_alreadyCaughtFish)
                    break;
                
                _alreadyCaughtFish = true;
                _currentAnimation = "IsCatalogue";
                OpenHighlight(CatalogueCanvas, CatalogueHighlight);
                break;
            
            case "Trash":
                if (_alreadyCaughtTrash)
                    break;
                
                _alreadyCaughtTrash = true;
                _currentAnimation = "IsHungerBar";
                OpenHighlight(HungerBarCanvas, HungerBarHighlight);
                break;
            
            case "CookBook":
                if (_alreadyShowedCookBook)
                    break;
                
                _currentAnimation = "IsCookBook";
                OpenHighlight(CookBookCanvas, CookBookHighlight);
                break;
        }
    }

    public void CorrectlyClosedCookBook()
    {
        _alreadyShowedCookBook = true;
        CloseOverlay();
    }
    
    private void OpenHighlight(Canvas currentCanvas, GameObject currentHighlight)
    {
        _mayCheck = false;
        _currentCanvas = currentCanvas;
        _currentHighlight = currentHighlight;
        Animator.SetBool(_currentAnimation, true);
        OpenOverlay();
    }

    public void Pauze()
    {
        PauzeTime.Pauze();
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
        CookBookCanvas.sortingOrder = -1;
        Animator.SetBool(_currentAnimation, false);
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