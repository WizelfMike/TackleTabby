using System.Collections;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridBaitSpawner : MonoBehaviour
{
    [SerializeField]
    private GridPlayField PlayField;
    [SerializeField]
    private MatchRemover MatchRemover;
    [SerializeField]
    private FieldBlockPool BlockPool;
    [SerializeField]
    private LayerMask FieldBlockMask;
    [SerializeField]
    private BaitDefinition[] Baits;
    [SerializeField]
    private float GravityDelaySeconds = 0.1f;
    private void Start()
    {
        MatchRemover.OnRemovedFromColumns.AddListener(OnRemovedFromColumns);
    }

    private void OnRemovedFromColumns(int[] columnIndices)
    {
        StartCoroutine(RemovedFromColumnsCoroutine(columnIndices));
    }

    private IEnumerator RemovedFromColumnsCoroutine(int[] columnIndices)
    {
        SpawnNewBlocks(columnIndices);
        yield return new WaitForSeconds(0.2f);
        InstigateGravity(columnIndices);
    }

    private void SpawnNewBlocks(int[] columnIndices)
    {
        Vector3 playFieldWorldLocation = PlayField.transform.position;
        Vector3 playFieldScale = PlayField.transform.localScale;
        int columnCount = columnIndices.Length;
        for (int i = 0; i < columnCount; i++)
        {
            int columnIndex = columnIndices[i];
            Vector2 localisedGridCoords = PlayField.GetLocalisedCoordinate(columnIndex, 0) * playFieldScale;
            Vector2 rayCastOrigin = new Vector2(playFieldWorldLocation.x + localisedGridCoords.x,
                playFieldWorldLocation.y + localisedGridCoords.y);           
            
            RaycastHit2D[] hits = Physics2D.RaycastAll(rayCastOrigin, Vector2.up, Mathf.Infinity, FieldBlockMask);
            int spawnCount = PlayField.VerticalCount - hits.Length;
            for (int j = 0; j < spawnCount; j++)
            {
                FieldBlock newBlock = BlockPool.Retrieve();
                newBlock.transform.SetParent(PlayField.transform);
                newBlock.transform.localPosition = PlayField.GetLocalisedCoordinateUnclamped(columnIndex, PlayField.VerticalCount + j);
                newBlock.BaitDefinitionReference = Baits[Random.Range(0, Baits.Length)];
            }
        }
    }

    private void InstigateGravity(int[] columnIndices)
    {
         Vector3 playFieldWorldLocation = PlayField.transform.position;
         Vector3 playFieldScale = PlayField.transform.localScale;
         int columnCount = columnIndices.Length;
         for (int i = 0; i < columnCount; i++)
         {
             int columnIndex = columnIndices[i];
             Vector2 localisedGridCoords = PlayField.GetLocalisedCoordinate(columnIndex, 0) * playFieldScale;
             Vector2 rayCastOrigin = new Vector2(playFieldWorldLocation.x + localisedGridCoords.x,
                 playFieldWorldLocation.y + localisedGridCoords.y);
 
             RaycastHit2D[] hits = Physics2D.RaycastAll(rayCastOrigin, Vector2.up, Mathf.Infinity, FieldBlockMask);
             FieldBlock[] columnBlocks = hits.Select(x => x.transform.GetComponent<FieldBlock>()).ToArray();
             StartCoroutine(ApplyColumnGravity(columnBlocks));
         }
    }

    private IEnumerator ApplyColumnGravity(FieldBlock[] columnBlocks)
    { 
        int blockCount = columnBlocks.Length;
        for (int j = 0; j < blockCount; j++)
        {
            columnBlocks[j].NotifyOfGravity();
            yield return new WaitForSeconds(GravityDelaySeconds);
        }
    }
}