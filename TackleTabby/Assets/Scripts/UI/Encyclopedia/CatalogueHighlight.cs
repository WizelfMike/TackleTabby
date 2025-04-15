using System;
using UnityEngine;
using UnityEngine.Events;

public class CatalogueHighlight : MonoBehaviour //, IOverlayMenu
{
    [SerializeField] 
    private GameObject FieldGrayout;

    private bool _hasBeenOpened;
    
    //public UnityEvent<IOverlayMenu> OnOpened;
   // public UnityEvent<IOverlayMenu> OnClosed;
    
    public void Open()
    {
        if (_hasBeenOpened)
            return;
        _hasBeenOpened = true;
        gameObject.SetActive(true);
        FieldGrayout.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
        FieldGrayout.SetActive(false);
    }
}
