using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.
    public GameObject[] rooms;

    PlayerController playerController;
    public float timeToInput = 0.5f;
    public AudioSource backgroundAudio;
    public PatternManager patternManager;

    // To be reset every time the player is changing rooms
    private Vector3 playerPositionOriginal;

    private Mesh meshOriginal;
    private GameObject player;
    private ChangeEffect changePad;
    private bool patternIndicatorMode;
    private int currentRoomIndex;

    internal void ChangeParametersNewRoom()
    {
        currentRoomIndex++;
        playerPositionOriginal = rooms[currentRoomIndex].transform.Find("SpawnPoint").position;
        player.GetComponentInChildren<MeshFilter>().mesh = meshOriginal;
    }

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    void Start () {
        currentRoomIndex = -1;
        player = GameObject.FindWithTag("Player");
        meshOriginal = player.GetComponentInChildren<MeshFilter>().mesh;
        playerController = player.GetComponent<PlayerController>();
        patternIndicatorMode = false;
        backgroundAudio.Play();
        //ChangeParametersNewRoom();
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
        player.GetComponent<PlayerController>().ResetToPosition(playerPositionOriginal);
        player.GetComponentInChildren<MeshFilter>().mesh = meshOriginal;
    }

    public void SetPlayerControllerScript(bool b)
    {
        playerController.enabled = b;
    }


    public void ActivatePad(ChangeEffect changePad, PatternValidator validator)
    {
        this.changePad = changePad;
        patternManager.SetPatterValidator(validator);
    }

    public void ActionsForRightCombination(Form selectedForm)
    {
        patternIndicatorMode = false;
        playerController.enabled = true;
        changePad.isCorrectPattern = true;
        playerController.ChangeForm(selectedForm);
    }

    public void ActionsForWrongCombination()
    {
        patternIndicatorMode = false;
        playerController.enabled = true;
        StartCoroutine(changePad.makePlayerGoOut());

    }
}
