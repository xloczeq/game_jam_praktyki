using UnityEngine;

[CreateAssetMenu(menuName = "Card/CardData")]
public class CardData : ScriptableObject
{
    public int stage;        // e.g. 1 in Q11
    public int cardIndex;    // e.g. 1 in Q11
    public string displayText;
    public string characterName;
    public bool isCorrect;
}
