using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleFollow : MonoBehaviour
{
    [SerializeField] GameObject target;

    [SerializeField] float followDistance = 8f;

    [SerializeField] bool enableFollowHeight = false;
    [SerializeField] float followHeight = 2.5f;

    [SerializeField] bool enableFollowRotation = false;
    [SerializeField] float followRotationAngle = 0f;
    [SerializeField] float rotationDamping = 2.0f;

    float rotationOffset = 0;
    float heightOffset = 0;
    float distanceOffset = 0;

    void LateUpdate()
    {        
        Quaternion currentRotation = Quaternion.identity;
        if (enableFollowRotation)
        {
            float wantedRotationAngle = target.transform.eulerAngles.y;
            float currentRotationAngle = transform.eulerAngles.y;
            currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle + followRotationAngle + rotationOffset, rotationDamping * Time.deltaTime);
            currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);
        }

        float currentHeight = followHeight;
        if (enableFollowHeight)
        {
            float wantedHeight = target.transform.position.y;
            currentHeight = wantedHeight + followHeight + heightOffset;
        }

        transform.position = target.transform.position;
        transform.position -= currentRotation * (Vector3.forward * (followDistance + distanceOffset));
        transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);

        transform.LookAt(target.transform);
    }
}
