using UnityEngine;
using System.Collections.Generic;

public class IcicleManager : MonoBehaviour
{
    public GameObject iciclePrefab; // Assign your Candy prefab in the Inspector
    public List<Vector3> iciclePositions; // Set this list in the Inspector
    private List<GameObject> spawnedIcicles = new List<GameObject>(); // Keep track of spawned candies

    void Start()
    {
        SpawnIcicles();
    }

    // This method spawns candies at all specified positions
    public void SpawnIcicles()
    {
        // First, clear existing candies
        foreach (GameObject icicle in spawnedIcicles)
        {
            if (icicle)
            {
                Destroy(icicle);
            }
        }
        spawnedIcicles.Clear();

        // Loop through each position and instantiate a candy prefab
        foreach (Vector3 position in iciclePositions)
        {
            GameObject spawnedIcicle = Instantiate(iciclePrefab, position, Quaternion.identity, transform);
            spawnedIcicles.Add(spawnedIcicle);
        }
    }
}