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
    private int _maxStrikes = 6;

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
            if (_strikes < 0)
            {
                _light.color = _spotColors[0];
            }
            else if (_strikes >= _spotColors.Length)
            {
                _light.color = _spotColors[_spotColors.Length - 1];
            }
            else
            {
                _light.color = _spotColors[_strikes];
            }
        } else
        {
            _strikes--;
            if (_strikes < 0)
            {
                _light.color = _spotColors[0];
            } else if (_strikes >= _spotColors.Length)
            {
                _light.color = _spotColors[_spotColors.Length-1];
            } else
            {
                _light.color = _spotColors[_strikes];
            }
        }
        if (_strikes >= _maxStrikes)
        {
            _strikes = 0;
            GameManager.instance.ResetPlayer();
        }
    }
}
