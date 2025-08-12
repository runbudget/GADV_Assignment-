using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// stores name, sprite, and start position
[RequireComponent(typeof(SpriteRenderer))]
public class DraggableFruit : MonoBehaviour
{
    [Tooltip("Match the name of the fruit in the database")]
    public string fruitName;
    private SpriteRenderer sr;
    private Vector3 startPos;


    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        //record spawn position for resetting on wrong drop
        startPos = transform.position;
    }

    public void Init(string name, Sprite sprite)
    {
        fruitName = name;
        sr.sprite = sprite;
    }
    
    public void ResetToStart()
    {
        transform.position = startPos;
    }
}

