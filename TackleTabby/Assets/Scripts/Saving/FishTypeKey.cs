using System;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
public struct FishTypeKey
{
    [JsonProperty("FishName")]
    public readonly string FishName;
    [JsonProperty("MagicKey")]
    public readonly Hash128 MagicKey;
    [JsonProperty("Verify")]
    public readonly Hash128 Verify;

    [JsonIgnore]
    private FishDefinition _expanded;

    public FishTypeKey(FishDefinition fish)
    {
        FishName = fish.DisplayName;
        MagicKey = fish.MagicKey();
        
        Hash128 hash = new Hash128();
        hash.Append(FishName);
        hash.Append(MagicKey.ToString());
        Verify = hash;

        _expanded = null;
    }

    public FishDefinition Expand()
    {
        if (_expanded != null)
            return _expanded;
        
        ReadOnlySpan<FishDefinition> fishes = CentralFishStorage.Instance.GetAllFish();
        int fishCount = fishes.Length;

        for (int i = 0; i < fishCount; i++)
        {
            if (fishes[i].MagicKey() != MagicKey)
                continue;

            _expanded = fishes[i];
            return _expanded;
        }

        return null;
    }

    public bool VerifySelf()
    {
        FishDefinition type = Expand();
        if (type == null)
            return false;
        
        return new FishTypeKey(type).Verify == Verify;
    }
}