using UnityEngine;

public class Card : MonoBehaviour
{
    public bool isCorrect = false;                 // Assigned by GameManager
    public System.Action<bool> onCardSelected;     // Callback to GameManager

    private void OnMouseDown()
    {
        onCardSelected?.Invoke(isCorrect);
    }
}
