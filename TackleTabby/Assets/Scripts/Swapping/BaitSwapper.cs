
using UnityEngine;
using UnityEngine.Events;


public class BaitSwapper : MonoBehaviour
{
    [SerializeField]
    private VectorMapper AllowedSwipeDirections;
    [SerializeField]
    [Range(1f, 10f)]
    private float SwappingSpeed = 1f;
    
    public UnityEvent<FieldBlock, FieldBlock> OnBlocksSwapped = new();
    
    private bool _isSwapping = false;
    private FieldBlock _swapBlockA;
    private FieldBlock _swapBlockB;
    private Vector3 _swapATargetPosition;
    private Vector3 _swapBTargetPosition;
    private float _swapProgress = 0;
    
    private void Update()
    {
        if (_isSwapping)
            SwapBlocks(Time.deltaTime);
    }
    
    private void SwapBlocks(float deltaTime)
    {
        _swapBlockA.transform.position =
            Vector3.Lerp(_swapBlockA.transform.position, _swapATargetPosition, _swapProgress);
        _swapBlockB.transform.position =
            Vector3.Lerp(_swapBlockB.transform.position, _swapBTargetPosition, _swapProgress);

        if (_swapProgress >= 1f)
        {
            _swapProgress = 0;
            _isSwapping = false;
            return;
        }
        _swapProgress += deltaTime * SwappingSpeed;
    }

    public void MoveBaitPieces(FieldBlock targetBlock, Vector2 direction)
    {
        if (_isSwapping)
            return;
        
        direction.Normalize();
        FieldBlock neighbour = targetBlock.FindNeighbourInDirection(AllowedSwipeDirections.MapInput(direction));
        if (neighbour == null)
            return;
        
        _swapBlockA = targetBlock;
        _swapATargetPosition = neighbour.transform.position;
        _swapBlockB = neighbour;
        _swapBTargetPosition = targetBlock.transform.position;
        _isSwapping = true;
    }
}
