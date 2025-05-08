using System;
using System.Collections;
using System.Threading;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
struct CloudSetting
{
    [SerializeField]
    public Sprite CloudSprite;
    [SerializeField]
    public Vector2 CloudSpeedRange;
    [SerializeField]
    public Vector2 CloudScaleRange;
}

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
    [Range(0f, 100f)]
    private float SpawnXBuffer = 10f;

    [Header("Per cloud settings")]
    [SerializeField]
    private CloudSetting[] CloudSettings;

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
        int settingIndex = Random.Range(0, CloudSettings.Length);
        cloudObject.ObjectPool = CloudObjectPool;
        cloudObject.ParentRect = _rectTransform;
        
        cloudTransform.SetParent(transform);
        cloudObject.Speed = GetRandomSpeed(settingIndex);
        cloudTransform.localScale = Vector3.one * GetRandomScale(settingIndex);
        cloudTransform.anchoredPosition =
            new Vector2(
                GetStartX(cloudObject.transform.localScale.x * cloudTransform.rect.width),
                GetRandomY(cloudObject.transform.localScale.y * cloudTransform.rect.height));
        
        cloudObject.Sprite = CloudSettings[settingIndex].CloudSprite;
    }

    private bool TestChance(float chance)
    {
        float value = Random.value * 100f;
        return value <= chance;
    }

    private float GetRandomSpeed(int cloudSetting)
    {
        return Random.Range(CloudSettings[cloudSetting].CloudSpeedRange.x, CloudSettings[cloudSetting].CloudSpeedRange.y);
    }

    private float GetRandomScale(int cloudSetting)
    {
        return Random.Range(CloudSettings[cloudSetting].CloudScaleRange.x, CloudSettings[cloudSetting].CloudScaleRange.y);
    }

    private float GetStartX(float xWidth)
    {
        return -(xWidth/2f) - SpawnXBuffer;
    }

    private float GetRandomY(float height)
    {
        return Random.Range(_rectTransform.rect.yMin + height/2, _rectTransform.rect.yMax - height/2);
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