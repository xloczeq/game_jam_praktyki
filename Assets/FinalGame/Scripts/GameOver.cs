using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public TextMeshProUGUI stagesText;
    void Start()
    {
        stagesText.text = "Stages Completed: " + StageTracker.stagesCompleted;
    }
    public void QuitGame()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }
}
