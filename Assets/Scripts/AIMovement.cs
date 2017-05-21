using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : MonoBehaviour {

    public int[] pattern;
    [SerializeField]
    private Form _form;

    public Form form
    {
        get { return _form; }
    }

    public int[] actions;
    public float timeToMakeMovement;
    public float timeToMakeRotation;
    public bool loop;

    private int currentIndex = -1;
    private bool aller;
    private Vector3 start;
    private Vector3 destination;
    [SerializeField]
    private GameObject _guardModel;

    private const int MOVE_FORWARD = 1;
    private const int MOVE_BACK = 2;
    private const int MOVE_LEFT = 3;
    private const int MOVE_RIGHT = 4;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        GameObject player = GameObject.FindWithTag("Player");
        Vector3 vectorBetweenGuardAndPlayer = player.transform.position - transform.position;
        Vector3 localForward = transform.forward;
        vectorBetweenGuardAndPlayer.Normalize();
        //vectorBetweenGuardAndPlayer = vectorBetweenGuardAndPlayer * visionDistance;

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

    public void NextAction()
    {
        SetCurrentIndex();

        Vector3 direction = Vector3.zero;

        switch (actions[currentIndex])
        {
            case MOVE_FORWARD:
                direction = transform.forward;
                break;
            case MOVE_BACK:
                direction = -transform.forward;
                break;
            case MOVE_LEFT:
                direction = -transform.right;
                break;
            case MOVE_RIGHT:
                direction = transform.right;
                break;
        }

        StartCoroutine(DoMove(direction));
    }

    private IEnumerator DoMove(Vector3 direction)
    {
        float moveTime = 0f;

        start = transform.localPosition;
        LayerMask maskToIgnore = LayerMask.GetMask("Obstacle");
        if (transform.forward != direction)
        {
            transform.forward = direction;
            destination = start;

            do
            {
                yield return null;

                moveTime += Time.deltaTime / timeToMakeMovement;
                _guardModel.transform.localPosition = new Vector3(0f, -4 * moveTime * moveTime + 4 * moveTime, 0f);
            } while (moveTime < 1.0f);
        }
        else
        {
            destination = start + direction;

            do
            {
                yield return null;

                moveTime += Time.deltaTime / timeToMakeMovement;
                transform.localPosition = Vector3.Lerp(start, destination, moveTime);
                _guardModel.transform.localPosition = new Vector3(0f, -4 * moveTime * moveTime + 4 * moveTime, 0f);
            } while (moveTime < 1.0f);

            transform.localPosition = destination;
        }

        _guardModel.transform.localPosition = Vector3.zero;
    }

    private void SetCurrentIndex()
    {
        if (!loop)
        {
            if (aller)
            {
                currentIndex++;
                aller = currentIndex < actions.Length - 1;
            }
            else
            {
                currentIndex--;
                aller = currentIndex == 0;
            }
        } else
        {
            currentIndex = (currentIndex + 1) % actions.Length;
        }
    }
}
