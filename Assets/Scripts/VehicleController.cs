using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
    [SerializeField] Vector3 centerOfMass;
    [SerializeField] WheelCollider[] wheelColliders;
    [SerializeField] GameObject[] wheelMeshes;

    [SerializeField] float speed = 25;
    [SerializeField] float pitchCorrectionTime = 5;
    [SerializeField] float targetPitchAngle = 0;

    [SerializeField] GameObject voxelPrefab;
    [SerializeField] float explosionScatter = 1;
    [SerializeField] float numExplosionVoxels = 20;

    Rigidbody vehicleRigidbody;

    float lastSurfaceTime = 0;

    bool isRunning = false;

    List<GameObject> particles;

    void Start()
    {
        vehicleRigidbody = GetComponentInParent<Rigidbody>();

        // Increase stability
        vehicleRigidbody.centerOfMass = centerOfMass;
        vehicleRigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
    }

    void Update()
    {
        if (false == isRunning)
        {
            return;
        }

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
        if (false == isRunning)
        {
            return;
        }

        if (IsAirborne())
        {
            float elapsedAirTime = Time.time - lastSurfaceTime;
            float fractionOfCorrection = Mathf.Clamp(elapsedAirTime / pitchCorrectionTime, 0, 1);

            // Straighten out the rotation so we land properly
            Vector3 eulerAngles = transform.eulerAngles;
            float newPitchAngle = Mathf.LerpAngle(eulerAngles.x, targetPitchAngle, fractionOfCorrection);
            transform.rotation = Quaternion.Euler(newPitchAngle, 0, 0);
        }
        else
        {
            lastSurfaceTime = Time.time;

            if (IsGrounded())
            {
                // Maintain speed when grounded
                vehicleRigidbody.velocity = Vector3.forward * speed;

                // Remove any stray rotations (why isn't this fixed by rigidbody constraints?)
                transform.rotation = Quaternion.identity;

                // Remove any stray position drift (why isn't this fixed by rigidbody constraints?)
                transform.position = new Vector3(0, transform.position.y, transform.position.z);
            }
        }
    }

    public bool IsGrounded()
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

    public bool IsAirborne()
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

    public bool IsRunning()
    {
        return isRunning;
    }

    public void StartDriving()
    {
        isRunning = true;

        // Start motion
        vehicleRigidbody.velocity = Vector3.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("VehicleController: OnTriggerEnter: " + other.name);

        if (other.tag == "Obstacle")
        {
            for (int i = 0; i < numExplosionVoxels; i++)
            {
                Vector3 particlePosition = transform.position + new Vector3(Random.Range(-explosionScatter, explosionScatter), Random.Range(0, explosionScatter), Random.Range(-explosionScatter, explosionScatter));
                Vector3 eulerAngles = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
                Instantiate(voxelPrefab, particlePosition, Quaternion.Euler(eulerAngles));
            }

            GameSession gameSession = FindObjectOfType<GameSession>();
            gameSession.PlayerDied();

            Destroy(gameObject);
        }
    }
}
