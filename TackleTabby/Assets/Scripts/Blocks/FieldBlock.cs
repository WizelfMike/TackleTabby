using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(GravityManager))]
public class FieldBlock : MonoBehaviour
{
    [SerializeField]
    private BaitDefinition BaitReference;
    [SerializeField]
    private SpriteRenderer SpriteRenderer;
    [SerializeField]
    private float RaycastDistance = 1f;
    
    public BaitDefinition BaitDefinitionReference
    {
        get => BaitReference;
        set
        {
            BaitReference = value;
            SpriteRenderer.sprite = BaitReference.BaitSprite;
        }
    }

    public GridPlayField ParentField => _parentField;
    public int HorizontalPosition => (int)_parentField.GetGridCoordinates(transform.localPosition).x;
    public int VerticalPosition => (int) _parentField.GetGridCoordinates(transform.localPosition).y;

    private BoxCollider2D _collider;
    private GravityManager _gravityManager;
    private GridPlayField _parentField;

    private void OnValidate()
    {
        SpriteRenderer.sprite = BaitReference.BaitSprite;
    }

    private void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
        _gravityManager = GetComponent<GravityManager>();
        _parentField = transform.parent.GetComponent<GridPlayField>();
        
        SpriteRenderer.sprite = BaitReference.BaitSprite;
        transform.localPosition = _parentField.GetPreciseGridLocation(transform.localPosition);
    }

    public bool BlockUpdate(Directions ignoreDirections = Directions.None)
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
            return true;
        }

        if (validateHorizontal)
        {
            MatchMediator.Instance.NotifyOfMatch(this, horizontalProgress);
            return true;
        }

        if (validateVertical)
        {
            MatchMediator.Instance.NotifyOfMatch(this, verticalProgress);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Function that will get used to notify this block that it should start falling
    /// </summary>
    public void NotifyOfGravity()
    {
        Vector2 gridCoords = _parentField.GetGridCoordinates(transform.localPosition);
        if (gridCoords.y == 0)
            return;

        RaycastHit2D below = PerformSafeCast(transform.position, Vector2.down, transform.localScale.y/2 + 0.1f);
        if (below)
            return;
        
        _gravityManager.OnLanded.AddListener(OnLanded);
        _gravityManager.StartFalling();
    }

    private void OnLanded(GravityManager gravityManager)
    {
        gravityManager.OnLanded.RemoveListener(OnLanded);
        transform.localPosition = _parentField.GetPreciseGridLocation(transform.localPosition);
        BlockUpdate(Directions.Up);
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
        Debug.DrawRay(origin, direction * RaycastDistance, Color.red, 3f);

        bool wasActive = _collider.enabled;
        if (wasActive)
            _collider.enabled = false;
        
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance);
        
        if (wasActive)
            _collider.enabled = true;
        
        return hit;
    }

    public FieldBlock FindNeighbourInDirection(Vector2 direction)
    {
        RaycastHit2D cast = PerformSafeCast(transform.position, direction, RaycastDistance);
        if (!cast)
            return null;

        return cast.transform.GetComponent<FieldBlock>();
    }
        
    
    #if UNITY_EDITOR

    [ContextMenu("BlockUpdate/Update Horizontal")]
    private void BlockUpdateHorizontal()
    {
        BlockUpdate(Directions.Vertical);
    }
    
    [ContextMenu("BlockUpdate/Update Vertical")]
    private void BlockUpdateVertical()
    {
        BlockUpdate(Directions.Horizontal);
    }

    [ContextMenu("BlockUpdate/All Directions")]
    private void BlockUpdateAllDirections()
    {
        BlockUpdate(Directions.None);
    }

    [ContextMenu("Gravity/StartGravity")]
    private void StartGravity()
    {
        NotifyOfGravity();
    }
    
    #endif
}
