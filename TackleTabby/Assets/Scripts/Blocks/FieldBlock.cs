using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class FieldBlock : MonoBehaviour
{
    [SerializeField]
    private BaitDefinition BaitReference;
    [SerializeField]
    private SpriteRenderer SpriteRenderer;
    [SerializeField]
    private float RaycastDistance = 1f;

    private LayerMask _defaultLayerMask = -1;
    private BoxCollider2D _collider;
    
    public BaitDefinition BaitDefinitionReference
    {
        get => BaitReference;
        set
        {
            BaitReference = value;
            SpriteRenderer.sprite = BaitReference.BaitSprite;
        }
    }

    private void OnValidate()
    {
        SpriteRenderer.sprite = BaitReference.BaitSprite;
    }

    private void Start()
    {
        SpriteRenderer.sprite = BaitReference.BaitSprite;
        _defaultLayerMask = gameObject.layer;
        _collider = GetComponent<BoxCollider2D>();
    }

    public void BlockUpdate(Directions ignoreDirections = Directions.None)
    {
        List<FieldBlock> horizontalProgress = new();
        List<FieldBlock> verticalProgress = new();

        if (!ignoreDirections.HasFlag(Directions.Up))
            CheckUp(this, verticalProgress);
        
        if (!ignoreDirections.HasFlag(Directions.Right))
            CheckRight(this, horizontalProgress);
        
        if (!ignoreDirections.HasFlag(Directions.Down))
            CheckDown(this, verticalProgress);
        
        if (!ignoreDirections.HasFlag(Directions.Left))
            CheckLeft(this, horizontalProgress);

        bool validateHorizontal = FieldMatchValidator.Instance.ValidateMatch(this, horizontalProgress);
        bool validateVertical = FieldMatchValidator.Instance.ValidateMatch(this, verticalProgress);

        if (validateHorizontal && validateVertical)
        {
            MatchMediator.Instance.NotifyOfMatch(this, horizontalProgress.Concat(verticalProgress));
            return;
        }

        if (validateHorizontal)
        {
            MatchMediator.Instance.NotifyOfMatch(this, horizontalProgress);
            return;
        }

        if (validateVertical)
        {
            MatchMediator.Instance.NotifyOfMatch(this, verticalProgress);
        }
    }

    private void CheckUp(FieldBlock instigator, ICollection<FieldBlock> progress)
    {
        RaycastHit2D hit = PerformSafeCast(transform.position, Vector2.up, RaycastDistance);
        
        if (!hit)
            return;

        if (!hit.transform.TryGetComponent(out FieldBlock otherBlock))
            return;
        
        if (otherBlock.BaitReference != instigator.BaitReference)
            return;
        
        progress.Add(otherBlock);
        otherBlock.CheckUp(instigator, progress);
    }

    private void CheckRight(FieldBlock instigator, ICollection<FieldBlock> progress)
    {
        RaycastHit2D hit = PerformSafeCast(transform.position, Vector2.right, RaycastDistance);
        
        if (!hit)
            return;

        if (!hit.transform.TryGetComponent(out FieldBlock otherBlock))
            return;
        
        if (otherBlock.BaitReference != instigator.BaitReference)
            return;
        
        progress.Add(otherBlock);
        otherBlock.CheckRight(instigator, progress);
    }

    private void CheckDown(FieldBlock instigator, ICollection<FieldBlock> progress)
    {
         RaycastHit2D hit = PerformSafeCast(transform.position, Vector2.down, RaycastDistance);
         
         if (!hit)
             return;
 
         if (!hit.transform.TryGetComponent(out FieldBlock otherBlock))
             return;
         
         if (otherBlock.BaitReference != instigator.BaitReference)
             return;
         
         progress.Add(otherBlock);
         otherBlock.CheckDown(instigator, progress);
    }

    private void CheckLeft(FieldBlock instigator, ICollection<FieldBlock> progress)
    {
        RaycastHit2D hit = PerformSafeCast(transform.position, Vector2.left, RaycastDistance);
        
        if (!hit)
            return;
 
        if (!hit.transform.TryGetComponent(out FieldBlock otherBlock))
            return;
        
        if (otherBlock.BaitReference != instigator.BaitReference)
            return;
         
        progress.Add(otherBlock);
        otherBlock.CheckLeft(instigator, progress);
    }

    private RaycastHit2D PerformSafeCast(Vector2 origin, Vector2 direction, float distance)
    {
        Debug.DrawRay(origin, direction * RaycastDistance, Color.red, 1f);
            
        _collider.enabled = false;
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance);
        _collider.enabled = true;
        return hit;
    }
        
    
    #if UNITY_EDITOR

    [ContextMenu("BlockUpdate/Update Horizontal")]
    private void BlockUpdateHorizontal()
    {
        // Will call the block-update, and ignores vertical directions
        BlockUpdate(Directions.Vertical);
    }
    
    [ContextMenu("BlockUpdate/Update Vertical")]
    private void BlockUpdateVertical()
    {
        // Will call the block-update, and ignores horizontal directions
        BlockUpdate(Directions.Horizontal);
    }

    [ContextMenu("BlockUpdate/All Directions")]
    private void BlockUpdateAllDirections()
    {
        // Will call the block-update, and ignores no directions; calling all
        BlockUpdate(Directions.None);
    }
    
    #endif
}
