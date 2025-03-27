using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridPlayField : GenericSingleton<GridPlayField>
{
    [Header("Size configuration")]
    [SerializeField]
    private int HorizontalGridCount;
    [SerializeField]
    private int VerticalGridCount;
    [SerializeField]
    private float GridItemUnitSize = 1f;

    [Header("Alpha mask")]
    [SerializeField]
    private SpriteMask AlphaMaskSpriteRenderer;
    [SerializeField]
    private float MaskDistance = 1f;
    
    #if UNITY_EDITOR

    [Header("Filling")]
    [SerializeField]
    private FieldBlock FieldBlockPrefab;
    [SerializeField]
    private BaitDefinition[] RandomBaits;
    
    #endif
    
    private void OnValidate()
    {
        PositionAlphaMask();
    }

    private void OnDrawGizmos()
    {
        Transform selfTransform = transform;
        float width = HorizontalGridCount * GridItemUnitSize;
        float height = VerticalGridCount * GridItemUnitSize;

        Vector3 origin = selfTransform.position + (Vector3.left * (GridItemUnitSize / 2 * selfTransform.localScale.x) + Vector3.down * (GridItemUnitSize / 2 * selfTransform.localScale.y));
        Vector3 bottomRight = origin + Vector3.right * width * selfTransform.localScale.x;
        Vector3 topRight = bottomRight + Vector3.up * height * selfTransform.localScale.y;
        Vector3 topLeft = topRight + Vector3.left * width * selfTransform.localScale.x;

        Gizmos.color = Color.green;
        
        Gizmos.DrawLine(origin, bottomRight);
        Gizmos.DrawLine(bottomRight, topRight);
        Gizmos.DrawLine(topRight, topLeft);
        Gizmos.DrawLine(topLeft, origin);
        
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(GetWorldWeightPoint(), 0.2f);
    }

    private void Start()
    {
        PositionAlphaMask();
    }

    private void PositionAlphaMask()
    {
        Transform maskTransform = AlphaMaskSpriteRenderer.transform;
        maskTransform.localScale =
            new Vector3(HorizontalGridCount * GridItemUnitSize, VerticalGridCount * GridItemUnitSize, 1);
        Vector3 weightCentre = GetWorldWeightPoint();
        maskTransform.position =
            new Vector3(weightCentre.x, weightCentre.y, weightCentre.z) + transform.forward * MaskDistance;
    }
    
    public Vector2 GetLocalisedCoordinate(int horizontalGridIndex, int verticalGridIndex)
    {
        if (horizontalGridIndex >= HorizontalGridCount || verticalGridIndex >= VerticalGridCount || horizontalGridIndex < 0 || verticalGridIndex < 0)
            Debug.LogErrorFormat("The given indices were not valid: ({0}, {1})", horizontalGridIndex, verticalGridIndex);
            
        return new Vector2(horizontalGridIndex * GridItemUnitSize, verticalGridIndex * GridItemUnitSize);
    }

    public Vector2 GetGridCoordinates(Vector2 localLocation)
    {
        Vector2 scaledApproximates = localLocation / GridItemUnitSize;
        return new Vector2(Mathf.Floor(localLocation.x), Mathf.Floor(localLocation.y));
    }

    public Vector2 GetPreciseGridLocation(Vector2 localLocation)
    {
        Vector2 gridCoordinates = GetGridCoordinates(localLocation);
        return GetLocalisedCoordinate((int)gridCoordinates.x, (int)gridCoordinates.y);
    }

    public Vector3 GetWorldWeightPoint()
    {
        Transform selfTransform = transform;
        return selfTransform.position + new Vector3((HorizontalGridCount-1) / 2f * GridItemUnitSize * selfTransform.localScale.x,
            (VerticalGridCount-1) / 2f * GridItemUnitSize * selfTransform.localScale.y);
    }
    
    #if UNITY_EDITOR

    [ContextMenu("Filling/Fill Field")]
    private void FillField()
    {
        for (int x = 0; x < HorizontalGridCount; x++)
        {
            for (int y = 0; y < VerticalGridCount; y++)
            {
                FieldBlock fieldBlockInstance = Instantiate(FieldBlockPrefab, transform);
                if (RandomBaits.Length > 0)
                    fieldBlockInstance.BaitDefinitionReference = RandomBaits[Random.Range(0, RandomBaits.Length)];
                fieldBlockInstance.name = FieldBlockPrefab.name + $" ({x}, {y})";
                fieldBlockInstance.transform.localPosition = GetLocalisedCoordinate(x, y);
            }
        }
    }
    
    #endif
}