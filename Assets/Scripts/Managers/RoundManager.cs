using System;
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
    [SerializeField] private DraggableFruit draggableFruitPrefab; // assign prefab (root has DraggableFruit)
    [SerializeField] private List<Transform> spawnPoints;         // assign in Inspector
    [SerializeField] private int fruitsPerRound = 3;


    [SerializeField] private ScoreManager score;   // << assign in Inspector
[SerializeField] private StarHUD starHUD;
[SerializeField] private StarHUD starHUDComplete;   // optional: for the complete panel
[SerializeField] private CoinHUD coinHUD;
[SerializeField] private CoinHUD coinHUDComplete;   // optional
[SerializeField] private GameObject levelCompletePanel;


    private readonly List<DraggableFruit> activeFruits = new List<DraggableFruit>();
    private readonly HashSet<string> uniqueCorrect = new HashSet<string>(); //track unique fruit names
    private string currentRequest;
    
    

    void Start()
    {
        if (!ValidateSetup()) return;


        //check if auto-find not wired

        if (score == null)
        {
            score = FindObjectOfType<ScoreManager>();
            if (score == null)
            {
                Debug.LogError("[RoundManger] scoremanager not found", this);
                return;
            }
        }

        database.Initialize();
        score.ResetAll();

        // Hook HUD updates
        score.OnScoreChanged += () =>
        {
            starHUD?.SetStars(score.UniqueStars, score.StarsToWin);
            starHUDComplete?.SetStars(score.UniqueStars, score.StarsToWin);
            coinHUD?.SetCoins(score.Coins);
            coinHUDComplete?.SetCoins(score.Coins);
        };
        score.OnLevelCleared += () =>
        {
            ClearFruits();
            if (levelCompletePanel != null) levelCompletePanel.SetActive(true);
        };

        // Prime HUD
        starHUD?.SetStars(score.UniqueStars, score.StarsToWin);
        coinHUD?.SetCoins(score.Coins);
        if (levelCompletePanel != null) levelCompletePanel.SetActive(false);

        StartNewRound();
    }

    public void StartNewRound()
    {
        if (score.LevelCleared) return;
        if (!ValidateSetup()) return;

        ClearFruits();

        currentRequest = score.PickRequestedFruit(database);
        if (string.IsNullOrEmpty(currentRequest)) return; // nothing to request

        requestUI.SetRequest(currentRequest);
        // 2) figure out how many fruits we can actually spawn
        int maxBySpawnPoints = (spawnPoints != null) ? spawnPoints.Count : 0;
        int totalUnique = Mathf.Max(1, database.All.Count); // unique fruit entries available
        int maxByUnique = Mathf.Min(totalUnique, fruitsPerRound);
        int targetCount = Mathf.Min(fruitsPerRound, maxBySpawnPoints, maxByUnique);

        if (targetCount <= 0)
        {
            Debug.LogError("[RoundManager] Either no spawn points or fruitsPerRound <= 0.", this);
            return;
        }

        // 3) build the names list: includes exactly one 'currentRequest' + (targetCount-1) distinct others
        var names = BuildNamesSafe(targetCount, currentRequest);

        // 4) spawn them (parent to spawn point to avoid offsets/overlaps)
        Shuffle(spawnPoints);

        for (int i = 0; i < targetCount; i++)
        {
            var nameToSpawn = names[i];
            var data = database.GetByEnglish(nameToSpawn);
            if (data == null || data.sprite == null)
            {
                Debug.LogWarning($"[RoundManager] Missing data/sprite for '{nameToSpawn}'. Skipping.");
                continue;
            }

            var fruit = Instantiate(draggableFruitPrefab, spawnPoints[i], false);
            fruit.transform.localPosition = Vector3.zero;
            fruit.transform.localRotation = Quaternion.identity;
            fruit.transform.localScale = Vector3.one;


            var drag = fruit.GetComponent<DragDrop2D>();
            if (drag != null) drag.SetRoundManager(this);

            fruit.Init(data); // <-- now each fruit knows Eng/Khmer + audio
            activeFruits.Add(fruit);
        }
    }

    public void CheckDrop(string fruitName, DraggableFruit fruit)
    {
        if (fruit == null) return;

        string f = fruitName?.Trim();
        string r = currentRequest?.Trim();
        bool correct = string.Equals(f, r, StringComparison.OrdinalIgnoreCase);

        if (correct)
        {
            score.RegisterCorrect(r);
            if (!score.LevelCleared) StartNewRound();
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

    // Builds a list of 'count' English names that includes exactly one 'mustInclude' and the rest unique others.
    private List<string> BuildNamesSafe(int count, string mustInclude)
    {
        // Gather all unique English names from the database
        var all = database.All;
        var pool = new List<string>(all.Count);
        for (int i = 0; i < all.Count; i++)
        {
            var n = all[i]?.fruitNameEnglish; // <<< UPDATED FIELD NAME
            if (!string.IsNullOrEmpty(n) && !pool.Contains(n))
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

        // If we still don't have enough (very small DB), fill with non-correct items
        int guard = 100;
        while (result.Count < count && guard-- > 0)
        {
            if (others.Count == 0) break; // nothing else to add
            var candidate = others[UnityEngine.Random.Range(0, others.Count)];
            // allow duplicates if necessary, but do not add multiple of mustInclude
            result.Add(candidate);
        }

        // Shuffle result so correct isn't always first
        Shuffle(result);
        return result;
    }

    private void Shuffle<T>(IList<T> list)
    {
        if (list == null) return;
        for (int i = 0; i < list.Count; i++)
        {
            int j = UnityEngine.Random.Range(i, list.Count);
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