using UnityEngine;
using TMPro;

public class DisplayFinalTime : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI finalTimeText;

    private void Start()
    {
        finalTimeText.text = Timer.FinalTime.ToString("F3"); // Display the saved time
    }
}
