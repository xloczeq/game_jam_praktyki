using UnityEngine;
using TMPro;

public class Card : MonoBehaviour
{
    public bool isCorrect = false;
    public System.Action<bool> onCardSelected;
    public TextMeshPro textDisplay;
    public TextMeshPro nameDisplay;

    public void SetText(string text)
    {
        textDisplay.text = text;
    }
    public void SetName(string text)
    {
        nameDisplay.text = text;
    }

    private void OnMouseDown()
    {
        onCardSelected?.Invoke(isCorrect);
    }

    private void OnMouseEnter()
    {
        gameObject.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
    }
    private void OnMouseExit()
    {
        gameObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
    }
}
