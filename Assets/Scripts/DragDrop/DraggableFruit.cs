using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;



[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(AudioSource))]
public class DraggableFruit : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public string fruitName;                 // used by RoundManager for correctness
    public FruitData Data { get; private set; }

    
    [SerializeField] private TMP_Text label;

    private SpriteRenderer sr;
    private AudioSource audioSource;
    private Vector3 startPos;

    private enum Lang { English, Khmer }
    private Lang currentMode = Lang.English;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        
        if (label == null || !label.transform.IsChildOf(transform))
        {
           
            label = GetComponentInChildren<TextMeshProUGUI>(true);
            if (label == null) label = GetComponentInChildren<TextMeshPro>(true);
            if (label == null) label = GetComponentInChildren<TMP_Text>(true);

            if (label == null)
            {
                Debug.LogError("[DraggableFruit] No TMP label found under this fruit. " +
                               "Add a child Canvas (World Space) with a Text (TMP), or a TextMeshPro (3D).", this);
            }
        }
    }

    void Start()
    {
        startPos = transform.position;
        RefreshLabel(); // safe even if label is null 
    }

    public void Init(FruitData data)
    {
        Data = data;
        fruitName = (data != null) ? data.fruitNameEnglish : null;
        if (sr != null && data != null) sr.sprite = data.sprite;
        currentMode = Lang.English;
        RefreshLabel();
    }

    public void Init(string englishName, Sprite sprite)
    {
        Data = null;
        fruitName = englishName;
        if (sr != null) sr.sprite = sprite;
        currentMode = Lang.English;
        RefreshLabel();
    }

    public void ResetToStart() => transform.position = startPos;

    private void RefreshLabel()
    {
        if (label == null) return; 
        if (currentMode == Lang.English)
            label.text = (Data != null) ? Data.fruitNameEnglish : fruitName;
        else
            label.text = (Data != null) ? Data.fruitNameKhmer : "(—)";
    }

    //hover / click 
    public void OnPointerEnter(PointerEventData _)
    {
        if (label == null) return;
        if (Data != null)
        {
            label.text = Data.fruitNameKhmer;
            PlayClip(Data.audioKhmer, 0.6f);
        }
    }

    public void OnPointerExit(PointerEventData _)
    {
        RefreshLabel();
    }

    public void OnPointerClick(PointerEventData _)
    {
        currentMode = (currentMode == Lang.English) ? Lang.Khmer : Lang.English;
        RefreshLabel();

        AudioClip clip = null;
        if (Data != null)
            clip = (currentMode == Lang.English) ? Data.audioEnglish : Data.audioKhmer;

        PlayClip(clip, 1f);
    }

    private void PlayClip(AudioClip clip, float volume)
    {
        if (clip == null || audioSource == null) return;
        audioSource.volume = Mathf.Clamp01(volume);
        audioSource.PlayOneShot(clip);
    }
}