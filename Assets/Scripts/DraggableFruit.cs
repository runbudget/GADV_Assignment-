using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;



[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class DraggableFruit : MonoBehaviour
{
    // Legacy-friendly field used by DragDrop2D/CheckDrop
    public string fruitName;                // will mirror Data.fruitNameEnglish

    // Full data (bilingual, audio, etc.)
    public FruitData Data { get; private set; }

    private SpriteRenderer sr;
    private Vector3 startPos;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        startPos = transform.position;
    }

    // --- Overload #1: old style ---
    public void Init(string englishName, Sprite sprite)
    {
        fruitName = englishName;
        if (sr != null) sr.sprite = sprite;
        Data = null; // optional
    }

    // --- Overload #2: new data-based style ---
    public void Init(FruitData data)
    {
        Data = data;
        fruitName = data != null ? data.fruitNameEnglish : null;
        if (sr != null && data != null) sr.sprite = data.sprite;
    }

    public void ResetToStart()
    {
        transform.position = startPos;
    }
}