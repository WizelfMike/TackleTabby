using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class CentralTrashStorage : GenericSingleton<CentralTrashStorage>
{
    [SerializeField]
    private TrashDefinition[] TrashList = Array.Empty<TrashDefinition>();

    public TrashDefinition GetRandomTrash()
    {
        if (TrashList.Length == 0)
            return null;
        
        return TrashList[Random.Range(0, TrashList.Length)];
    }
}