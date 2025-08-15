using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FruitGalleryItem : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text nameLabel;
    private AudioClip audioClip;

    public void SetData(FruitData data)
    {
        
        icon.sprite = null; 

        // Show both names
        nameLabel.text = $"{data.fruitNameEnglish}\n{data.fruitNameKhmer}";

        // Store audio for click
        audioClip = data.audioEnglish; 
    }

    public void OnCardClicked()
    {
        if (audioClip != null)
        {
            var source = GetComponent<AudioSource>();
            if (source != null)
                source.PlayOneShot(audioClip);
        }
    }
}
