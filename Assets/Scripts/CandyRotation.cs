using UnityEngine;
using System.Collections;

public class CandyRotation : MonoBehaviour
{
    public float lowAngleBound = -15f; // Lower bound of the rotation in degrees
    public float highAngleBound = 15f; // Upper bound of the rotation in degrees
    public float rotationSpeed = 2f; // The speed of the rotation

    private float targetAngle;
    private bool rotatingToLow;

    void Start()
    {
        // Set a random starting angle between the bounds
        float startingAngle = Random.Range(lowAngleBound, highAngleBound);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, startingAngle);

        // Randomly choose the starting direction of rotation
        rotatingToLow = (Random.value > 0.5f);
        targetAngle = rotatingToLow ? lowAngleBound : highAngleBound;

        StartCoroutine(RotateCandy());
    }

    IEnumerator RotateCandy()
    {
        while (true)
        {
            // Determine the direction of rotation based on the target angle
            targetAngle = rotatingToLow ? lowAngleBound : highAngleBound;

            // Rotate the candy towards the target angle
            float zRotation = Mathf.MoveTowardsAngle(transform.eulerAngles.z, targetAngle, Time.deltaTime * rotationSpeed);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, zRotation);

            // Check if the rotation has reached the target angle within a small threshold
            if (Mathf.Abs(zRotation - targetAngle) < 0.1f)
            {
                // If it has, switch the target angle
                rotatingToLow = !rotatingToLow;
            }

            yield return null;
        }
    }
}