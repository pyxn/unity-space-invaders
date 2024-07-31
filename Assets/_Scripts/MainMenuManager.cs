using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    private Button startButton;

    private void Start()
    {
        startButton = GetComponent<Button>();
        startButton.onClick.AddListener(StartGame);
    }

    private void StartGame()
    {
        GameManager.Instance.ChangeState(GameManager.GameState.Loading);
    }
}