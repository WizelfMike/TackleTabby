using System;
using UnityEngine;

public class MovingWater : MonoBehaviour
{
    [SerializeField]
    private RectTransform[] MovingPanels;
    [SerializeField]
    private float MovingSpeed = 25f;
    [SerializeField]
    private NonFlaggedDirections MovingDirection = NonFlaggedDirections.Right;
    
    private RectTransform RectTransform => (RectTransform)transform;
    private float Width => RectTransform.rect.width;
    private float Height => RectTransform.rect.height;

    private float _progression = 0;

    private void OnValidate()
    {
        UpdatePosition(0);
        SetPanelWidth();
    }

    private void Update()
    {
        _progression = (_progression + CalculateAbsoluteProgressionDelta(MovingSpeed) * Time.deltaTime) % 1;
        UpdatePosition(_progression * Width);
        SetPanelWidth();
    }

    private float CalculateAbsoluteProgressionDelta(float absoluteSpeed)
    {
        return absoluteSpeed / Width; 
    }

    private void SetPanelWidth()
    {
        int panelCount = MovingPanels.Length;
        for (int i = 0; i < panelCount; i++)
        {
            RectTransform rect = MovingPanels[i].transform as RectTransform;
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Width);
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Height);
        }
    }

    private void UpdatePosition(float offset)
    {
        switch (MovingDirection)
        {
            case NonFlaggedDirections.Right:
                UpdateToRight(offset);
                break;
    
            case NonFlaggedDirections.Left:
                UpdateToLeft(offset);
                break;
            
            case NonFlaggedDirections.None:
                Debug.LogWarning("A direction of `NONE` makes the panels not move no matter the speed set");
                break;
            
            default:
                throw new NotImplementedException($"Not implement for {MovingDirection}");
        }
    }

    private void UpdateToRight(float offset)
    {
        int panelCount = MovingPanels.Length;
        for (int i = panelCount - 1; i >= 0; i--)
        {
            RectTransform rect = MovingPanels[i];
            Vector2 preCalculatedNext = rect.anchoredPosition;
            preCalculatedNext.x = i == panelCount - 1 ?
                    offset :
                    MovingPanels[i + 1].anchoredPosition.x -
                    MovingPanels[i + 1].rect.width;
            rect.anchoredPosition = preCalculatedNext;
        }
    }

    private void UpdateToLeft(float offset)
    {
        int panelCount = MovingPanels.Length;
        for (int i = 0; i < panelCount; i++)
        {
            RectTransform rect = MovingPanels[i];
            Vector2 preCalculatedNext = rect.anchoredPosition;
            preCalculatedNext.x = i == 0 ?
                -offset :
                MovingPanels[i - 1].anchoredPosition.x +
                MovingPanels[i - 1].rect.width;
            rect.anchoredPosition = preCalculatedNext;
        }
    }
}