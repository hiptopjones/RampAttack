using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveController : MonoBehaviour
{
    [SerializeField] Vector3 centerOfMass;
    [SerializeField] WheelCollider[] wheelColliders;
    [SerializeField] GameObject[] wheelMeshes;

    [SerializeField] float speed = 25;
    [SerializeField] float pitchCorrectionTime = 5;
    [SerializeField] float targetPitchAngle = -10;

    Rigidbody vehicleRigidbody;

    float lastTimeGrounded = 0;

    void Start()
    {
        vehicleRigidbody = GetComponentInParent<Rigidbody>();

        // Increase stability
        vehicleRigidbody.centerOfMass = centerOfMass;
        vehicleRigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

        // Start motion
        vehicleRigidbody.velocity = Vector3.forward * speed;
    }

    void Update()
    {
        for (int i = 0; i < wheelColliders.Length; i++)
        {
            WheelCollider wheelCollider = wheelColliders[i];
            GameObject wheelMesh = wheelMeshes[i];

            // Make the mesh track the colliders
            Quaternion rotation;
            Vector3 position;
            wheelCollider.GetWorldPose(out position, out rotation);
            wheelMesh.transform.position = position;
            wheelMesh.transform.rotation = rotation;
        }
    }

    void FixedUpdate()
    {
        if (IsAirborne())
        {
            float elapsedAirTime = Time.time - lastTimeGrounded;
            float fractionOfCorrection = Mathf.Clamp(elapsedAirTime / pitchCorrectionTime, 0, 1);

            // Straighten out the rotation so we land properly
            Vector3 eulerAngles = transform.eulerAngles;
            float newPitchAngle = Mathf.LerpAngle(eulerAngles.x, targetPitchAngle, fractionOfCorrection);
            transform.rotation = Quaternion.Euler(newPitchAngle, 0, 0);
        }
        else if (IsGrounded())
        {
            lastTimeGrounded = Time.time;

            // Maintain speed when grounded
            vehicleRigidbody.velocity = Vector3.forward * speed;

            // Remove any stray rotations (why isn't this fixed by rigidbody constraints?)
            transform.rotation = Quaternion.identity;

            // Remove any stray position drift (why isn't this fixed by rigidbody constraints?)
            transform.position = new Vector3(0, transform.position.y, transform.position.z);
        }
    }

    private bool IsGrounded()
    {
        // Determine if all four wheels are on the ground
        foreach (WheelCollider wheelCollider in wheelColliders)
        {
            WheelHit hit;
            if (false == wheelCollider.GetGroundHit(out hit))
            {
                return false;
            }

            if (hit.collider.tag != "Ground")
            {
                return false;
            }
        }

        return true;
    }

    private bool IsAirborne()
    {
        // Determine if all four wheels are off the ground
        foreach (WheelCollider wheelCollider in wheelColliders)
        {
            WheelHit hit;
            if (wheelCollider.GetGroundHit(out hit))
            {
                return false;
            }
        }

        return true;
    }
}
