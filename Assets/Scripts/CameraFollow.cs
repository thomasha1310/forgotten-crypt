using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    [SerializeField] private Transform target;
    [SerializeField] private float verticalOffset = 0f;
    [SerializeField] private float smoothingHorizontal = 0.25f;
    [SerializeField] private bool followVertical = true;
    [SerializeField] private float smoothingUp = 0.25f;
    [SerializeField] private float smoothingDown = 0.25f;

    [SerializeField] private float minCameraDist = 0.05f;

    private void Awake()
    {
        Debug.Log($"{GetType().Name} Initialized");

        Vector3 cameraPosition = transform.position;
        Vector3 targetPosition = target.position + new Vector3(0, verticalOffset, 0);
        if (!followVertical)
        {
            targetPosition.y = cameraPosition.y;
        }
        targetPosition.z = cameraPosition.z;
        transform.position = targetPosition;
    }

    private void Update()
    {

        float intHorizontal = 1.0f / Mathf.Max(0.00001f, smoothingHorizontal) * Time.deltaTime;
        float intUp = 1.0f / Mathf.Max(0.00001f, smoothingUp) * Time.deltaTime;
        float intDown = 1.0f / Mathf.Max(0.00001f, smoothingDown) * Time.deltaTime;

        Vector3 newPosition = Vector3.zero;
        Vector3 cameraPosition = transform.position;
        Vector3 targetPosition = target.position + new Vector3(0, verticalOffset, 0);
        if (!followVertical)
        {
            targetPosition.y = cameraPosition.y;
        }
        newPosition.z = cameraPosition.z;
        targetPosition.z = cameraPosition.z;

        float cameraDist = (cameraPosition - targetPosition).magnitude;

        if (cameraDist > minCameraDist)
        {
            if (cameraPosition.y < targetPosition.y)
            {
                newPosition.y = Mathf.Lerp(cameraPosition.y, targetPosition.y, intUp);
            }
            else
            {
                newPosition.y = Mathf.Lerp(cameraPosition.y, targetPosition.y, intDown);
            }

            newPosition.x = Mathf.Lerp(cameraPosition.x, targetPosition.x, intHorizontal);

            transform.position = newPosition;
        }

    }
}
