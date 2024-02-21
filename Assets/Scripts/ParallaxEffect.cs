using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public Transform cameraTransform;
    public float parallaxFactor;

    private Vector3 previousCameraPosition;
    private float startingPosX;

    void Start()
    {
        if (!cameraTransform)
        {
            cameraTransform = Camera.main.transform;
        }

        startingPosX = transform.position.x;
        previousCameraPosition = cameraTransform.position;
    }

    void LateUpdate()
    {
        if (cameraTransform)
        {
            float deltaX = cameraTransform.position.x - previousCameraPosition.x;

            float parallaxMovement = deltaX * parallaxFactor;
            Vector3 newPosition = new Vector3(startingPosX + parallaxMovement, transform.position.y, transform.position.z);

            transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime);

            previousCameraPosition = cameraTransform.position;
        }
    }

}