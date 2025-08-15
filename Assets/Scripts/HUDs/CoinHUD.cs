using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinHUD : MonoBehaviour
{
    [SerializeField] private TMP_Text coinText;
    public void SetCoins(int coins)
    {
        if (coinText != null) coinText.text = $"Coins: {coins}";
    }
}
