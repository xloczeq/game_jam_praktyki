using TMPro;
using UnityEngine;

public class StagesCompleted : MonoBehaviour
{
    public TextMeshProUGUI stagesText;

    void Update()
    {
        stagesText.text = "Stages Completed: " + StageTracker.stagesCompleted;
    }
}
