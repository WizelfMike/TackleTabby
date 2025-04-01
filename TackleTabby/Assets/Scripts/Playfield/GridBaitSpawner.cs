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
    [SerializeField]
    private float SpawnGravityDelay = 0.1f;

    private readonly Queue<Dictionary<int, int>> _spawnQueue = new();
    private readonly List<FieldBlock> _spawnBatch = new();
    
    private int _landingLeftCount = 0;
    
    private void Start()
    {
        MatchRemover.OnRemovedFromColumns.AddListener(OnRemovedFromColumns);
    }

    private void OnRemovedFromColumns(Dictionary<int, int> columnIndices)
    {
        TrySpawn(columnIndices);
    }

    private IEnumerator SpawnColumnsCoroutine(Dictionary<int, int> columnIndices)
    {
        SpawnNewBlocks(columnIndices);
        yield return new WaitForSeconds(SpawnGravityDelay);
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
                _spawnBatch.Add(newBlock);
                BindToLanding(newBlock.GetComponent<GravityManager>());

                newBlock.GetComponent<Collider2D>().enabled = false;
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
             FieldBlock[] columnBlocks = FindFloating(playFieldWorldLocation, playFieldScale, key).Concat(_spawnBatch).ToArray();
             StartCoroutine(ApplyColumnGravity(columnBlocks));
         }
         _spawnBatch.Clear();
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

    private void TrySpawn(Dictionary<int, int> columnIndices)
    {
        foreach (int key in columnIndices.Keys)
            LockFloatingInColumn(key);
        
        _spawnQueue.Enqueue(columnIndices);
        if (_landingLeftCount > 0)
            return;
        
        CommenceSpawning();
    }

    private void LockFloatingInColumn(int columnIndex)
    { 
        Vector3 playFieldWorldLocation = PlayField.transform.position;
        Vector3 playFieldScale = PlayField.transform.localScale;
        FieldBlock[] blocks = FindFloating(playFieldWorldLocation, playFieldScale, columnIndex);
        int blockCount = blocks.Length;
        for (int i = 0; i < blockCount; i++)
            blocks[i].NotifyInFallingPosition();
    }

    private void CommenceSpawning()
    {
        if (_spawnQueue.Count <= 0)
            return;
        
        Dictionary<int, int> indices = _spawnQueue.Dequeue();
        StartCoroutine(SpawnColumnsCoroutine(indices));
    }

    private FieldBlock[] FindFloating(Vector3 playFieldWorldLocation, Vector3 playFieldScale, int columnIndex)
    { 
        Vector2 localisedGridCoords = PlayField.GetLocalisedCoordinate(columnIndex, 0) * playFieldScale;
        Vector2 rayCastOrigin = new Vector2(playFieldWorldLocation.x + localisedGridCoords.x, 
            playFieldWorldLocation.y + localisedGridCoords.y);

        bool foundLowest = false;
 
        return Physics2D.RaycastAll(rayCastOrigin, Vector2.up, Mathf.Infinity, FieldBlockMask)
            .Select(x => x.transform.GetComponent<FieldBlock>())
            .Where(x =>
            {
                if (x.VerticalPosition <= 0)
                    return false;

                if (foundLowest)
                    return true;

                if (x.FindNeighbourInDirection(Vector2.down) == null)
                {
                    foundLowest = true;
                    return true;
                }

                return false;
            }).ToArray();
    }

    private void BindToLanding(GravityManager gravityManager)
    {
        _landingLeftCount++;
        gravityManager.OnLanded.AddListener(OnBlockLanded);
    }

    private void OnBlockLanded(GravityManager gravityManager)
    {
        gravityManager.OnLanded.RemoveListener(OnBlockLanded);
        _landingLeftCount--;
        if (_landingLeftCount <= 0)
            AllLanded();
    }

    private void AllLanded()
    {
        CommenceSpawning();
    }
}