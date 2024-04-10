using UnityEngine;
using TMPro;

public class DisplayFinalTime : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI finalTimeText;

    private void Start()
    {
        // Calculate minutes, seconds, and milliseconds from Timer.FinalTime
        int minutes = (int)(Timer.FinalTime / 60);
        int seconds = (int)(Timer.FinalTime % 60);
        int milliseconds = (int)((Timer.FinalTime - (int)Timer.FinalTime) * 1000);

        // Format the string as 00:00:000
        finalTimeText.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }
}
