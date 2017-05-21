using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Form
{
    Paper,
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

    public int _patternLenght;
    [SerializeField]
    private float _interval;

    [SerializeField]
    private MoveAction[] _bunnyPattern;

    [SerializeField]
    private MoveAction[] _frogPattern;

    [SerializeField]
    private MoveAction[] _fishPattern;

    [SerializeField]
    private AIMovement[] _guards;

    private int _currentAction;

    private void Start()
    {
        _currentAction = -1;
        StartCoroutine(PatternLoop());
    }

    private IEnumerator PatternLoop()
    {
        while (true)
        {
            _currentAction = (_currentAction + 1) % (_patternLenght + 1);

            for (int i = 0; i < _guards.Length; i++)
            {
                AIMovement ai = _guards[i];
                if (GetCurrentActionForForm(ai.form) == MoveAction.Hop)
                {
                    ai.NextAction();
                }
            }

            yield return new WaitForSeconds(_interval);
        }
    }

    public MoveAction[] GetPatternForForm(Form form)
    {
        switch(form)
        {
            case Form.Bunny:
                return _bunnyPattern;
            case Form.Frog:
                return _frogPattern;
            case Form.Fish:
                return _fishPattern;
        }

        return new MoveAction[0]; 
    }

    public MoveAction GetCurrentActionForForm(Form form)
    {
        if (_currentAction == _patternLenght)
        {
            return MoveAction.NoHop;
        }

        switch(form)
        {
            case Form.Bunny:
                if (_currentAction < _bunnyPattern.Length)
                    return _bunnyPattern[_currentAction];
                else
                    return MoveAction.NoHop;
            case Form.Frog:
                if (_currentAction < _frogPattern.Length)
                    return _frogPattern[_currentAction];
                else
                    return MoveAction.NoHop;
            case Form.Fish:
                if (_currentAction < _fishPattern.Length)
                    return _fishPattern[_currentAction];
                else
                    return MoveAction.NoHop;
        }

        return MoveAction.NoHop;
    }
}
