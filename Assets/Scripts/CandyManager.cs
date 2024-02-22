using UnityEngine;
using System.Collections.Generic;

public class CandyManager : MonoBehaviour
{
    public GameObject candyPrefab; // Assign your Candy prefab in the Inspector
    public List<Vector3> candyPositions; // Set this list in the Inspector
    private List<GameObject> spawnedCandies = new List<GameObject>(); // Keep track of spawned candies

    void Start()
    {
        SpawnCandies();
    }

    // This method spawns candies at all specified positions
    public void SpawnCandies()
    {
        // First, clear existing candies
        foreach (GameObject candy in spawnedCandies)
        {
            if (candy)
            {
                Destroy(candy);
            }
        }
        spawnedCandies.Clear();

        // Loop through each position and instantiate a candy prefab
        foreach (Vector3 position in candyPositions)
        {
            GameObject spawnedCandy = Instantiate(candyPrefab, position, Quaternion.identity, transform);
            spawnedCandies.Add(spawnedCandy);
        }
    }
}