using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] Transform targetCamera;

    void LateUpdate()
    {
        // TODO: Why this?
        // https://www.youtube.com/watch?v=BLfNP4Sc_iA
        transform.LookAt(transform.position + targetCamera.forward);        
    }
}
