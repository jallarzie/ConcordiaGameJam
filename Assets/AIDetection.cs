using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDetection : MonoBehaviour {

    public float visionDistance = 2f;

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
            }
            else
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
                Debug.Log("Intruder detected");
                GameManager.instance.ResetPlayer();
            }
        }
    }
}
