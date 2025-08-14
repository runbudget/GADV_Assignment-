using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropAreaCheck : MonoBehaviour
{
    public CustomerRequest customerRequest;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Fruit"))
        {
            FruitInfo fruit = other.GetComponent<FruitInfo>();
            if (fruit != null && customerRequest != null)
            {
                if (customerRequest.CheckIfCorrect(fruit.fruitName))
                {
                    Debug.Log(" Correct fruit delivered!");
                    // Optional: play sound, give points, remove customer
                }
                else
                {
                    Debug.Log(" Wrong fruit.");
                    // Optional: show sad face or retry message
                }
            }
        }
    }
}
