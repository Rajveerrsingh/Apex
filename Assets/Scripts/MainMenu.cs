using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string gameSceneName = "Test Room";

    public void StartGame()
    {
        ScoreManager.Reset();
        Time.timeScale = 1f;
        SceneManager.LoadScene(gameSceneName);
    }
} 

