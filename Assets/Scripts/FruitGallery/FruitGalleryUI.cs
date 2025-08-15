using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FruitGalleryPanel : MonoBehaviour
{
    [SerializeField] private FruitDatabaseSO database;  // Assign in Inspector
    [SerializeField] private Transform content;         // ScrollView Content
    [SerializeField] private FruitGalleryItem itemPrefab; // Card prefab

    void Awake()
    {
        // Force hidden on scene load
        gameObject.SetActive(false);
    }

    public void Open()
    {
        gameObject.SetActive(true);
        Populate();
    }

    public void Close()
    {
        Clear();
        gameObject.SetActive(false);
    }

    private void Populate()
    {
        if (!database || !content || !itemPrefab)
        {
            Debug.LogError("[FruitGalleryPanel] Missing refs.");
            return;
        }

        Clear();

        foreach (var fd in database.All)
        {
            if (fd == null) continue;
            var card = Instantiate(itemPrefab, content);   // instantiate prefab under Content
            card.SetData(fd);
        }
    }

    private void Clear()
    {
        for (int i = content.childCount - 1; i >= 0; i--)
            Destroy(content.GetChild(i).gameObject);
    }
}
