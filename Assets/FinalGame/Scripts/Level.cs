using TMPro;
using UnityEngine;

public class Level : MonoBehaviour
{
    public TextMeshPro textDisplay;
    public void SetText(string text)
    {
        textDisplay.text = text;
    }
}
