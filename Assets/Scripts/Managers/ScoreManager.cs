using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private LevelCriteriaSO criteria;

    private readonly HashSet<string> uniqueCorrect =
        new HashSet<string>(StringComparer.OrdinalIgnoreCase);


    public int Coins { get; private set; }
    public int StarsToWin => criteria != null ? criteria.starsToWin : 3;
    public int UniqueStars => uniqueCorrect.Count;
    public bool LevelCleared { get; private set; }

    public event Action OnScoreChanged;
    public event Action OnLevelCleared;

    public void ResetAll()
    {
        Coins = 0;
        uniqueCorrect.Clear();
        LevelCleared = false;
        OnScoreChanged?.Invoke();

    }

    public void RegisterCorrect(string fruitName)
    {
        if (LevelCleared) return;

        Coins += criteria != null ? criteria.coinsPerCorrect : 5;
        bool newStar = uniqueCorrect.Add(fruitName?.Trim());

        OnScoreChanged?.Invoke();
        
        if (newStar && UniqueStars >= StarsToWin)
        {
            LevelCleared = true;
            OnLevelCleared?.Invoke();
        }

    }
    public bool HasStarFor(string fruitName) =>
        uniqueCorrect.Contains(fruitName?.Trim());

    // Prefer requests not yet collected; fallback to any
    public string PickRequestedFruit(FruitDatabaseSO db)
    {
        var all = db.All;
        var names = new List<string>(all.Count);
        for (int i = 0; i < all.Count; i++)
        {
            var n = all[i]?.fruitNameEnglish;
            if (!string.IsNullOrEmpty(n) && !names.Contains(n))
                names.Add(n);
        }

        // If already cleared, return null
        if (UniqueStars >= StarsToWin) return null;

        var remaining = new List<string>();
        foreach (var n in names) if (!HasStarFor(n)) remaining.Add(n);

        if (remaining.Count > 0) return remaining[UnityEngine.Random.Range(0, remaining.Count)];
        if (names.Count > 0) return names[UnityEngine.Random.Range(0, names.Count)];
        return null;
    }


}
