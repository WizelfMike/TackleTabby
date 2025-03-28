using UnityEngine;
using UnityEngine.InputSystem;

public class ValidatePress : MonoBehaviour
{
    [SerializeField]
    private float MinSwipeLength = 10f;
    [SerializeField]
    private BaitSwapper BaitSwapper;
    [SerializeField]
    private LayerMask BaitLayerMask;
    [SerializeField]
    private Camera GameCamera;
    
    private bool ValidateSwipe(Vector2 direction)
    {
        return direction.magnitude >= MinSwipeLength;
    }

    public void OnSwipeEnded(Vector2 start, Vector2 direction)
    {
        if (!ValidateSwipe(direction))
            return;
        
        Vector2 worldPosition = GameCamera.ScreenToWorldPoint(start);

        RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero.normalized * 0, Mathf.Infinity, BaitLayerMask);
        if (!hit)
            return;
        
        FieldBlock fieldBlock = hit.transform.gameObject.GetComponent<FieldBlock>();
        BaitSwapper.MoveBaitPieces(fieldBlock, direction.normalized);
    }
    
    
}
