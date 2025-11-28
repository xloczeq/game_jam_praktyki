using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartsUI : MonoBehaviour
{
    [Header("Assign in Inspector")]
    public GameObject heartPrefab;       // Prefab serca
    public Transform heartsContainer;    // Parent dla serc (UI HorizontalLayoutGroup)

    private List<GameObject> hearts = new List<GameObject>();

    public void InitializeHearts(int count)
    {
        // Wyczysc stare
        foreach (var h in hearts)
            Destroy(h);

        hearts.Clear();

        // Stworz nowe
        for (int i = 0; i < count; i++)
        {
            GameObject heart = Instantiate(heartPrefab, heartsContainer);
            hearts.Add(heart);
        }
    }

    public void LoseHeart()
    {
        if (hearts.Count == 0) return;

        GameObject lastHeart = hearts[hearts.Count - 1];
        hearts.Remove(lastHeart);
        Destroy(lastHeart);
    }
}
