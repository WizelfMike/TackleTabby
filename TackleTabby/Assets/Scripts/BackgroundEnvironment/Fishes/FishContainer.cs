using System.Collections;
using UnityEngine;

public class FishContainer : MonoBehaviour
{
    [SerializeField]
    private float BoundaryBufferSize = 100f;
    
    [Header("Spawn Settings")]
    [SerializeField]
    [Range(0, 100)]
    private int FishCount;
    [SerializeField]
    [Range(0f, 2f)]
    private float SpawnDelay;
    [SerializeField]
    private EnvironmentFish EnvironmentFish;
    [SerializeField]
    private RuntimeAnimatorController[] FishAnimationControllers;
    
    public RectTransform RectTransform
    {
        get
        {
            if (_rectTransform)
                return _rectTransform;

            _rectTransform = GetComponent<RectTransform>();
            return _rectTransform;
        }
    }
    
    public float BoundaryBuffer => BoundaryBufferSize;
    
    private RectTransform _rectTransform;

    private IEnumerator Start()
    {
        for (int i = 0; i < FishCount; i++)
        {
            SpawnFish();
            yield return new WaitForSeconds(SpawnDelay);
        } 
    }

    private void SpawnFish()
    {
        EnvironmentFish fish = Instantiate(EnvironmentFish, transform);
        fish.RectTransform.anchoredPosition = SpawnPosition();
        fish.GetComponent<Animator>().runtimeAnimatorController =
            FishAnimationControllers[Random.Range(0, FishAnimationControllers.Length)];
        fish.SetDirection(SpawnDirection());
    }

    private Vector3 SpawnPosition()
    {
        float x = Random.Range(BoundaryBufferSize, RectTransform.rect.width - BoundaryBufferSize);
        float y = Random.Range(BoundaryBufferSize, RectTransform.rect.height - BoundaryBufferSize);
        
        return new Vector3(x, y, 0f);
    }

    private Vector3 SpawnDirection()
    {
        return Random.insideUnitCircle.normalized;
    }
}