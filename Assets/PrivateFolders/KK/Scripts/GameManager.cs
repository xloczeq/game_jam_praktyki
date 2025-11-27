using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Card cardPrefab;
    public int numberOfCards = 3;
    public int hearts = 2;

    public List<CardData> allCardData;   // Assign all Qxx objects here

    private List<Card> spawnedCards = new List<Card>();
    private int currentStage = 1;

    public string gameOverSceneName = "GameOver";
    private bool isAnimatingStage = false;


    void Start()
    {
        StageTracker.stagesCompleted = 0;  // reset
        GenerateStage();
    }

    bool StageHasData(int stage)
    {
        // Checks if ANY CardData exists for this stage
        return allCardData.Exists(d => d.stage == stage);
    }


    void GenerateStage()
    {
        if (isAnimatingStage) return;
        isAnimatingStage = true;

        // --- 1. First slide old cards out if there are any ---
        if (spawnedCards.Count > 0)
        {
            int cardsToExit = spawnedCards.Count;
            int cardsExited = 0;

            foreach (Card card in spawnedCards)
            {
                CardAnimator anim = card.GetComponent<CardAnimator>();

                // Slide out **always to the left**
                Vector3 exitPos = card.transform.position + new Vector3(-30f, 0, 0);

                anim.SlideOutTo(exitPos, () =>
                {
                    cardsExited++;

                    if (cardsExited >= cardsToExit)
                    {
                        ClearOldCards();
                        SpawnNewStageCards();
                    }
                });
            }
        }
        else
        {
            SpawnNewStageCards();
        }

    }

    void SpawnNewStageCards()
    {
        // No more scriptable objects for this stage -> game over
        if (!StageHasData(currentStage))
        {
            Debug.Log("No more stages! Game Over.");
            SceneManager.LoadScene(gameOverSceneName);
            return;
        }

        int correctIndex = Random.Range(0, numberOfCards);

        for (int i = 0; i < numberOfCards; i++)
        {
            Vector3 finalPos = new Vector3((i - 1) * 5, 0, 0);

            // Slide in always from the right
            Vector3 startPos = finalPos + new Vector3(30f, 0, 0);

            Card newCard = Instantiate(cardPrefab, finalPos, Quaternion.identity);

            // TEXT + CORRECT/NONCORRECT
            CardData data = allCardData.Find(d => d.stage == currentStage && d.cardIndex == i + 1);
            newCard.isCorrect = (i == Random.Range(0, numberOfCards)); // keep previous logic
            newCard.onCardSelected = OnCardSelected;
            newCard.SetText(data.displayText);

            // Animate in
            CardAnimator anim = newCard.GetComponent<CardAnimator>();
            anim.targetPosition = finalPos;
            anim.SlideInFrom(startPos);

            spawnedCards.Add(newCard);
        }


        isAnimatingStage = false;
    }


    void OnCardSelected(bool isCorrect)
    {
        // Count this stage as completed
        if (isCorrect)
        {
            Debug.Log("Correct!");
            currentStage++;
            StageTracker.stagesCompleted++;
            GenerateStage();
        }
        else
        {
            hearts--;
            Debug.Log("Wrong! Hearts left: " + hearts);

            if (hearts <= 0)
            {
                Debug.Log("Game Over!");
                SceneManager.LoadScene(gameOverSceneName);

                return;
            }

            currentStage++;
            GenerateStage();
        }
    }


    void ClearOldCards()
    {
        foreach (Card c in spawnedCards)
            Destroy(c.gameObject);

        spawnedCards.Clear();
    }
}
