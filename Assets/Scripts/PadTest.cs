﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PadTest : MonoBehaviour {

    public Vector3 originPositionOfPlayer;
    public GameObject guardRelated;
    public Transform padTestEnd;

    private Transform player;

    void Start() {
        player = GameObject.FindWithTag("Player").transform;
    }

    void Update() {

    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.tag == "Player")
        {
            originPositionOfPlayer = player.GetComponent<PlayerController>().origin;
            GameManager.instance.SetPlayerControllerScript(false);
            GameManager.instance.SetPatternIndicationMode(true);
            GameManager.instance.GetPattern(this, guardRelated.GetComponent<AIMovement>().pattern);
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

    public void Teleportation()
    {
        player.position = new Vector3(padTestEnd.position.x,
            0, padTestEnd.position.z);
    }

    public IEnumerator makePlayerGoOut()
    {
        player.GetComponent<PlayerController>().Move(originPositionOfPlayer - player.transform.position); // to make the player rotate
        yield return new WaitForSeconds(0.5f);
        player.GetComponent<PlayerController>().Move(originPositionOfPlayer - player.transform.position); // to make the player move
    }

    public void ChangeMaterial()
    {
        player.GetChild(0).transform.GetComponent<MeshFilter>().mesh = guardRelated.GetComponentInChildren<MeshFilter>().mesh;
    }
}
