using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Start()
    {
        ScoreManager.Instance.OnScoreChanged += UpdateScoreDisplay;
        UpdateScoreDisplay(ScoreManager.Instance.Score);
    }

    private void OnDestroy()
    {
        ScoreManager.Instance.OnScoreChanged -= UpdateScoreDisplay;
    }

    private void UpdateScoreDisplay(int newScore)
    {
        scoreText.text = $"{newScore}";
    }
}