using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public CandyManager candyManager;
    public IcicleManager icicleManager;
    public LivesManager livesManager;

    public Vector2 respawnPoint;
    public GameObject penguinPrefab;
    public int score;
    public int lives = 3;

    private GameObject penguinInstance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        SceneManager.sceneLoaded += OnSceneLoaded; // Subscribe to the sceneLoaded event
    }
    
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Unsubscribe to prevent memory leaks
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reset game state here
        score = 0;
        lives = 3; // Reset lives if needed
        RespawnPenguin(); // Respawn the penguin
        // Make sure to also reset other necessary components like candyManager and icicleManager
        if (candyManager != null) candyManager.SpawnCandies();
        if (icicleManager != null) icicleManager.SpawnIcicles();
        if (livesManager != null) livesManager.ResetLives(); // You will need to implement this method
    }

    public void RespawnPenguin()
    {
        if (lives <= 0) return; // Don't respawn if no lives left

        if (penguinInstance) Destroy(penguinInstance);
        StartCoroutine(RespawnPenguinCoroutine());
    }

    IEnumerator RespawnPenguinCoroutine()
    {
        yield return new WaitForSeconds(1f); // Delay for 1 second before respawning
        score = 0; // Reset score or handle as needed
        candyManager.SpawnCandies();
        icicleManager.SpawnIcicles();
        penguinInstance = Instantiate(penguinPrefab, (Vector3)respawnPoint + new Vector3(0, 0, 0), Quaternion.identity);
        Camera.main.GetComponent<CameraController>().ResetCamera(penguinInstance.transform);
        livesManager.LoseLife(lives); // Update the UI each time the penguin respawns
    }
    
    public void PenguinDied()
    {
        lives--;
        if (lives > 0)
        {
            RespawnPenguin(); // Respawn the penguin if lives are left
        }
        else
        {
            GameOver(); // End the game if no lives are left
        }
        livesManager.LoseLife(lives); // Update the UI whenever a life is lost
    }

    public void ClearLevel()
    {
        Debug.Log("Level Cleared!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Or load the next level
        // Consider what should happen to lives when the level is cleared.
    }

    public void PenguinPickedCandy()
    {
        Debug.Log("Candy picked up!");
        score++;
        // Update the score in the UI here if you have a UI element for it
    }
    
    private void GameOver()
    {
        Debug.Log("Game Over! All lives lost.");
        // Here you could load a game over scene or display a game over screen
        SceneManager.LoadScene("GameOverScene"); // Assuming you have a game over scene
    }
}