
using UnityEngine;
using TMPro;

public class CustomerRequestManager : MonoBehaviour
{
    [SerializeField] private FruitDatabaseSO database;
    [SerializeField] private TextMeshProUGUI requestLabel;


    public string CurrentRequest { get; private set; }

    void Start()
    {
        database.Initialize();
        NewRequest();
    }


    public void NewRequest()
    {
        CurrentRequest = database.GetRandomName();
        requestLabel.text = string.IsNullOrEmpty(CurrentRequest)
            ? "No fruits avalable"
            : $"Hi! {CurrentRequest}, please!";
    }
}
