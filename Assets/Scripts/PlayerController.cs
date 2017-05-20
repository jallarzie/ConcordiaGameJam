using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour 
{
    public Vector3 origin; // used for Padtest
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
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            if (Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f)
            {
                if (Mathf.Abs(horizontal) > Mathf.Abs(vertical))
                {
                    Move(new Vector3(Mathf.Sign(horizontal) * 1f, 0f, 0f));
                }
                else
                {
                    Move(new Vector3(0f, 0f, Mathf.Sign(vertical) * 1f));
                }
            }
        }
	}

    public void Move(Vector3 direction)
    {
        StartCoroutine(DoMove(direction));
    }

    private IEnumerator DoMove(Vector3 direction)
    {
        LayerMask maskToIgnore = 8;
        if (transform.forward != direction || Physics.Raycast(_raycastPoint.position, direction, 0.5f, maskToIgnore))
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

            origin = transform.localPosition;
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

    public bool isMoving()
    {
        return _isMoving;
    }
}
