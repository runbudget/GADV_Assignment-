
using UnityEngine;
using TMPro;

public class CustomerRequestManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI requestLabel;

    void Awake()
    {
        // Auto-find a label if not assigned
        if (requestLabel == null)
        {
            requestLabel = GetComponentInChildren<TextMeshProUGUI>(true);
            if (requestLabel == null)
            {
                Debug.LogError("[CustomerRequestManager] No TextMeshProUGUI assigned or found. " +
                               "Assign one in the Inspector.", this);
            }
        }
    }

    public void SetRequest(string fruitName)
    {
        if (requestLabel == null)
        {
            Debug.LogError("[CustomerRequestManager] requestLabel is null; cannot update text.", this);
            return;
        }
        requestLabel.text = string.IsNullOrEmpty(fruitName)
            ? "No request!"
            : $"Hi! {fruitName}, please!";
    }
}
