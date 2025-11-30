using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Card cardPrefab;
    public TextMeshPro questionPrefab;
    public Level levelPrefab;
    public int numberOfCards = 3;
    public int hearts = 2;
    public HeartsUI heartsUI;

    public AudioSource audioSource;
    public AudioClip correctSound;
    public AudioClip wrongSound;

    public PauseMenu pauseMenu;

    public List<CardData> allCardData;
    public List<QuestionText> allQuestions;// Assign all Qxx objects here

    private List<Card> spawnedCards = new List<Card>();
    private bool isAnimatingStage = false;

    private List<int> availableStages = new List<int>();
    private int currentStage;
    private int currentLevel = 1;

    public string gameOverSceneName = "GameOver";
    public string victorySceneName = "Victory";

    void Start()
    {
        availableStages = allCardData.Select(d => d.stage).Distinct().ToList();

        StageTracker.stagesCompleted = 0;
        heartsUI.InitializeHearts(hearts);
        GenerateStage();
    }

    private IEnumerator GameOverRoutine()
    {
        audioSource.PlayOneShot(wrongSound);
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(gameOverSceneName);
    }

    private IEnumerator VictoryRoutine()
    {
        audioSource.PlayOneShot(correctSound);
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(victorySceneName);
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
        if (availableStages.Count == 0)
        {
            SceneManager.LoadScene(victorySceneName);
            return;
        }

        // Random stage
        int randomStageIndex = Random.Range(0, availableStages.Count);
        currentStage = availableStages[randomStageIndex];
        // Delete if you don't want stage replays:
        availableStages.RemoveAt(randomStageIndex);

        // Set question
        QuestionText q = allQuestions.Find(x => x.stage == currentStage);
        if (q != null) questionPrefab.SetText(q.displayText);

        // Set LEVEL text
        levelPrefab.SetText("Poziom " + currentLevel);


        // Draw all cards for this stage
        List<CardData> stageCards = allCardData
            .Where(d => d.stage == currentStage)
            .OrderBy(x => Random.value) // random order of cards
            .Take(numberOfCards)
            .ToList();

        for (int i = 0; i < stageCards.Count; i++)
        {
            Vector3 finalPos = new Vector3((i - 1) * 5, 0, 0);

            // Slide in always from the right
            Vector3 startPos = finalPos + new Vector3(30f, 0, 0);

            Card newCard = Instantiate(cardPrefab, finalPos, Quaternion.identity);

            // TEXT + CORRECT/NONCORRECT
            CardData data = stageCards[i];
            newCard.isCorrect = data.isCorrect;
            newCard.onCardSelected = OnCardSelected;
            newCard.SetText(data.displayText);
            newCard.SetName(data.characterName);
            newCard.SetPortrait(data.characterPortrait);

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
        currentLevel++;
        if (pauseMenu.isGamePaused) return;

        // Count this stage as completed
        if (isCorrect)
        {
            audioSource.PlayOneShot(correctSound);
            StageTracker.stagesCompleted++;

            // checking if there are stages
            if (availableStages.Count == 0)
            {
                StartCoroutine(VictoryRoutine());
                return;
            }

            GenerateStage();
        }
        else
        {
            audioSource.PlayOneShot(wrongSound);
            hearts--;
            heartsUI.LoseHeart();

            if (hearts <= 0)
            {
                StartCoroutine(GameOverRoutine());
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
