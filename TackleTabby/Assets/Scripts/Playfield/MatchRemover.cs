using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class MatchRemover : MonoBehaviour
{
    [SerializeField]
    private MatchMediator MatchMediator;
    [SerializeField]
    private FieldBlockPool BlockPool;

    [SerializeField]
    [Range(0f, 10f)]
    private float RemovalDelaySeconds = 1f;

#if UNITY_EDITOR

    [SerializeField]
    private BaitDefinition SimulationBait;

#endif 

    public UnityEvent<Match> OnMatchDestroyed = new();
    public UnityEvent<Dictionary<int, int>> OnRemovedFromColumns = new();

    private MatchMapper _matchMapper = new();

    private void Start()
    {
        MatchMediator.OnMatchFound.AddListener(OnMatchFound);
    }

    private void OnMatchFound(ICollection<FieldBlock> fieldBlocks)
    {
        StartCoroutine(OnMatchFoundCoroutine(fieldBlocks));
    }

    private IEnumerator OnMatchFoundCoroutine(ICollection<FieldBlock> fieldBlocks)
    {
        Match fieldMatch = _matchMapper.MapFrom(fieldBlocks);
        Dictionary<int, int> columns = CollectColumns(fieldBlocks);

        foreach (FieldBlock block in fieldBlocks)
        {
            block.GetComponent<Collider2D>().enabled = false;
            block.PlayMatchAnimation();
        }

        yield return new WaitForSeconds(RemovalDelaySeconds);
            
        foreach (FieldBlock block in fieldBlocks)
            DestroyBlock(block);

        OnMatchDestroyed.Invoke(fieldMatch);
        OnRemovedFromColumns.Invoke(columns);
    }

    private void DestroyBlock(FieldBlock block)
    {
        BlockPool.Store(block);
        block.ResetAnimator();
    }

    private Dictionary<int, int> CollectColumns(ICollection<FieldBlock> fieldBlocks)
    {
        Dictionary<int, int> columns = new();
        foreach (FieldBlock block in fieldBlocks)
        {
            if (!columns.TryAdd(block.HorizontalPosition, 1))
                columns[block.HorizontalPosition]++;
        }

        return columns;
    }
    

#if UNITY_EDITOR

    [ContextMenu("Simulate/Bait")]
    private void SimulateBait()
    {
        Match match = new Match { BaitType = SimulationBait, MatchSize = 3 };
        OnMatchDestroyed.Invoke(match);
    }

#endif
}