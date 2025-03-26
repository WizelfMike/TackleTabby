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

        private MatchMapper _matchMapper = new();

        private void Start()
        {
            MatchMediator.OnMatchFound.AddListener(OnMatchFound);
        }

        private void OnMatchFound(ICollection<FieldBlock> fieldBlocks)
        {
            Match fieldMatch = _matchMapper.MapFrom(fieldBlocks);
            var upperBlocks = GetUpper(fieldBlocks);
            
            foreach (FieldBlock block in fieldBlocks)
                DestroyBlock(block);

            OnMatchDestroyed.Invoke(fieldMatch);
            
            foreach (FieldBlock upperBlock in upperBlocks)
            {
                Debug.Log(upperBlock.name);
                upperBlock.OnGravityNotified();
            }
        }

        private void DestroyBlock(FieldBlock block)
        {
            BlockPool.Store(block);
            
        }

        private FieldBlock[] GetUpper(ICollection<FieldBlock> fieldBlocks)
        {
            return fieldBlocks.Where(block =>
            {
                RaycastHit2D above = block.PerformSafeCast(block.transform.position, Vector2.up, 1f);
                if (!above)
                    return false;

                if (!above.transform.TryGetComponent(out FieldBlock upperBlock))
                    return false;

                return !fieldBlocks.Contains(upperBlock);
            })
            .Select(block =>
            {
                RaycastHit2D above = block.PerformSafeCast(block.transform.position, Vector2.up, 1f);
                if (!above)
                    return null;
            
                if (!above.transform.TryGetComponent(out FieldBlock upperBlock))
                    return null;
            
                return upperBlock;
            }).Where(x => x != null).ToArray();
        }
    }