using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    private TMP_Text label;
    private int shownScore = -1;

    private void Awake()
    {
        label = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        // Only rewrite the text when the score changes.
        if (ScoreManager.Score != shownScore)
        {
            shownScore = ScoreManager.Score;
            label.text = "Score: " + shownScore;
        }
    }
}