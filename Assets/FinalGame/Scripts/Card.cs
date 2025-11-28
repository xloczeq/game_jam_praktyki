using UnityEngine;
using TMPro;

public class Card : MonoBehaviour
{
    public bool isCorrect = false;
    public System.Action<bool> onCardSelected;

    public TextMeshPro textDisplay;
    public TextMeshPro nameDisplay;
    public UnityEngine.UI.Image portraitDisplay;

    // --- SCALE ANIMATION ---
    private Vector3 targetScale;
    private Vector3 normalScale = new Vector3(0.7f, 0.7f, 0.7f);
    private Vector3 hoverScale = new Vector3(0.85f, 0.85f, 0.85f);
    public float scaleSpeed = 10f;

    void Start()
    {
        targetScale = normalScale;
        transform.localScale = normalScale;
    }

    void Update()
    {
        // Plynne przejscie skali
        transform.localScale = Vector3.Lerp(
            transform.localScale,
            targetScale,
            Time.deltaTime * scaleSpeed
        );
    }

    public void SetText(string text)
    {
        textDisplay.text = text;
    }

    public void SetName(string text)
    {
        nameDisplay.text = text;
    }

    public void SetPortrait(Sprite sprite)
    {
        portraitDisplay.sprite = sprite;
    }

    private void OnMouseDown()
    {
        onCardSelected?.Invoke(isCorrect);
    }

    private void OnMouseEnter()
    {
        targetScale = hoverScale;
    }

    private void OnMouseExit()
    {
        targetScale = normalScale;
    }
}
