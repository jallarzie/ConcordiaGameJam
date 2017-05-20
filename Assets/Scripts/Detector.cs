using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour {

    [SerializeField]
    private Transform _detector;

    private Transform _target;

    private void Update()
    {
        if (_target != null)
        {
            _detector.LookAt(_target);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        _target = other.attachedRigidbody.transform;
    }

    private void OnTriggerExit(Collider other)
    {
        _target = null;
        _detector.LookAt(Vector3.zero);
    }
}
