using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.

    PlayerController playerController;
    public GameObject[] guards;
    public float timeToInput = 0.5f;
    public AudioSource backgroundAudio;
    public PatternManager patternManager;

    private GameObject player;
    private Mesh meshOriginal;
    private Vector3 playerPositionOriginal;
    private PadTest padTest;
    private bool patternIndicatorMode;
    private bool musicIsPlaying = false;
    private bool startGuardsMove = false;
    private float musicTime;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    void Start () {
        player = GameObject.FindWithTag("Player");
        meshOriginal = player.GetComponentInChildren<MeshFilter>().mesh;
        playerPositionOriginal = player.transform.position;
        playerController = player.GetComponent<PlayerController>();
        patternIndicatorMode = false;
        backgroundAudio.Play();
	}
	
	void Update () {
        
	}

    public bool GetPatternIndicationMode()
    {
        return patternIndicatorMode;
    }

    public void SetPatternIndicationMode(bool b)
    {
        patternIndicatorMode = b;
    }

    public void ResetPlayer()
    {
        player.transform.position = playerPositionOriginal;
        player.GetComponentInChildren<MeshFilter>().mesh = meshOriginal;
    }

    public void SetPlayerControllerScript(bool b)
    {
        playerController.enabled = b;
    }


    public void GetPattern(PadTest padTest, int[] pattern)
    {
        this.padTest = padTest;
        patternManager.GetPattern(pattern);
    }

    public void ActionsForRightCombination()
    {
        patternIndicatorMode = false;
        playerController.enabled = true;
        padTest.Teleportation();
        padTest.ChangeMaterial();
    }

    public void ActionsForWrongCombination()
    {
        patternIndicatorMode = false;
        playerController.enabled = true;
        StartCoroutine(padTest.makePlayerGoOut());

    }
}
