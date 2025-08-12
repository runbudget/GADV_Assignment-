using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// picks the request, spawns fruits,
// clears between rounds, and resets the pos of wrong selections

public class RoundManager : MonoBehaviour
{
    [Header("Data & UI")]
    [SerializeField] private FruitDatabaseSO database;          // assign in Inspector
    [SerializeField] private CustomerRequestManager requestUI;  // assign in Inspector

    [Header("Spawning")]
    [SerializeField] private DraggableFruit draggableFruitPrefab; // assign prefab
    [SerializeField] private List<Transform> spawnPoints;         // assign in Inspector
    [SerializeField] private int fruitsPerRound = 4;

    private readonly List<DraggableFruit> activeFruits = new List<DraggableFruit>();
    private string currentRequest;

    void Start()
    {
        if (!ValidateSetup()) return;

        database.Initialize();
        StartNewRound();
    }

    public void StartNewRound()
    {
        if (!ValidateSetup()) return;

        ClearFruits();

        // 1) pick the requested fruit (must exist)
        currentRequest = database.GetRandomName();
        if (string.IsNullOrEmpty(currentRequest))
        {
            Debug.LogError("[RoundManager] Database has no fruits. Cannot start round.", this);
            return;
        }

        requestUI.SetRequest(currentRequest);

        // 2) figure out how many fruits we can actually spawn
        int maxBySpawnPoints = spawnPoints != null ? spawnPoints.Count : 0;
        int totalUnique = Mathf.Max(1, database.All.Count); // unique fruit names available
        int maxByUnique = Mathf.Min(totalUnique, fruitsPerRound);
        int targetCount = Mathf.Min(fruitsPerRound, maxBySpawnPoints, maxByUnique);

        if (targetCount <= 0)
        {
            Debug.LogError("[RoundManager] Either no spawn points or fruitsPerRound <= 0.", this);
            return;
        }

        // 3) build the names list: one correct + (targetCount-1) distinct others
        var names = BuildNamesSafe(targetCount, currentRequest);

        // 4) spawn them
        // (names.Count == targetCount and contains exactly one correct already)
        Shuffle(spawnPoints);

        for (int i = 0; i < targetCount; i++)
        {
            var nameToSpawn = names[i];
            var sprite = database.GetFruitSprite(nameToSpawn);
            if (sprite == null)
            {
                Debug.LogWarning($"[RoundManager] No sprite for '{nameToSpawn}'. Skipping.");
                continue;
            }

            var fruit = Instantiate(draggableFruitPrefab, spawnPoints[i].position, Quaternion.identity);
            fruit.Init(nameToSpawn, sprite);
            activeFruits.Add(fruit);
        }
    }

    public void CheckDrop(string fruitName, DraggableFruit fruit)
    {
        // guard just in case
        if (fruit == null) return;

        if (string.Equals(fruitName, currentRequest))
        {
            StartNewRound();
        }
        else
        {
            fruit.ResetToStart();
        }
    }

    private void ClearFruits()
    {
        foreach (var f in activeFruits)
        {
            if (f != null) Destroy(f.gameObject);
        }
        activeFruits.Clear();
    }

    // Builds a list of 'count' names that includes exactly one 'mustInclude' and the rest unique others.
    private List<string> BuildNamesSafe(int count, string mustInclude)
    {
        // Gather all unique names
        var all = database.All;
        var pool = new List<string>(all.Count);
        for (int i = 0; i < all.Count; i++)
        {
            var n = all[i].fruitName;
            if (!string.IsNullOrEmpty(n))
                pool.Add(n);
        }

        // Ensure mustInclude exists in pool
        if (!pool.Contains(mustInclude))
            pool.Add(mustInclude);

        // Pick (count-1) distinct others != mustInclude
        var others = new List<string>(pool);
        others.RemoveAll(n => n == mustInclude);

        Shuffle(others);

        var result = new List<string>(count);
        result.Add(mustInclude); // exactly one correct
        for (int i = 0; i < count - 1 && i < others.Count; i++)
            result.Add(others[i]);

        // If we still don't have enough (very small DB), just repeat random entries from pool (excluding adding more corrects)
        int guard = 100;
        while (result.Count < count && guard-- > 0)
        {
            var candidate = others.Count > 0 ? others[Random.Range(0, others.Count)] : mustInclude;
            // allow duplicates if we must, but avoid adding second copy of mustInclude
            if (candidate != mustInclude || !result.Contains(candidate))
                result.Add(candidate);
        }

        // Shuffle result so correct isn't always index 0
        Shuffle(result);
        return result;
    }

    private void Shuffle<T>(IList<T> list)
    {
        if (list == null) return;
        for (int i = 0; i < list.Count; i++)
        {
            int j = Random.Range(i, list.Count);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }

    private bool ValidateSetup()
    {
        if (database == null)
        {
            Debug.LogError("[RoundManager] 'database' is not assigned.", this);
            return false;
        }
        if (requestUI == null)
        {
            Debug.LogError("[RoundManager] 'requestUI' is not assigned.", this);
            return false;
        }
        if (draggableFruitPrefab == null)
        {
            Debug.LogError("[RoundManager] 'draggableFruitPrefab' is not assigned.", this);
            return false;
        }
        if (spawnPoints == null || spawnPoints.Count == 0)
        {
            Debug.LogError("[RoundManager] 'spawnPoints' is empty or not assigned.", this);
            return false;
        }
        if (fruitsPerRound <= 0)
        {
            Debug.LogError("[RoundManager] 'fruitsPerRound' must be > 0.", this);
            return false;
        }
        return true;
    }
}