using UnityEngine;
using System.Collections.Generic;

public class CandyManager : MonoBehaviour
{
    public GameObject candyPrefab;
    public List<Vector3> candyPositions;
    private List<GameObject> spawnedCandies = new List<GameObject>();

    void Start()
    {
        SpawnCandies();
    }

    public void SpawnCandies()
    {
        foreach (GameObject candy in spawnedCandies)
        {
            if (candy)
            {
                Destroy(candy);
            }
        }
        spawnedCandies.Clear();

        foreach (Vector3 position in candyPositions)
        {
            GameObject spawnedCandy = Instantiate(candyPrefab, position, Quaternion.identity, transform);
            spawnedCandies.Add(spawnedCandy);
        }
    }
}