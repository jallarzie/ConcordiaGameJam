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
    public AudioSource poofAudio;
    public PatternManager patternManager;

    // To be reset every time the player is changing rooms
    private Vector3 playerPositionOriginal;

    private GameObject player;
    private ChangeEffect changePad;
    private bool patternIndicatorMode;
    private int currentRoomIndex;

    internal void ChangeParametersNewRoom()
    {
        currentRoomIndex++;
        playerPositionOriginal = rooms[currentRoomIndex].transform.Find("SpawnPoint").position;
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
        player.GetComponent<PlayerController>().ChangeForm(Form.Paper);
        poofAudio.Play();
    }

    public void SetPlayerInput(bool b)
    {
        playerController.SetInput(b);
    }


    public void ActivatePad(ChangeEffect changePad, PatternValidator validator)
    {
        this.changePad = changePad;
        patternManager.SetPatterValidator(validator);
    }

    public void ActionsForRightCombination(Form selectedForm)
    {
        patternIndicatorMode = false;
        SetPlayerInput(true);
        changePad.isCorrectPattern = true;
        playerController.ChangeForm(selectedForm);
    }

    public void ActionsForWrongCombination()
    {
        patternIndicatorMode = false;
        SetPlayerInput(true);
        StartCoroutine(changePad.makePlayerGoOut());

    }
}
