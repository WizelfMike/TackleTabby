using System.Collections;
using System.Collections.Generic;
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

    private void OnRemovedFromColumns(Dictionary<int, int> columnIndices)
    {
        StartCoroutine(RemovedFromColumnsCoroutine(columnIndices));
        // SpawnNewBlocks(columnIndices);
    }

    private IEnumerator RemovedFromColumnsCoroutine(Dictionary<int, int> columnIndices)
    {
        SpawnNewBlocks(columnIndices);
        yield return new WaitForSeconds(0.5f);
        InstigateGravity(columnIndices);
    }

    private void SpawnNewBlocks(Dictionary<int, int> columnIndices)
    {
        foreach (int key in columnIndices.Keys)
        {
            int spawnCount =  columnIndices[key];
            for (int j = 0; j < spawnCount; j++)
            {
                FieldBlock newBlock = BlockPool.Retrieve();
                newBlock.transform.SetParent(PlayField.transform);
                newBlock.transform.localPosition = PlayField.GetLocalisedCoordinateUnclamped(key, PlayField.VerticalCount + j);
                newBlock.BaitDefinitionReference = Baits[Random.Range(0, Baits.Length)];
            }
        }
    }

    private void InstigateGravity(Dictionary<int, int> columnIndices)
    {
         Vector3 playFieldWorldLocation = PlayField.transform.position;
         Vector3 playFieldScale = PlayField.transform.localScale;
         foreach (int key in columnIndices.Keys)
         {
             Vector2 localisedGridCoords = PlayField.GetLocalisedCoordinate(key, 0) * playFieldScale;
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