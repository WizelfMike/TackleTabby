using System;
using UnityEngine;

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
    private SpriteRenderer AlphaMaskSpriteRenderer;

    private void OnDrawGizmos()
    {
        float width = HorizontalGridCount * GridItemUnitSize;
        float height = VerticalGridCount * GridItemUnitSize;

        Vector3 origin = transform.position + (Vector3.left * GridItemUnitSize / 2 + Vector3.down * GridItemUnitSize / 2);
        Vector3 bottomRight = origin + Vector3.right * width;
        Vector3 topRight = bottomRight + Vector3.up * height;
        Vector3 topLeft = topRight + Vector3.left * width;

        Gizmos.color = Color.green;
        
        Gizmos.DrawLine(origin, bottomRight);
        Gizmos.DrawLine(bottomRight, topRight);
        Gizmos.DrawLine(topRight, topLeft);
        Gizmos.DrawLine(topLeft, origin);
        
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(GetWorldWeightPoint(), 0.2f);
    }

    private void OnValidate()
    {
        Transform spriteTransform = AlphaMaskSpriteRenderer.transform;
        spriteTransform.localScale =
            new Vector3(HorizontalGridCount * GridItemUnitSize, VerticalGridCount * GridItemUnitSize, 1);
        Vector3 weightCentre = GetWorldWeightPoint();
        spriteTransform.position = new Vector3(weightCentre.x, weightCentre.y, weightCentre.z - 2);
    }

    public Vector2 GetLocalisedCoordinate(int horizontalGridIndex, int verticalGridIndex)
    {
        if (horizontalGridIndex >= HorizontalGridCount || verticalGridIndex >= VerticalGridCount || horizontalGridIndex < 0 || verticalGridIndex < 0)
            Debug.LogErrorFormat("The given indices were not valid: ({0}, {1})", horizontalGridIndex, verticalGridIndex);
            
        return new Vector2(horizontalGridIndex * GridItemUnitSize, verticalGridIndex * GridItemUnitSize);
    }

    public Vector2 GetPreciseGridLocation(Vector2 localLocation)
    {
        Vector2 scaledApproximates = localLocation / GridItemUnitSize;
        return GetLocalisedCoordinate((int)Mathf.Floor(scaledApproximates.x), (int)Mathf.Floor(scaledApproximates.y));
    }

    public Vector3 GetWorldWeightPoint()
    {
        return transform.position + new Vector3((HorizontalGridCount-1) / 2f * GridItemUnitSize,
            (VerticalGridCount-1) / 2f * GridItemUnitSize);
    }
}