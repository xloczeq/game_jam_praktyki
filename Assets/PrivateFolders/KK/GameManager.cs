using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public Card cardPrefab;          // Assign in Inspector
    public int numberOfCards = 3;
    public int hearts = 2;

    private List<Card> spawnedCards = new List<Card>();

    void Start()
    {
        GenerateStage();
    }

    void GenerateStage()
    {
        ClearOldCards();

        // Randomly pick 1 correct card index
        int correctIndex = Random.Range(0, numberOfCards);

        for (int i = 0; i < numberOfCards; i++)
        {
            Card newCard = Instantiate(cardPrefab, new Vector3((i - 1) * 5, 0, -1), Quaternion.identity);

            // mark which one is correct
            newCard.isCorrect = (i == correctIndex);

            // subscribe to the card click event
            newCard.onCardSelected = OnCardSelected;

            spawnedCards.Add(newCard);
        }
    }

    void OnCardSelected(bool isCorrect)
    {
        if (isCorrect)
        {
            Debug.Log("Correct! Next stage.");
            GenerateStage(); // go to next
        }
        else
        {
            hearts--;
            Debug.Log("Wrong! Hearts left: " + hearts);

            if (hearts <= 0)
            {
                Debug.Log("Game Over!");
                // Stop the game or reset hearts; you decide
                return;
            }

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
