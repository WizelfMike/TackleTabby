using UnityEngine;
using UnityEngine.Events;


public class BaitSwapper : MonoBehaviour
{
    [SerializeField] 
    private bool AllowNonMatchSwap;
    [SerializeField]
    private VectorMapper AllowedSwipeDirections;
    [SerializeField]
    [Range(1f, 10f)]
    private float SwappingSpeed = 1f;
    
    private bool _isSwapping;
    private bool _hasSwappedBack;
    private FieldBlock _swapBlockA;
    private FieldBlock _swapBlockB;
    private Vector3 _swapATargetPosition;
    private Vector3 _swapBTargetPosition;
    private Vector2 _direction;
    private float _swapProgress;


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
            _isSwapping = false;
            _swapProgress = 0;
            CheckSwapBack();
            return;
        }
        _swapProgress += deltaTime * SwappingSpeed;
    }

    private void CheckSwapBack()
    {
        if (_hasSwappedBack)
        {
            _hasSwappedBack = false;
            return;
        }
        
        bool checkBlockA = _swapBlockA.BlockUpdate(Directions.None);
        bool checkBlockB = _swapBlockB.BlockUpdate(Directions.None);
        
        if (checkBlockA || checkBlockB || AllowNonMatchSwap)
            return;

        _hasSwappedBack = true;
        MoveBaitPieces(_swapBlockA , -_direction);
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
        _direction = direction;
    }
}
