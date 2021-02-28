using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleRenderController : MonoBehaviour
{
    // Collisions
    [SerializeField] GameObject voxelPrefab;
    [SerializeField] float explosionScatter = 1;
    [SerializeField] float numExplosionVoxels = 20;

    // Tricks
    [SerializeField] AnimationCurve rollCurve = AnimationCurve.Linear(0, 0, 1, 1);
    [SerializeField] AnimationCurve yawCurve = AnimationCurve.Linear(0, 0, 1, 1);
    [SerializeField] GameObject rollPivot;
    [SerializeField] GameObject yawPivot;

    private bool isRollActive;
    private float startRollTime;
    private float startRollAngle = 0;
    private float endRollAngle = 360;
    private float fullRollTime = 1;

    private bool isYawActive;
    private float startYawTime;
    private float startYawAngle = 0;
    private float endYawAngle = 360;
    private float fullYawTime = 1;

    private GameSession gameSession;
    private VehiclePhysicsController vehiclePhysicsController;
    
    // Start is called before the first frame update
    void Start()
    {
        gameSession = FindObjectOfType<GameSession>();
        if (gameSession == null)
        {
            throw new System.Exception($"Unable to find object of type {nameof(GameSession)}");
        }

        vehiclePhysicsController = FindObjectOfType<VehiclePhysicsController>();
        if (vehiclePhysicsController == null)
        {
            throw new System.Exception($"Unable to find object of type {nameof(VehiclePhysicsController)}");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (vehiclePhysicsController == null)
        {
            return;
        }

        transform.position = vehiclePhysicsController.transform.position;
        transform.rotation = vehiclePhysicsController.transform.rotation;

        // TODO: Wheels should appear to rotate in sync with the wheel colliders
        // (This is the code originall used by the physics twin)
        //for (int i = 0; i < wheelColliders.Length; i++)
        //{
        //    WheelCollider wheelCollider = wheelColliders[i];
        //    GameObject wheelMesh = wheelMeshes[i];

        //    // Make the mesh track the colliders
        //    Quaternion rotation;
        //    Vector3 position;
        //    wheelCollider.GetWorldPose(out position, out rotation);
        //    wheelMesh.transform.position = position;
        //    wheelMesh.transform.rotation = rotation;
        //}

        if (vehiclePhysicsController.IsAirborne())
        {
            // TODO: Activate separate objects to handle the tricks and get the code out of this class

            if (Input.GetKey(KeyCode.Space))
            {
                StartRoll();
            }
            
            if (Input.GetKey(KeyCode.LeftAlt))
            {
                StartYaw();
            }
        }

        if (isYawActive && false == Input.GetKey(KeyCode.LeftAlt))
        {
            EndYaw();
        }

        if (isRollActive)
        {
            float newRollAngle = Mathf.Lerp(startRollAngle, endRollAngle, rollCurve.Evaluate((Time.time - startRollTime) / fullRollTime));
            if (newRollAngle >= endRollAngle)
            {
                EndRoll();
            }
            else
            {
                Vector3 eulerAngles = transform.eulerAngles;
                float deltaAngle = newRollAngle - eulerAngles.z;
                transform.RotateAround(rollPivot.transform.position, Vector3.forward, deltaAngle);
            }
        }
        
        if (isYawActive)
        {
            float newYawAngle = Mathf.Lerp(startYawAngle, endYawAngle, yawCurve.Evaluate((Time.time - startYawTime) / fullYawTime));
            if (newYawAngle >= endYawAngle)
            {
                // Restart the curve
                startYawTime = Time.time;
            }
            else
            {
                Vector3 eulerAngles = transform.eulerAngles;
                float deltaAngle = newYawAngle - eulerAngles.y;
                transform.RotateAround(yawPivot.transform.position, Vector3.up, deltaAngle);
            }
        }
    }

    void StartRoll()
    {
        if (isRollActive)
        {
            return;
        }

        Debug.Log("StartRoll");

        isRollActive = true;
        startRollTime = Time.time;
    }

    void EndRoll()
    {
        Debug.Log("EndRoll");
        isRollActive = false;
    }

    void StartYaw()
    {
        if (isYawActive)
        {
            return;
        }

        Debug.Log("StartYaw");

        isYawActive = true;
        startYawTime = Time.time;
    }

    void EndYaw()
    {
        Debug.Log("EndYaw");

        isYawActive = false;
    }

    public void OnPlayerDeath()
    {
        for (int i = 0; i < numExplosionVoxels; i++)
        {
            Vector3 particlePosition = transform.position + new Vector3(Random.Range(-explosionScatter, explosionScatter), Random.Range(0, explosionScatter), Random.Range(-explosionScatter, explosionScatter));
            Vector3 eulerAngles = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
            Instantiate(voxelPrefab, particlePosition, Quaternion.Euler(eulerAngles));
        }

        // TODO: Who should be reporting this?
        gameSession.PlayerDied();

        Destroy(vehiclePhysicsController);
        Destroy(gameObject);
    }
}
