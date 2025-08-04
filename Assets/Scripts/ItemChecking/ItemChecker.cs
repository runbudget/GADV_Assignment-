using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ItemChecker : MonoBehaviour
{
    public FruitDatabaseSO fruitDatabase;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;
        Sprite sprite = gameObject.GetComponent<SpriteRenderer>().sprite;


    }

    public bool CheckFruit(string fruitName)
    {
        Sprite sprite = fruitDatabase.GetFruitSprite(fruitName);
        if (sprite)
        {
            return true;
        }
        return false;
    }    
}
