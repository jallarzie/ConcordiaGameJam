using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public GameObject player;
    public GameObject[] guards;
    public float timeToInput = 0.5f;
    public AudioSource backgroundAudio;

    private bool musicIsPlaying = false;
    private bool startGuardsMove = false;
    private float musicTime;

	void Start () {
        backgroundAudio.Play();
	}
	
	void Update () {
        Debug.Log(backgroundAudio.time);
	}

    
}
