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
    [SerializeField] float followRotation = 0f;
    [SerializeField] float rotationDamping = 2.0f;

    float rotationOffset = 0;
    float heightOffset = 0;
    float distanceOffset = 0;

    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            rotationOffset = Mathf.Min(rotationOffset + 360f * Time.deltaTime, 180f);
        }
        else if (Input.GetKey(KeyCode.W))
        {
            rotationOffset = Mathf.Max(rotationOffset - 360f * Time.deltaTime, -180f);
        }

        if (Input.GetKey(KeyCode.A))
        {
            heightOffset = Mathf.Min(heightOffset + 5 * Time.deltaTime, 20);
        }
        else if (Input.GetKey(KeyCode.Z))
        {
            heightOffset = Mathf.Max(heightOffset - 5 * Time.deltaTime, 0);
        }

        if (Input.GetKey(KeyCode.X))
        {
            distanceOffset = Mathf.Min(distanceOffset + 5 * Time.deltaTime, 20);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            distanceOffset = Mathf.Max(distanceOffset - 5 * Time.deltaTime, 0);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            rotationOffset = 0;
            heightOffset = 0;
            distanceOffset = 0;
        }
    }

    void LateUpdate()
    {
        
        Quaternion currentRotation = Quaternion.identity;
        if (enableFollowRotation)
        {
            float wantedRotationAngle = target.transform.eulerAngles.y;
            float currentRotationAngle = transform.eulerAngles.y;
            currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle + rotationOffset, rotationDamping * Time.deltaTime);
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
