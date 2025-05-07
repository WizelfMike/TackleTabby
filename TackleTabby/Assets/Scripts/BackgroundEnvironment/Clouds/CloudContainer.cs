using System.Collections;
using System.Threading;
using UnityEngine;

public class CloudContainer : MonoBehaviour
{
    [Header("Spawning")]
    [SerializeField]
    private CloudPool CloudObjectPool;
    [SerializeField]
    [Range(0f, 15f)]
    private float SpawnTimeoutSeconds;
    [SerializeField]
    [Range(0f, 100f)]
    private float SpawnChance;
    [SerializeField]
    [Range(1f, 120f)]
    private float CloudLifeTimeSeconds;
    [SerializeField]
    [Range(0f, 100f)]
    private float SpawnXBuffer = 10f;

    [Header("Per cloud settings")]
    [SerializeField]
    private Vector2 CloudSpeedRange;
    [SerializeField]
    private Vector2 CloudScaleRange;
    [SerializeField]
    private Sprite[] CloudSprites;

    private CancellationTokenSource _cancellationTokenSource;
    private RectTransform _rectTransform;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _cancellationTokenSource = new CancellationTokenSource();
        StartCoroutine(SpawnCloudCoroutine(_cancellationTokenSource.Token));
    }

    private void OnDestroy()
    {
        _cancellationTokenSource.Cancel();
    }

    private IEnumerator SpawnCloudCoroutine(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            if (cancellationToken.IsCancellationRequested)
                break;
            
            SpawnCloud();
            yield return new WaitForSeconds(SpawnTimeoutSeconds);
        }
    }

    private void SpawnCloud()
    {
        if (!TestChance(SpawnChance))
            return;
        
        MovingCloud cloudObject = CloudObjectPool.Retrieve();
        RectTransform cloudTransform = cloudObject.GetComponent<RectTransform>();
        cloudObject.ObjectPool = CloudObjectPool;
        cloudObject.Lifetime = CloudLifeTimeSeconds;
        
        cloudTransform.SetParent(transform);
        cloudObject.Speed = GetRandomSpeed();
        cloudTransform.localScale = Vector3.one * GetRandomScale();
        cloudTransform.anchoredPosition =
            new Vector2(
                GetStartX(cloudObject.transform.localScale.x * cloudTransform.rect.width),
                GetRandomY());
        cloudObject.Sprite = GetRandomSprite();
    }

    private bool TestChance(float chance)
    {
        float value = Random.value * 100f;
        return value <= chance;
    }

    private float GetRandomSpeed()
    {
        return Random.Range(CloudSpeedRange.x, CloudSpeedRange.y);
    }

    private Sprite GetRandomSprite()
    {
        return CloudSprites[Random.Range(0, CloudSprites.Length)];
    }

    private float GetRandomScale()
    {
        return Random.Range(CloudScaleRange.x, CloudScaleRange.y);
    }

    private float GetStartX(float xWidth)
    {
        return -(xWidth/2f) - SpawnXBuffer;
    }

    private float GetRandomY()
    {
        return Random.Range(_rectTransform.rect.yMin, _rectTransform.rect.yMax);
    }
    
#if UNITY_EDITOR
    
    [ContextMenu("Spawning/Stop")]
    private void StopSpawning()
    {
        _cancellationTokenSource.Cancel();
    }

    [ContextMenu("Spawning/Start")]
    private void StartSpawning()
    {
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource = new CancellationTokenSource();
        StartCoroutine(SpawnCloudCoroutine(_cancellationTokenSource.Token));
    }
    
#endif // UNITY_EDITOR
}