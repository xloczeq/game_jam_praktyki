using UnityEngine;

[CreateAssetMenu(menuName = "Card/QuestionText")]
public class QuestionText : ScriptableObject
{
    public int stage;        // e.g. 1 in S1
    public string displayText;
}
