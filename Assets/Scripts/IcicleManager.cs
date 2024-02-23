using UnityEngine;
using System.Collections.Generic;

public class IcicleManager : MonoBehaviour
{
    public GameObject iciclePrefab;
    public List<Vector3> iciclePositions;
    private List<GameObject> spawnedIcicles = new List<GameObject>();

    void Start()
    {
        SpawnIcicles();
    }

    public void SpawnIcicles()
    {
        foreach (GameObject icicle in spawnedIcicles)
        {
            if (icicle)
            {
                Destroy(icicle);
            }
        }
        spawnedIcicles.Clear();

        foreach (Vector3 position in iciclePositions)
        {
            GameObject spawnedIcicle = Instantiate(iciclePrefab, position, Quaternion.identity, transform);
            spawnedIcicles.Add(spawnedIcicle);
        }
    }
}