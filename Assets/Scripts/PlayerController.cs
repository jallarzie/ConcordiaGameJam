using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

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

    private void Start()
    {
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

                Debug.Log("Hop");

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
                    Debug.Log("No Hop");

                    yield return new WaitForSeconds(_moveInterval - inputTimer);
                }
            }

            inputTimer = 0f;
        }
	}

    private IEnumerator DoMove(Vector3 direction, float timeToMove)
    {
        _isMoving = true;

        float moveTime = 0f;

        if (transform.forward != direction || Physics.Raycast(_raycastPoint.position, direction, 0.5f))
        {
            transform.forward = direction;

            do
            {
                yield return null;

                moveTime += Time.deltaTime / timeToMove;
                _playerModel.transform.localPosition = new Vector3(0f, -4 * moveTime * moveTime + 4 * moveTime + 0.5f, 0f);
            } while (moveTime < 1.0f);
        }
        else
        {
            Vector3 origin = transform.localPosition;
            Vector3 destination = origin + direction * _moveDistance;

            do
            {
                yield return null;
                
                moveTime += Time.deltaTime / timeToMove;
                transform.localPosition = Vector3.Lerp(origin, destination, moveTime);
                _playerModel.transform.localPosition = new Vector3(0f, -4 * moveTime * moveTime + 4 * moveTime + 0.5f, 0f);
            } while (moveTime < 1.0f);

            transform.localPosition = destination;
        }

        _playerModel.transform.localPosition = new Vector3(0f, 0.5f, 0f);

        _isMoving = false;
    }
}
