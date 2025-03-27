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

        public UnityEvent<Match> OnMatchDestroyed = new();
        public UnityEvent<int[]> OnRemovedFromColumns = new();

        private MatchMapper _matchMapper = new();

        private void Start()
        {
            MatchMediator.OnMatchFound.AddListener(OnMatchFound);
        }

        private void OnMatchFound(ICollection<FieldBlock> fieldBlocks)
        {
            Match fieldMatch = _matchMapper.MapFrom(fieldBlocks);
            int[] columns = CollectColumns(fieldBlocks);
            
            foreach (FieldBlock block in fieldBlocks)
                DestroyBlock(block);

            OnMatchDestroyed.Invoke(fieldMatch);
            OnRemovedFromColumns.Invoke(columns);
        }

        private void DestroyBlock(FieldBlock block)
        {
            BlockPool.Store(block);
        }

        private int[] CollectColumns(ICollection<FieldBlock> fieldBlocks)
        {
            HashSet<int> columns = new();
            foreach (FieldBlock block in fieldBlocks)
            {
                columns.Add(block.HorizontalPosition);
            }

            return columns.ToArray();
        }
    }