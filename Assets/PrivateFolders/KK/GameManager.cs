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

    public string gameOverSceneName = "GameOverScene";

    void Start()
    {
        GenerateStage();
    }

    void GenerateStage()
    {
        ClearOldCards();

        int correctIndex = Random.Range(0, numberOfCards);

        for (int i = 0; i < numberOfCards; i++)
        {
            Card newCard = Instantiate(cardPrefab, new Vector3((i-1) * 5, 0, -1), Quaternion.identity);

            // correct card index
            newCard.isCorrect = (i == correctIndex);

            // ---- LOAD SCRIPTABLE OBJECT TEXT BASED ON STAGE + CARD NUMBER ----
            CardData data = allCardData.First(d => d.stage == currentStage && d.cardIndex == i + 1);

            newCard.SetText(data.displayText);

            newCard.onCardSelected = OnCardSelected;

            spawnedCards.Add(newCard);
        }
    }

    void OnCardSelected(bool isCorrect)
    {
        if (isCorrect)
        {
            Debug.Log("Correct!");
            currentStage++;     // move to next stage
            GenerateStage();
        }
        else
        {
            hearts--;
            Debug.Log("Wrong! Hearts left: " + hearts);

            if (hearts <= 0)
            {
                Debug.Log("Game Over!");
                SceneManager.LoadSceneAsync(gameOverSceneName);
                return;
            }

            currentStage++;      // <--- ADVANCE STAGE EVEN ON WRONG ANSWER
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
