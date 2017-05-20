using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PadTest : MonoBehaviour {

    public Transform padTestEnd;
    public Transform player;



	void Start () {
		
	}
	
	void Update () {
		
	}

    void OnTriggerEnter(Collider collider)
    {       
        if (collider.transform.tag == "PlayerModel")
        {
            StartCoroutine(Teleport());
        }
    }

    private IEnumerator Teleport()
    {
        while (player.GetComponent<PlayerController>().isMoving())
        {
            yield return null;
        }
        Debug.Log("Triggered");
        player.position = new Vector3(padTestEnd.position.x,
            padTestEnd.position.y, padTestEnd.position.z);
        Debug.Log(padTestEnd.transform.position);
        yield return null;
    }

}
