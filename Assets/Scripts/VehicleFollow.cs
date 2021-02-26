using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleFollow : MonoBehaviour
{
    [SerializeField] float followDistance = 8f;

    [SerializeField] bool enableFollowHeight = false;
    [SerializeField] float followHeight = 2.5f;

    [SerializeField] bool enableFollowRotation = false;
    [SerializeField] float followRotationAngle = 0f;
    [SerializeField] float rotationDamping = 2.0f;

    float rotationOffset = 0;
    float heightOffset = 0;
    float distanceOffset = 0;

    VehiclePhysicsController vehiclePhysicsController;

    void Start()
    {
        vehiclePhysicsController = FindObjectOfType<VehiclePhysicsController>();
    }

    void LateUpdate()
    {
        if (vehiclePhysicsController == null)
        {
            return;
        }

        Quaternion currentRotation = transform.rotation;
        if (enableFollowRotation)
        {
            float wantedRotationAngle = vehiclePhysicsController.transform.eulerAngles.y;
            float currentRotationAngle = transform.eulerAngles.y;
            currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle + followRotationAngle + rotationOffset, rotationDamping * Time.deltaTime);
            currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);
        }

        float currentHeight = followHeight;
        if (enableFollowHeight)
        {
            float wantedHeight = vehiclePhysicsController.transform.position.y;
            currentHeight = wantedHeight + followHeight + heightOffset;
        }

        transform.position = vehiclePhysicsController.transform.position;
        transform.position -= currentRotation * (Vector3.forward * (followDistance + distanceOffset));
        transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);

        transform.LookAt(vehiclePhysicsController.transform);
    }
}
