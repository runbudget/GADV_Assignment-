using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FruitDatabase", menuName = "ScriptableObjects/FruitDatabaseSO")]
public class FruitDatabaseSO : ScriptableObject
{
    [Header("Serialized List of Fruits")]
    [SerializeField]
    private List<FruitData> fruitList = new List<FruitData>();

    private Dictionary<string, Sprite> fruitDictionary;

    public void Initialize()
    {
        fruitDictionary = new Dictionary<string, Sprite>();

        foreach (FruitData fruitData in fruitList)
        {
            if (!fruitDictionary.ContainsKey(fruitData.fruitName))
            {
                fruitDictionary.Add(fruitData.fruitName, fruitData.fruit);
            }
            else
            {
                Debug.LogWarning($"Duplicate fruit name found: {fruitData.fruitName}. Skipping.");
            }
        }
    }

    public Sprite GetFruitSprite(string fruitName)
    {
        if (fruitDictionary == null)
        {
            Debug.LogWarning("Fruit dictionary not initialized. Call Initialize() first.");
            return null;
        }

        if (fruitDictionary.TryGetValue(fruitName, out Sprite sprite))
        {
            return sprite;
        }

        Debug.LogWarning($"Fruit not found: {fruitName}");
        return null;
    }

    public IReadOnlyList<FruitData> All => fruitList;

    public string GetRandomName()
    {
        if (fruitList == null || fruitList.Count == 0) return null;
        return fruitList[Random.Range(0, fruitList.Count)].fruitName;
    }

    
}

