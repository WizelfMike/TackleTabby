using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class EncyclopediaOpenBtn : MonoBehaviour
{
    [SerializeField]
    private Animator Animator;

    private bool _madeFirstCombo = false;
    private bool _animating = false;
    private Button _button;
    
    private static readonly int _onAttentionTrigger = Animator.StringToHash("OnAttentionTrigger");
    private static readonly int _onTempAttentionTrigger = Animator.StringToHash("OnTempAttentionTrigger");
    private static readonly int _stopAttentionTrigger = Animator.StringToHash("StopAttentionTrigger");

    public void OnEncyclopediaOpened()
    {
        if (!_animating)
            return;
        
        _animating = false;
        Animator.SetTrigger(_stopAttentionTrigger);
    }

    public void OnFishCaughtPopupClosed()
    {
        bool previous = _madeFirstCombo;
        _madeFirstCombo = true;

        if (previous)
        {
            Animator.SetTrigger(_onTempAttentionTrigger);
        }
        else
        {
            if (_animating)
                return;
            
            _animating = true;
            Animator.SetTrigger(_onAttentionTrigger);
        }
    }

    public void OnCaughtTrash()
    {
        if (_madeFirstCombo)
            return;

        if (_animating)
            return;

        _animating = true;
        _madeFirstCombo = true;
        Animator.SetTrigger(_onAttentionTrigger);
    }
}