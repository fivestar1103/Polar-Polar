using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    // Call this function on Try Again button click
    public void RetryGame()
    {
        // Assuming "GameScene" is the name of your main game scene
        SceneManager.LoadScene("GameScene");
    }
}