using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitDropJudge : MonoBehaviour
{
    [SerializeField] private CustomerRequestManager requestManager;

    public void CheckDroppedFruit( string droppedFruitName)
    {
        bool correct = string.Equals(droppedFruitName?.Trim(), requestManager.CurrentRequest?.Trim(), 
                                     System.StringComparison.OrdinalIgnoreCase);

        // ?.Trim() for nulls or whitespaces

        if (correct)
        {
            Debug.Log("You've sold the correct fruit!");
            // hmm add success sound, add score, show some animate success?
            requestManager.NewRequest();

        }

        else
        {
            Debug.Log("You've selected the wrong fruit!");
            // feedback for wrong ans like sound
        }
    }
}
