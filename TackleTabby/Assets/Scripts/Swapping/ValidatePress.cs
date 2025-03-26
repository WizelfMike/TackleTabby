using UnityEditor.Experimental.GraphView;
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
    
    //FilledPiece
    private bool ValidateSwipe(Vector2 start, Vector2 end)
    {
        return Vector2.Distance(start, end) >= MinSwipeLength;
    }

    public void OnSwipeEnded(Vector2 start, Vector2 end)
    {
        if (!ValidateSwipe(start, end))
            return;
        
        Vector2 worldPosition = GameCamera.ScreenToWorldPoint(start);

        RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero.normalized * 0, Mathf.Infinity, BaitLayerMask);
        if (!hit)
            return;

        Debug.Log(hit.transform.gameObject);
        
        Vector2 direction = (end - start).normalized;
        FieldBlock fieldBlock = hit.transform.gameObject.GetComponent<FieldBlock>();
        BaitSwapper.MoveBaitPieces(fieldBlock, direction);
    }
    
    
}
