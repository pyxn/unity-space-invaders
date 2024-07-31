using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;
    private void Start()
    {
        GameManager.Instance.OnStateChanged += HandleStateChanged;
        restartButton.onClick.AddListener(RestartGame);
        mainMenuButton.onClick.AddListener(ReturnToMainMenu);
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnStateChanged -= HandleStateChanged;
        }
    }

    private void HandleStateChanged(GameManager.GameState newState)
    {
        if (newState == GameManager.GameState.GameOver)
        {
            gameObject.SetActive(true);
        }
    }

    private void RestartGame() 
    {
        GameManager.Instance.ChangeState(GameManager.GameState.Loading);
    }
    private void ReturnToMainMenu()
    {
        Debug.Log("MainMenu button clicked");
        GameManager.Instance.ReturnToMainMenu();

        mainMenuButton.enabled = false;
    }
}