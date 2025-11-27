using TMPro;
using UnityEngine;

public class Question : MonoBehaviour
{
    public TextMeshPro textDisplay;
    public void SetText(string text)
    {
        textDisplay.text = text;
    }
}
