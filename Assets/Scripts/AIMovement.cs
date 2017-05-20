using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : MonoBehaviour {

    public int[] actions;
    public float timeToMakeMovement;
    public float timeToMakeRotation;
    public bool loop;
    public float visionDistance = 2f;

    private int currentIndex;
    private bool isDoingAction;
    private Vector3 start;
    private Vector3 destination;
    private float timer;
    private bool aller = true;
    private bool move;
    private bool rotate;
    private bool transition;
    private bool _isMoving;
    [SerializeField]
    private Transform _raycastPoint;
    [SerializeField]
    private GameObject _guardModel;
    private bool coroutineStarted;

    private const int MOVE_FORWARD = 1;
    private const int MOVE_BACK = 2;
    private const int MOVE_LEFT = 3;
    private const int MOVE_RIGHT = 4;
    private const int MOVE_PAUSE = 5;
    private const int ROTATE_FORWARD = 6;
    private const int ROTATE_BACK = 7;
    private const int ROTATE_LEFT = 8;
    private const int ROTATE_RIGHT = 9;

    void Start () {
        currentIndex = -1;
        isDoingAction = false;
        start = transform.position;
        // SetDestination();
        timer = 0.0f;
        move = true;
        transition = false;
        coroutineStarted = false;
	}

    private void SetDestination()
    {
        if (transition)
        {
            move = false;
            rotate = false;
            destination = transform.position + -transform.forward;
            return;
        }
        if (aller)
        {
            switch (actions[currentIndex])
            {
                case MOVE_FORWARD:
                    move = true;
                    rotate = false;
                    destination = transform.position + transform.forward;
                    break;
                case MOVE_BACK:
                    move = true;
                    rotate = false;
                    destination = transform.position + -transform.forward;
                    break;
                case MOVE_LEFT:
                    move = true;
                    rotate = false;
                    destination = transform.position + -transform.right;
                    break;
                case MOVE_RIGHT:
                    move = true;
                    rotate = false;
                    destination = transform.position + transform.right;
                    break;
                case MOVE_PAUSE:
                    move = false;
                    rotate = false;
                    destination = transform.position;
                    break;
                case ROTATE_FORWARD:
                    move = false;
                    rotate = true;
                    destination = transform.position + transform.forward;
                    break;
                case ROTATE_BACK:
                    move = false;
                    rotate = true;
                    destination = transform.position + -transform.forward;
                    break;
                case ROTATE_LEFT:
                    move = false;
                    rotate = true;
                    destination = transform.position + -transform.right;
                    break;
                case ROTATE_RIGHT:
                    move = false;
                    rotate = true;
                    destination = transform.position + transform.right;
                    break;
            }
        } else 
        {
            switch (actions[currentIndex])
            {
                case MOVE_FORWARD:
                    move = true;
                    rotate = false;
                    destination = transform.position + transform.forward;
                    break;
                case MOVE_BACK:
                    move = true;
                    rotate = false;
                    destination = transform.position + -transform.forward;
                    break;
                case MOVE_LEFT:
                    move = true;
                    rotate = false;
                    destination = transform.position + -transform.right;
                    break;
                case MOVE_RIGHT:
                    move = true;
                    rotate = false;
                    destination = transform.position + transform.right;
                    break;
                case MOVE_PAUSE:
                    move = false;
                    rotate = false;
                    destination = transform.position;
                    break;
                case ROTATE_FORWARD:
                    move = false;
                    rotate = true;
                    destination = transform.position + -transform.forward;
                    break;
                case ROTATE_BACK:
                    move = false;
                    rotate = true;
                    destination = transform.position + transform.forward;
                    break;
                case ROTATE_LEFT:
                    move = false;
                    rotate = true;
                    destination = transform.position + transform.right;
                    break;
                case ROTATE_RIGHT:
                    move = false;
                    rotate = true;
                    destination = transform.position + -transform.right;
                    break;
            }
        }
    }


    void Update()
    {
        if (transition)
        {
            Rotate180Degrees();
            return;
        }
        if (move)
        {
            Move();
            return;
        }
        if (rotate) { 
            LookAtDirection();
            return;
        }
        if (!move && !rotate)
        {
            Pause();
        }
        
	}

    private void Pause()
    {
        if (isDoingAction)
        {
            if (HasFinishedAction())
            {
                // set isMoving to false
                isDoingAction = false;
            }
        }
        else
        {
            // update aller, currentIndex, start and destination
            SetAller();
            start = transform.position;
            SetDestination();
            timer = 0.0f;
            isDoingAction = true;
        }
        timer += Time.deltaTime;
    }

    private void Rotate180Degrees()
    {
        if (isDoingAction)
        {
            if (HasFinishedAction())
            {
                // set isMoving to false
                isDoingAction = false;
            }
            else
            {
                // continue the rotation
                //Vector3 direction = destination - start;
                //direction.Normalize();

                //// if we have a non-zero direction then look towards that direciton otherwise do nothing
                //if (direction.sqrMagnitude > 0.001f)
                //{
                //    float toRotation = (Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg);
                //    float rotation = Mathf.LerpAngle(transform.rotation.eulerAngles.y, toRotation, Time.deltaTime / timeToMakeRotation);

                //    transform.rotation = Quaternion.Euler(0, rotation, 0);
                //}
            }
            if (!coroutineStarted)
            {
                coroutineStarted = true;
                StartCoroutine(DoMove());
            }
        }
        else
        {
            // update aller, currentIndex, start and destination
            SetAller();
            start = transform.position;
            SetDestination();
            timer = 0.0f;
            isDoingAction = true;
            coroutineStarted = false;
        }
        timer += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        CheckFieldOfView();

    }

    private void CheckFieldOfView()
    {
        GameObject player = GameObject.FindWithTag("Player");
        Vector3 vectorBetweenGuardAndPlayer = player.transform.position - transform.position;
        Vector3 localForward = transform.forward;
        RaycastHit hit;

        bool firstObjectRayIntersect = Physics.Raycast(transform.position, vectorBetweenGuardAndPlayer, out hit, Mathf.Infinity);  
        if (firstObjectRayIntersect)
        {
            if (hit.transform.tag != "Player")
            {
                return;
            } else
            {
                // Debug.Log("Player in field of view");
            }
        }
        float distance = vectorBetweenGuardAndPlayer.magnitude;

        // if player is close enough
        if (distance < visionDistance)
        {
            float angle = Vector3.Angle(localForward, vectorBetweenGuardAndPlayer);
            if (angle < 20f)
            {
                Debug.Log("Player detected");
            }
        }         
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        GameObject player = GameObject.FindWithTag("Player");
        Vector3 vectorBetweenGuardAndPlayer = player.transform.position - transform.position;
        Vector3 localForward = transform.forward;
        vectorBetweenGuardAndPlayer.Normalize();
        vectorBetweenGuardAndPlayer = vectorBetweenGuardAndPlayer * visionDistance;

        Vector3 localForwardAngleMin = Quaternion.Euler(0, -20, 0) * localForward;
        Vector3 localForwardAngleMax = Quaternion.Euler(0, 20, 0) * localForward;
        Vector3 rayGizmoDestination = transform.position + vectorBetweenGuardAndPlayer;
        Vector3 rayGizmoAngleMin = transform.position + localForwardAngleMin;
        Vector3 rayGizmoAngleMax = transform.position + localForwardAngleMax;
        Gizmos.DrawLine(transform.position, rayGizmoDestination);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, rayGizmoAngleMin);
        Gizmos.DrawLine(transform.position, rayGizmoAngleMax);
    }

    private void LookAtDirection()
    {
        if (isDoingAction)
        {
            if (HasFinishedAction())
            {
                // set isMoving to false
                isDoingAction = false;
            }
            else
            {
                //// continue the rotation
                //Vector3 direction = destination - start;
                //direction.Normalize();

                //// if we have a non-zero direction then look towards that direciton otherwise do nothing
                //if (direction.sqrMagnitude > 0.001f)
                //{
                //    float toRotation = (Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg);
                //    float rotation = Mathf.LerpAngle(transform.rotation.eulerAngles.y, toRotation, Time.deltaTime / timeToMakeRotation);

                //    transform.rotation = Quaternion.Euler(0, rotation, 0);
                //}
            }
            if (!coroutineStarted)
            {
                coroutineStarted = true;
                StartCoroutine(DoMove());
            }
        }
        else
        {
            // update aller, currentIndex, start and destination
            SetAller();
            start = transform.position;
            SetDestination();
            timer = 0.0f;
            isDoingAction = true;
            coroutineStarted = false;
        }
        timer += Time.deltaTime;      
    }

    void Move()
    {
        if (isDoingAction)
        {
            if (HasFinishedAction())
            {
                // set isMoving to false
                isDoingAction = false;               
            }
            if (!coroutineStarted)
            {
                coroutineStarted = true;
                StartCoroutine(DoMove());
            }
        } else
        {
            // update aller, currentIndex, start and destination
            SetAller();            
            start = transform.position;
            SetDestination();
            timer = 0.0f;
            isDoingAction = true;
            coroutineStarted = false;
            
        }
        timer += Time.deltaTime;
    }

    private IEnumerator DoMove()
    {
        float moveTime = 0f;
        Vector3 direction = destination - start;
        if (transform.forward != direction)
        {
            transform.forward = direction;
            yield return null;

            while (timer < timeToMakeMovement)
            {
                moveTime = timer / timeToMakeMovement;
                _guardModel.transform.localPosition = new Vector3(0f, -4 * moveTime * moveTime + 4 * moveTime + 0.5f, 0f);
                yield return null;
            }

            _guardModel.transform.localPosition = new Vector3(0f, 0.5f, 0f);
        }
        else
        {
            while (timer < timeToMakeMovement)
            {
                moveTime = timer / timeToMakeMovement;
                transform.localPosition = Vector3.Lerp(start, destination, moveTime);
                _guardModel.transform.localPosition = new Vector3(0f, -4 * moveTime * moveTime + 4 * moveTime + 0.5f, 0f);
                yield return null;
            }

            transform.localPosition = destination;
            _guardModel.transform.localPosition = new Vector3(0f, 0.5f, 0f);
        }
    }

    private void SetCurrentIndex()
    {
        if (!loop)
        {
            if (aller)
            {
                currentIndex++;
            }
            else
            {
                currentIndex--;
            }
        } else
        {
            currentIndex = (currentIndex + 1) % actions.Length;
        }
    }

    private void SetAller()
    {   if (!loop)
        {
            if (transition)
            {
                transition = false;
                return;
            }
            if (aller)
            {
                if (currentIndex == actions.Length - 1)
                {
                    aller = false;
                    transition = true;
                }
                else
                {
                    SetCurrentIndex();
                }

            }
            else
            {
                if (currentIndex == 0)
                {
                    aller = true;
                    transition = true;
                }
                else
                {
                    SetCurrentIndex();
                }
            }
        } else
        {
            // loop
            SetCurrentIndex();           
        }
    }

    private bool HasFinishedAction()
    {
        if (timer < timeToMakeMovement)
        {
            return false;
        }
        // adjust rotation
        if (rotate || transition)
        {
            Vector3 direction = destination - start;
            float toRotation = (Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg);
            transform.rotation = Quaternion.Euler(0, toRotation, 0);
        }
        return true;
    }
}
