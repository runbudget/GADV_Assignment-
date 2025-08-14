using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

[CreateAssetMenu(fileName = "FruitDatabase", menuName = "ScriptableObjects/FruitDatabaseSO")]
public class FruitDatabaseSO : ScriptableObject
{
    [SerializeField] private List<FruitData> fruitList = new List<FruitData>();

    private Dictionary<string, FruitData> byEnglishName;

    public IReadOnlyList<FruitData> All => fruitList;

    public void Initialize()
    {
        byEnglishName = new Dictionary<string, FruitData>();
        foreach (var f in fruitList)
        {
            if (f == null || string.IsNullOrWhiteSpace(f.fruitNameEnglish)) continue;
            if (!byEnglishName.ContainsKey(f.fruitNameEnglish))
                byEnglishName.Add(f.fruitNameEnglish, f);
            else
                Debug.LogWarning($"Duplicate English fruit name: {f.fruitNameEnglish}");
        }
    }

    // Your RoundManager asks for this:
    public string GetRandomName()
    {
        if (fruitList == null || fruitList.Count == 0) return null;
        return fruitList[Random.Range(0, fruitList.Count)].fruitNameEnglish;
    }

    // Convenience (used by older code)
    public Sprite GetFruitSprite(string englishName)
    {
        var fd = GetByEnglish(englishName);
        return fd != null ? fd.sprite : null;
    }

    // Helpful for data-based spawning
    public FruitData GetRandomData()
    {
        if (fruitList == null || fruitList.Count == 0) return null;
        return fruitList[Random.Range(0, fruitList.Count)];
    }

    public FruitData GetByEnglish(string englishName)
    {
        if (string.IsNullOrWhiteSpace(englishName) || byEnglishName == null) return null;
        return byEnglishName.TryGetValue(englishName, out var fd) ? fd : null;
    }

    
}