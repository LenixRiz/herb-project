using UnityEngine;

public abstract class IdentifiableSO : ScriptableObject
{
    private const string GameNamespace = "herbproject";

    [Tooltip("Syncronized ID")]
    [SerializeField] private string _id;

    public string Id => _id;

#if UNITY_EDITOR
    // Can be used and overrided by child
    protected virtual void OnValidate()
    {
        string sanitizedName = name.Trim().ToLowerInvariant().Replace(" ", "_");

        _id = $"{GameNamespace}:{sanitizedName}";
    }
#endif
}