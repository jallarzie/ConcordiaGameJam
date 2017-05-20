using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	private void Update() 
    {
        if (!_isMoving)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                Move(Vector3.forward);
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                Move(Vector3.back);
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                Move(Vector3.left);
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                Move(Vector3.right);
            }
        }
	}

    private void Move(Vector3 direction)
    {
        StartCoroutine(DoMove(direction));
    }

    private IEnumerator DoMove(Vector3 direction)
    {
        if (transform.forward != direction || Physics.Raycast(_raycastPoint.position, direction, 0.5f))
        {
            transform.forward = direction;
            _isMoving = true;

            float moveTime = 0f;

            yield return null;

            while (moveTime < 1.0f)
            {
                moveTime += Time.deltaTime / _moveInterval;
                _playerModel.transform.localPosition = new Vector3(0f, -4 * moveTime * moveTime + 4 * moveTime + 0.5f, 0f);
                yield return null;
            }

            _playerModel.transform.localPosition = new Vector3(0f, 0.5f, 0f);

            _isMoving = false;
        }
        else
        {
            _isMoving = true;

            Vector3 origin = transform.localPosition;
            Vector3 destination = origin + direction * _moveDistance;

            float moveTime = 0f;

            yield return null;

            while (moveTime < 1.0f)
            {
                moveTime += Time.deltaTime / _moveInterval;
                transform.localPosition = Vector3.Lerp(origin, destination, moveTime);
                _playerModel.transform.localPosition = new Vector3(0f, -4 * moveTime * moveTime + 4 * moveTime + 0.5f, 0f);
                yield return null;
            }

            transform.localPosition = destination;
            _playerModel.transform.localPosition = new Vector3(0f, 0.5f, 0f);

            yield return null;

            _isMoving = false;
        }
    }
}
