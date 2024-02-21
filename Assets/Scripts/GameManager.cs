using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Vector2 respawnPoint;
    public GameObject penguinPrefab;
    public int score;

    private GameObject penguinInstance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        RespawnPenguin();
    }

    public void RespawnPenguin()
    {
        if (penguinInstance) Destroy(penguinInstance);
        StartCoroutine(RespawnPenguinCoroutine());
    }

    IEnumerator RespawnPenguinCoroutine()
    {
        yield return new WaitForSeconds(1f); // Delay for 1 second before respawning
        penguinInstance = Instantiate(penguinPrefab, (Vector3)respawnPoint + new Vector3(0, 0, 0), Quaternion.identity);
        Camera.main.GetComponent<CameraController>().ResetCamera(penguinInstance.transform);
    }

    public void ClearLevel()
    {
        Debug.Log("Level Cleared!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void PenguinPickedCandy()
    {
        Debug.Log("Candy picked up!");
        score++;
    }
}