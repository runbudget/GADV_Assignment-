
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class StarHUD : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text counter;
    [SerializeField] private List<Image> starIcons;   // 3 Image refs in order (left->right)
    [SerializeField] private Sprite starOn;           // filled / glowing star sprite
    [SerializeField] private Sprite starOff;          // empty star sprite

    public void SetStars(int current, int goal)
    {
        if (counter != null) counter.text = $"{current} / {goal}";
        if (starIcons == null) return;

        for (int i = 0; i < starIcons.Count; i++)
        {
            if (starIcons[i] == null) continue;
            bool lit = i < current;
            starIcons[i].sprite = lit ? starOn : starOff;
            starIcons[i].color = Color.white; // make sure it's visible
        }
    }
}





