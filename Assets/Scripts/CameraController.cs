using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float smoothing = 5f;
    public Vector2 minPosition;
    public Vector2 maxPosition;
    public float startHorizontalOffsetPercentage = 0.4f;
    public float endHorizontalOffsetPercentage = 0.6f;

    private Transform target;

    private void FixedUpdate()
    {
        if (target)
        {
            Vector3 targetCamPos = CalculateCameraPosition(target.position);
            Vector3 smoothPos = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);

            smoothPos.x = Mathf.Clamp(smoothPos.x, minPosition.x, maxPosition.x);
            smoothPos.y = Mathf.Clamp(smoothPos.y, minPosition.y, maxPosition.y);

            transform.position = smoothPos;
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private Vector3 CalculateCameraPosition(Vector3 targetPosition)
    {
        float levelProgress = (targetPosition.x - minPosition.x) / (maxPosition.x - minPosition.x);
        float dynamicHorizontalOffset = Mathf.Lerp(startHorizontalOffsetPercentage, endHorizontalOffsetPercentage, levelProgress);
        float targetX = targetPosition.x + (dynamicHorizontalOffset * (maxPosition.x - minPosition.x));

        Vector3 desiredPosition = new Vector3(targetX, targetPosition.y, transform.position.z);

        return desiredPosition;
    }

    public void ResetCamera(Transform newTarget)
    {
        target = newTarget;
        transform.position = CalculateCameraPosition(target.position);
    }
}