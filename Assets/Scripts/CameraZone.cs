using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZone : MonoBehaviour 
{
    [SerializeField]
    private Transform _cameraPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody.CompareTag("Player"))
        {
            Camera.main.transform.position = _cameraPoint.position;
            Camera.main.transform.localRotation = _cameraPoint.localRotation;
        }
    }
}
