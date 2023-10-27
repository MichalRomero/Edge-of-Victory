using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    // The UI text element that displays prompts to the player.
    [SerializeField]
    private TextMeshProUGUI promptText;

    // Start is called before the first frame update.
    void Start()
    {
        
    }

    // Public method that updates the prompt text on the player's UI.
    public void UpdateText(string promptMessage)
    {
        promptText.text = promptMessage; // Set the text of the 'promptText' UI element.
    }
}
