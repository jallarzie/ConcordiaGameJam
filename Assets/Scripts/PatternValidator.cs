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
    private MoveAction[] _bunnyPattern;

    [SerializeField]
    private MoveAction[] _frogPattern;

    [SerializeField]
    private MoveAction[] _fishPattern;

    private int _currentAction;

    public static PatternValidator Instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        Instance = this;
    }

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
        switch(form)
        {
            case Form.Bunny:
                return _bunnyPattern[_currentAction];
            case Form.Frog:
                return _frogPattern[_currentAction];
            case Form.Fish:
                return _fishPattern[_currentAction];
        }

        return MoveAction.NoHop;
    }
}
