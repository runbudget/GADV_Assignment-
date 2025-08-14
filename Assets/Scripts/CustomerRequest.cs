using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerRequest : MonoBehaviour
{
    public string requestedFruit = "apple"; 

    public bool CheckIfCorrect(string fruitName)
    {
        return fruitName.ToLower() == requestedFruit.ToLower(); 
    }
}
