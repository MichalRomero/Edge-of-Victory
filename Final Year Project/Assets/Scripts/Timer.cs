using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    float elapsedTime;
    public static float FinalTime { get; private set; } // Static property to hold final time

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        // Calculate minutes, seconds, and milliseconds
        int minutes = (int)(elapsedTime / 60);
        int seconds = (int)(elapsedTime % 60);
        int milliseconds = (int)((elapsedTime - (int)elapsedTime) * 1000);

        // Format the string as 00:00:000
        timerText.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }

    public void GameFinished()
    {
        FinalTime = elapsedTime; // Save the time when the game is finished
    }
}
