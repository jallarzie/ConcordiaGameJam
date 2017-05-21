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
        _currentAction = -1;
        StartCoroutine(PatternLoop());
    }

    private IEnumerator PatternLoop()
    {
        while (true)
        {
            _currentAction = (_currentAction + 1) % (_patternLenght + 1);

            GameObject[] guards = GameManager.instance.guards;

            for (int i = 0; i < guards.Length; i++)
            {
                AIMovement ai = guards[i].GetComponent<AIMovement>();
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

        return null; 
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
                return _bunnyPattern[_currentAction];
            case Form.Frog:
                return _frogPattern[_currentAction];
            case Form.Fish:
                return _fishPattern[_currentAction];
        }

        return MoveAction.NoHop;
    }
}
