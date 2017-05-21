using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PadTest : MonoBehaviour {

    public Vector3 originPositionOfPlayer;
    public GameObject[] guardsRelated;
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
            int length = -1;
            for (int i = 0; i < guardsRelated.Length; i++)
            {
                int guardPatternLength = guardsRelated[i].GetComponent<AIMovement>().pattern.Length;
                // get longest length
                if (length < guardPatternLength)
                {
                    length = guardPatternLength;
                }
            }
            int[][] patterns = new int[guardsRelated.Length][];
            for (int i = 0; i < guardsRelated.Length; i++)
            {
                patterns[i] = guardsRelated[i].GetComponent<AIMovement>().pattern;
            }
            GameManager.instance.GetPatterns(this, patterns);
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

    public void ChangeMaterial(int index)
    {
        player.GetChild(0).transform.GetComponent<MeshFilter>().mesh = guardsRelated[index].GetComponentInChildren<MeshFilter>().mesh;
    }
}
