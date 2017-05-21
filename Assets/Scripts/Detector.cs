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

    private bool streak = false;

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
            streak = false;
        }
    }

    private void OnTargetAction(MoveAction action)
    {
        Debug.Log(action);

        if (action != _patternValidator.GetCurrentActionForForm(_target.CurrentForm))
        {
            _strikes = Mathf.Min(_maxStrikes, _strikes + 1);
            _light.color = _spotColors[Mathf.Min(_strikes, _spotColors.Length - 1)];
            streak = false;
        }
        else if (streak)
        {
            _strikes = Mathf.Max(0, _strikes - 1);
            _light.color = _spotColors[_strikes];
        }
        else
        {
            streak = true;
        }
        if (_strikes >= _maxStrikes)
        {
            _strikes = 0;
            GameManager.instance.ResetPlayer();
        }
    }
}
