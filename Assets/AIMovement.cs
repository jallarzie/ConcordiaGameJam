using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : MonoBehaviour {

    public int[] movements;
    public float timeToMakeMovement;
    public float timeToMakeRotation;
    public bool loop;
    public float visionDistance = 2f;

    private int currentIndex;
    private bool isMoving;
    private Vector3 start;
    private Vector3 destination;
    private float timer;
    private bool aller = true;

    private const int MOVE_FORWARD = 1;
    private const int MOVE_BACK = 2;
    private const int MOVE_LEFT = 3;
    private const int MOVE_RIGHT = 4;
    private const int MOVE_PAUSE = 5;

    void Start () {
        timeToMakeMovement = 1f;
        timeToMakeRotation = 0.2f;
        loop = true;
        currentIndex = 0;
        isMoving = true;
        start = transform.position;
        SetDestination();
        timer = 0.0f;
	}

    private void SetDestination()
    {
        if (aller)
        {
            switch (movements[currentIndex])
            {
                case MOVE_FORWARD:
                    destination = transform.position + Vector3.forward;
                    break;
                case MOVE_BACK:
                    destination = transform.position + Vector3.back;
                    break;
                case MOVE_LEFT:
                    destination = transform.position + Vector3.left;
                    break;
                case MOVE_RIGHT:
                    destination = transform.position + Vector3.right;
                    break;
                case MOVE_PAUSE:
                    destination = transform.position;
                    break;
            }
        } else 
        {
            switch (movements[currentIndex])
            {
                case MOVE_FORWARD:
                    destination = transform.position + Vector3.back;
                    break;
                case MOVE_BACK:
                    destination = transform.position + Vector3.forward;
                    break;
                case MOVE_LEFT:
                    destination = transform.position + Vector3.right;
                    break;
                case MOVE_RIGHT:
                    destination = transform.position + Vector3.left;
                    break;
                case MOVE_PAUSE:
                    destination = transform.position;
                    break;
            }
        }
    }

    void Update () {
        Move();
        LookAtDirection();
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
        Vector3 direction = destination - start;
        direction.Normalize();

        // if we have a non-zero direction then look towards that direciton otherwise do nothing
        if (direction.sqrMagnitude > 0.001f)
        {
            float toRotation = (Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg);
            float rotation = Mathf.LerpAngle(transform.rotation.eulerAngles.y, toRotation, Time.deltaTime/timeToMakeRotation);

            transform.rotation = Quaternion.Euler(0, rotation, 0);
        }      
    }

    void Move()
    {
        if (isMoving)
        {
            if (HasArrived())
            {
                // set isMoving to false
                isMoving = false;               
            } else
            {
                // continue the movement
                transform.position = Vector3.Lerp(start, destination, timer/timeToMakeMovement);
            }
        } else
        {
            // update aller, currentIndex, start and destination
            SetAller();            
            start = transform.position;
            SetDestination();
            timer = 0.0f;
            isMoving = true;
        }
        timer += Time.deltaTime;
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
            currentIndex = (currentIndex + 1) % movements.Length;
        }
    }

    private void SetAller()
    {   if (!loop)
        {
            if (aller)
            {
                if (currentIndex == movements.Length - 1)
                {
                    aller = false;
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
                }
                else
                {
                    SetCurrentIndex();
                }
            }
        } else
        {
            // no loop
            SetCurrentIndex();           
        }
    }

    private bool HasArrived()
    {
        if (timer < timeToMakeMovement)
        {
            return false;
        }
        return true;
    }
}
