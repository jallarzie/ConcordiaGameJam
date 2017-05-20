using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Form
{
    Bunny,
    Frog,
    Fish
}

public enum MoveAction
{
    Hop,
    NoHop
}

public class PatternValidator : MonoBehaviour {

    [SerializeField]
    private int _patternLenght;
    [SerializeField]
    private float _interval;

    [SerializeField]
    private MoveAction[][] _patterns;

    private int _currentAction;

    private void Start()
    {
        _currentAction = 0;

        StartCoroutine(PatternLoop());
    }

    private IEnumerator PatternLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(_interval);

            _currentAction = (_currentAction + 1) % _patternLenght;
        }
    }

    public MoveAction GetCurrentActionForForm(Form form)
    {
        return _patterns[(int)form][_currentAction];
    }
}
