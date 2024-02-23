using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public CandyManager candyManager;
    public IcicleManager icicleManager;
    public LivesManager livesManager;
    public event Action<int> OnScoreChanged;

    public Vector2 respawnPoint;
    public GameObject penguinPrefab;
    public int score;
    public int lives = 3;

    private GameObject penguinInstance;

    void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        score = 0;
        lives = 3;
        RespawnPenguin();
        if (candyManager) candyManager.SpawnCandies();
        if (icicleManager) icicleManager.SpawnIcicles();
        if (livesManager) livesManager.ResetLives();
    }

    public GameObject GetPenguinInstance()
    {
        return penguinInstance;
    }
    
    public void RespawnPenguin()
    {
        if (lives <= 0) return;

        if (penguinInstance) Destroy(penguinInstance);
        StartCoroutine(RespawnPenguinCoroutine());
    }

    IEnumerator RespawnPenguinCoroutine()
    {
        yield return new WaitForSeconds(1f);
        score = 0;
        candyManager.SpawnCandies();
        icicleManager.SpawnIcicles();
        OnScoreChanged?.Invoke(score);
        penguinInstance = Instantiate(penguinPrefab, (Vector3)respawnPoint + new Vector3(0, 0, 0), Quaternion.identity);
        Camera.main.GetComponent<CameraController>().ResetCamera(penguinInstance.transform);
        livesManager.LoseLife(lives);
    }
    
    public void PenguinDied()
    {
        lives--;
        if (lives > 0)
        {
            RespawnPenguin();
        }
        else
        {
            GameOver();
        }
        livesManager.LoseLife(lives);
    }

    public void ClearLevel()
    {
        Debug.Log("Level Cleared!");
        SceneManager.LoadScene("Scenes/Stage2");
    }

    public void PenguinPickedCandy()
    {
        Debug.Log("Candy picked up!");
        score += 10;
        OnScoreChanged?.Invoke(score);
    }
    
    private void GameOver()
    {
        Debug.Log("Game Over! All lives lost.");
        SceneManager.LoadScene("GameOverScene");
    }
}