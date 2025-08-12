using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitDropCheck : MonoBehaviour
{
    [SerializeField] private CustomerRequestManager requestManager;

    public void CheckDroppedFruit(string droppedFruitName)
    {
        bool correct = string.Equals(droppedFruitName?.Trim(),
                                      requestManager.CurrentRequest?.Trim(),
                                      System.StringComparison.OrdinalIgnoreCase);
        // ?.Trim() to b safe with nulls/whitespace

        if (correct)
        {
            Debug.Log("Correct fruit given!");

            //Can add success sound, score, 
            requestManager.NewRequest();
        }
        else
        {
            Debug.Log("Wrong fruit given!");
            // feedback like sound or smth
        }

    }
}
