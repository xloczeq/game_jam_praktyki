using UnityEngine;
using TMPro;

public class Card : MonoBehaviour
{
    public bool isCorrect = false;
    public System.Action<bool> onCardSelected;
    public TextMeshPro textDisplay;

    public void SetText(string text)
    {
        textDisplay.text = text;
    }

    private void OnMouseDown()
    {
        onCardSelected?.Invoke(isCorrect);
    }
}
