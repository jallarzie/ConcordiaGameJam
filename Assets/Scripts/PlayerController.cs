using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;
using System;

public enum Direction
{
    None,
    Up,
    Down,
    Left,
    Right
}

public class PlayerController : MonoBehaviour 
{
    public Action<MoveAction> OnAction = delegate{};

    [SerializeField]
    private float _moveDistance;
    [SerializeField]
    private float _moveInterval;

    [SerializeField]
    private GameObject _playerModel;

    [SerializeField]
    private Transform _raycastPoint;
	
    private bool _isMoving;
    private Direction _lastDirection = Direction.None;
    private bool _acceptingInput;

    public Vector3 origin;
    public Vector3 destination;

    public Form CurrentForm { get; private set; }

    private void Start()
    {
        CurrentForm = Form.Bunny;
        StartCoroutine(InputLoop());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || InputManager.ActiveDevice.DPad.Up.WasPressed)
        {
            _lastDirection = Direction.Up;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) || InputManager.ActiveDevice.DPad.Down.WasPressed)
        {
            _lastDirection = Direction.Down;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) || InputManager.ActiveDevice.DPad.Left.WasPressed)
        {
            _lastDirection = Direction.Left;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) || InputManager.ActiveDevice.DPad.Right.WasPressed)
        {
            _lastDirection = Direction.Right;
        }
    }

    private IEnumerator InputLoop() 
    {
        float inputTimer = 0f;

        while (true)
        {
            if (_lastDirection != Direction.None)
            {
                Direction direction = _lastDirection;
                _lastDirection = Direction.None;

                OnAction(MoveAction.Hop);

                switch (direction)
                {
                    case Direction.Up:
                        yield return StartCoroutine(DoMove(Vector3.forward, _moveInterval - inputTimer));
                        break;
                    case Direction.Down:
                        yield return StartCoroutine(DoMove(Vector3.back, _moveInterval - inputTimer));
                        break;
                    case Direction.Left:
                        yield return StartCoroutine(DoMove(Vector3.left, _moveInterval - inputTimer));
                        break;
                    case Direction.Right:
                        yield return StartCoroutine(DoMove(Vector3.right, _moveInterval - inputTimer));
                        break;
                }
            }
            else
            {
                while (inputTimer < _moveInterval / 2f)
                {
                    if (_lastDirection != Direction.None)
                    {
                        break;
                    }

                    yield return null;

                    inputTimer += Time.deltaTime;
                }

                if (_lastDirection == Direction.None)
                {
                    OnAction(MoveAction.NoHop);
                    yield return new WaitForSeconds(_moveInterval - inputTimer);
                }
            }

            inputTimer = 0f;
        }
	}

    public void Move(Vector3 direction)
    {
        if (direction.y > 0f)
        {
            _lastDirection = Direction.Up;
        }
        if (direction.y < 0f)
        {
            _lastDirection = Direction.Down;
        }
        if (direction.x > 0f)
        {
            _lastDirection = Direction.Right;
        }
        if (direction.x < 0f)
        {
            _lastDirection = Direction.Left;
        }
    }

    private IEnumerator DoMove(Vector3 direction, float timeToMove)
    {
        _isMoving = true;

        float moveTime = 0f;

        origin = transform.localPosition;
        LayerMask maskToIgnore = LayerMask.GetMask("Obstacle");
        if (transform.forward != direction || Physics.Raycast(_raycastPoint.position, direction, 0.5f, maskToIgnore))
        {
            transform.forward = direction;
            destination = origin;

            do
            {
                yield return null;

                moveTime += Time.deltaTime / timeToMove;
                _playerModel.transform.localPosition = new Vector3(0f, -4 * moveTime * moveTime + 4 * moveTime, 0f);
            } while (moveTime < 1.0f);
        }
        else
        {
            destination = origin + direction * _moveDistance;

            do
            {
                yield return null;
                
                moveTime += Time.deltaTime / timeToMove;
                transform.localPosition = Vector3.Lerp(origin, destination, moveTime);
                _playerModel.transform.localPosition = new Vector3(0f, -4 * moveTime * moveTime + 4 * moveTime, 0f);
            } while (moveTime < 1.0f);

            transform.localPosition = destination;
        }

        _playerModel.transform.localPosition = Vector3.zero;

        _isMoving = false;
    }

    public bool isMoving()
    {
        return _isMoving;
    }

    public void ResetToPosition(Vector3 position)
    {
        transform.position = position;
        destination = transform.localPosition;
    }
}
