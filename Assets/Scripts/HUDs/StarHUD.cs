
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class StarHUD : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text counter;
    [SerializeField] private List<Image> starIcons;   // 3 Image refs in order (left->right)
    [SerializeField] private Sprite starOn;           //  star sprite
    [SerializeField] private Sprite starOff;          // empty star sprite

    public void SetStars(int current, int goal)
    {
        if (counter != null) counter.text = $"{current} / {goal}";

        if (starIcons == null || starIcons.Count == 0) return;
        for (int i = 0; i < starIcons.Count; i++)
        {
            var img = starIcons[i];
            if (img == null) continue;

            bool lit = i < current;
            var targetSprite = lit ? starOn : starOff;

            img.overrideSprite = targetSprite; // more explicit than .sprite
            img.SetAllDirty();                 // force refresh
            img.color = Color.white;

            
        }
    }
}





