using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private TextMeshProUGUI scoreText;

    void Start()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
        if (GameManager.instance)
        {
            GameManager.instance.OnScoreChanged += UpdateScoreText;
            scoreText.text = $"Score: {GameManager.instance.score}";
        }
        else
        {
            Debug.LogError("GameManager instance not found.");
        }
    }

     public void UpdateScoreText(int newScore)
     {
         if (scoreText)
         {
             scoreText.text = $"Score: {newScore}";
         }
     }

     void OnDestroy()
     {
         GameManager.instance.OnScoreChanged -= UpdateScoreText;
     }
}
