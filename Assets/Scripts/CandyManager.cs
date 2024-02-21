using UnityEngine;
using System.Collections.Generic;

public class CandyManager : MonoBehaviour
{
    public GameObject candyPrefab; // Assign your Candy prefab in the Inspector
    public List<Vector3> candyPositions; // Set this list in the Inspector

    void Start()
    {
        // Loop through each position and instantiate a candy prefab
        foreach (Vector3 position in candyPositions)
        {
            Instantiate(candyPrefab, position, Quaternion.identity, transform);
        }
    }
}