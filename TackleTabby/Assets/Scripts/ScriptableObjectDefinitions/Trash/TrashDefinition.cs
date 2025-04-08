using UnityEngine;

[CreateAssetMenu(fileName = "TrashDefinition", menuName = "Scriptable Objects/TrashDefinition")]
public class TrashDefinition : ScriptableObject
{
    public string DisplayName;
    public Sprite TrashSprite;
}