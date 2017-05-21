using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour {

    [SerializeField]
    private Transform _detector;
    [SerializeField]
    private Light _light;

    [SerializeField]
    private Color[] _spotColors;

    [SerializeField]
    private PatternValidator _patternValidator;

    private PlayerController _target;

    private int _strikes = 0;

    private void Update()
    {
        if (_target != null)
        {
            _detector.LookAt(_target.transform);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody.CompareTag("Player"))
        {
            _target = other.attachedRigidbody.GetComponent<PlayerController>();
            _target.OnAction += OnTargetAction;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.attachedRigidbody.CompareTag("Player"))
        {
            _target.OnAction -= OnTargetAction;
            _target = null;
            _detector.LookAt(Vector3.zero);
            _strikes = 0;
            _light.color = _spotColors[0];
        }
    }

    private void OnTargetAction(MoveAction action)
    {
        if (action != _patternValidator.GetCurrentActionForForm(_target.CurrentForm))
        {
            _strikes++;
            _light.color = _spotColors[_strikes < _spotColors.Length ? _strikes : _spotColors.Length - 1];
        }
    }
}
