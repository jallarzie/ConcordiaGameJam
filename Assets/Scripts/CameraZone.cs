using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZone : MonoBehaviour 
{
    [SerializeField]
    private Transform _cameraPoint;

    private Camera _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody.CompareTag("Player"))
        {
            _mainCamera.transform.position = _cameraPoint.position;
            _mainCamera.transform.localRotation = _cameraPoint.localRotation;
        }

        // Change PlayerOriginalPosition and Mesh
        GameManager.instance.ChangeParametersNewRoom();


    }
}
