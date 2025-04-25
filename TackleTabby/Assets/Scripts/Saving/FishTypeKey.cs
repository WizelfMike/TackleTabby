using System;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
public struct FishTypeKey
{
    [JsonProperty("FishName")]
    public readonly string FishName;
    [JsonProperty("MagicKey")]
    public readonly string MagicKey;
    [JsonProperty("Verify")]
    public readonly string Verify;

    [JsonIgnore]
    private FishDefinition _expanded;

    public FishTypeKey(FishDefinition fish)
    {
        FishName = fish.DisplayName;
        MagicKey = fish.MagicKey().ToString();
        
        Hash128 hash = new Hash128();
        hash.Append(FishName);
        hash.Append(MagicKey);
        Verify = BuildVerify(FishName, MagicKey).ToString();

        _expanded = null;
    }

    private static Hash128 BuildVerify(string fishName, string magicKey)
    {
        Hash128 hash = new Hash128();
        hash.Append(fishName);
        hash.Append(magicKey);
        return hash;
    }

    public FishDefinition Expand()
    {
        if (_expanded != null)
            return _expanded;

        Hash128 key = Hash128.Parse(MagicKey);
        ReadOnlySpan<FishDefinition> fishes = CentralFishStorage.Instance.GetAllFish();
        int fishCount = fishes.Length;

        for (int i = 0; i < fishCount; i++)
        {
            if (fishes[i].MagicKey() != key)
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

        return BuildVerify(FishName, MagicKey).ToString() == Verify;
    }
}