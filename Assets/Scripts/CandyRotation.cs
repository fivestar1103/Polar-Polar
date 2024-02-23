using UnityEngine;
using System.Collections;

public class CandyRotation : MonoBehaviour
{
    public float lowAngleBound = -15f;
    public float highAngleBound = 15f;
    public float rotationSpeed = 2f;

    private float targetAngle;
    private bool rotatingToLow;

    void Start()
    {
        float startingAngle = Random.Range(lowAngleBound, highAngleBound);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, startingAngle);

        rotatingToLow = (Random.value > 0.5f);
        targetAngle = rotatingToLow ? lowAngleBound : highAngleBound;

        StartCoroutine(RotateCandy());
    }

    IEnumerator RotateCandy()
    {
        while (true)
        {
            targetAngle = rotatingToLow ? lowAngleBound : highAngleBound;

            float zRotation = Mathf.MoveTowardsAngle(transform.eulerAngles.z, targetAngle, Time.deltaTime * rotationSpeed);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, zRotation);

            if (Mathf.Abs(zRotation - targetAngle) < 0.1f)
            {
                rotatingToLow = !rotatingToLow;
            }

            yield return null;
        }
    }
}