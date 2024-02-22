using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public Transform cameraTransform;
    public float parallaxFactor;

    private Vector3 previousCameraPosition;

    void Start()
    {
        if (!cameraTransform)
        {
            cameraTransform = Camera.main.transform;
        }

        previousCameraPosition = cameraTransform.position;
    }

    void LateUpdate()
    {
        if (cameraTransform)
        {
            float deltaX = cameraTransform.position.x - previousCameraPosition.x;

            float parallaxMovement = deltaX * parallaxFactor;
            Vector3 currentPosition = transform.position;
            Vector3 newPosition = new Vector3(currentPosition.x + parallaxMovement, transform.position.y, transform.position.z);

            transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime);

            previousCameraPosition = cameraTransform.position;
        }
    }

}