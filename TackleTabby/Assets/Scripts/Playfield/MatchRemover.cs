    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using Unity.Collections;
    using UnityEngine;
    using UnityEngine.Events;

    public class MatchRemover : MonoBehaviour
    {
        [SerializeField]
        private MatchMediator MatchMediator;
        [SerializeField]
        private FieldBlockPool BlockPool;

        public UnityEvent<Match> OnMatchDestroyed = new();
        public UnityEvent<Dictionary<int, int>> OnRemovedFromColumns = new();

        private MatchMapper _matchMapper = new();

        private void Start()
        {
            MatchMediator.OnMatchFound.AddListener(OnMatchFound);
        }

        private void OnMatchFound(ICollection<FieldBlock> fieldBlocks)
        {
            Match fieldMatch = _matchMapper.MapFrom(fieldBlocks);
            Dictionary<int, int> columns = CollectColumns(fieldBlocks);
            
            foreach (FieldBlock block in fieldBlocks)
                DestroyBlock(block);

            OnMatchDestroyed.Invoke(fieldMatch);
            OnRemovedFromColumns.Invoke(columns);
        }

        private void DestroyBlock(FieldBlock block)
        {
            BlockPool.Store(block);
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
    }