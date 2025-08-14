using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorrectItemChecker : MonoBehaviour
{

    [Header("Serialized List of Fruits")]
    [SerializeField]
    private List<FruitData> fruitList = new List<FruitData>();

    private Dictionary<string, Sprite> fruitDictionary;

    void Awake()
    {
        BuildFruitDictionary();
        Debug.Log("abc");
    }

    private void BuildFruitDictionary()
    {
        fruitDictionary = new Dictionary<string, Sprite>();

        foreach (FruitData fruitData in fruitList)
        {
            if (!fruitDictionary.ContainsKey(fruitData.fruitNameEnglish))
            {
                fruitDictionary.Add(fruitData.fruitNameEnglish, fruitData.sprite);
            }
            else
            {
                Debug.LogWarning($"Duplicate fruit name found: {fruitData.fruitNameEnglish}. Skipping.");
            }
        }
    }
}
