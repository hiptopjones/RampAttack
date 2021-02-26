using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereController : MonoBehaviour
{
    [SerializeField] float speed = 20;

    Rigidbody rigidbody;

    bool isRunning = false;

    bool isAirborne = false;
    float startAirTime;
    bool isGrounded = false;
    float startGroundTime;
    bool isRamping = false;
    float startRampTime;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (false == isRunning)
        {
            return;
        }
    }

    void FixedUpdate()
    {
        if (false == isRunning)
        {
            return;
        }

        Debug.Log("Before: " + rigidbody.velocity);
        if (!IsAirborne())
        {
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y, speed);
        }
        Debug.Log("After: " + rigidbody.velocity);
    }
    public bool IsRunning()
    {
        return isRunning;
    }

    public void StartDriving()
    {
        isRunning = true;
        rigidbody.velocity = Vector3.forward * speed;
    }

    public bool IsAirborne()
    {
        return isAirborne;
    }

    public bool IsGrounded()
    {
        return isGrounded;
    }

    public bool IsRamping()
    {
        return isRamping;
    }

    public bool IsTricking
    {
        get { return false; }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Obstacle")
        {
            GameSession gameSession = FindObjectOfType<GameSession>();
            gameSession.PlayerDied();

            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ramp" && false == isRamping)
        {
            Debug.Log("OnCollisionEnter: Enter the RAMP");
            isRamping = true;
            startRampTime = Time.time;
        }

        if (collision.gameObject.tag == "Ground" && false == isGrounded)
        {
            Debug.Log("OnCollisionEnter: Enter the GROUND");
            isGrounded = true;
            startGroundTime = Time.time;
        }

        if ((isRamping || isGrounded) && isAirborne)
        {
            Debug.Log("OnCollisionEnter: Leave the AIR");
            isAirborne = false;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Ramp" && false == isRamping)
        {
            Debug.Log("OnCollisionStay: Enter the RAMP");
            isRamping = true;
            startRampTime = Time.time;
        }

        if (collision.gameObject.tag == "Ground" && false == isGrounded)
        {
            Debug.Log("OnCollisionStay: Enter the GROUND");
            isGrounded = true;
            startGroundTime = Time.time;
        }

        if ((isRamping || isGrounded) && isAirborne)
        {
            Debug.Log("OnCollisionStay: Leave the AIR");
            isAirborne = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground" && isGrounded)
        {
            Debug.Log("OnCollisionExit: Leave the GROUND");
            isGrounded = false;
        }

        if (collision.gameObject.tag == "Ramp" && isRamping)
        {
            Debug.Log("OnCollisionExit: Leave the RAMP");
            isRamping = false;
        }

        if ((false == isRamping && false == isGrounded) && false == isAirborne)
        {
            Debug.Log("OnCollisionExit: Enter the AIR");
            isAirborne = true;
            startAirTime = Time.time;
        }
    }
}
