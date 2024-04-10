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
        timerText.text = elapsedTime.ToString("F3"); // Format the time
    }

    public void GameFinished()
    {
        FinalTime = elapsedTime; // Save the time when game is finished
        // Optional: Load the new scene here if needed
    }
}
