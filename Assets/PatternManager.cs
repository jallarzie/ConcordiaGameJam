using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PatternManager : MonoBehaviour {

    public GameObject patternIndicatorUI;
    public GameObject indicatorPrefab;

    private GameObject[] indicatorsArray;
    private int[] pattern;
    private int[] patternModified;
    private int[] patternInput;
    private int currentIndexPatternInput = 0;
    private bool readyForInput = false;

    private const int MOVE_FORWARD = 1;
    private const int MOVE_BACK = 2;
    private const int MOVE_LEFT = 3;
    private const int MOVE_RIGHT = 4;
    private const int MOVE_PAUSE = 5;
    private const int ROTATE_FORWARD = 6;
    private const int ROTATE_BACK = 7;
    private const int ROTATE_LEFT = 8;
    private const int ROTATE_RIGHT = 9;

    private const int HOP = 10;
    private const int NOHOP = 11;

    void Start () {
		
	}
	
	void Update () {
		if (GameManager.instance.GetPatternIndicationMode() && readyForInput)
        {
            if (Input.GetKeyDown("h"))
            {
                patternInput[currentIndexPatternInput] = HOP;
                Debug.Log("HOP input");
                indicatorsArray[currentIndexPatternInput].GetComponent<Image>().color = Color.green;
                currentIndexPatternInput++;
            } else if (Input.GetKeyDown("n"))
            {
                patternInput[currentIndexPatternInput] = NOHOP;
                Debug.Log("NOHOP input");
                indicatorsArray[currentIndexPatternInput].GetComponent<Image>().color = Color.green;
                currentIndexPatternInput++;
            }
            if (currentIndexPatternInput >= patternInput.Length)
            {
                Debug.Log("check validity of pattern input");
                bool equals = ComparePatterns();
                Debug.Log(equals);
                if (equals)
                {
                    Reset();
                    GameManager.instance.ActionsForRightCombination();
                    
                } else
                {
                    Reset();
                    GameManager.instance.ActionsForWrongCombination();                   
                }
            }
        }

	}

    private void Reset()
    {
        readyForInput = false;
        for (int i = 0; i < indicatorsArray.Length; i++)
        {
            Destroy(indicatorsArray[i]);
        }
        pattern = null;
        patternModified = null;
        patternInput = null;
        currentIndexPatternInput = 0;
    }

    private bool ComparePatterns()
    {
        for (int i = 0; i < patternModified.Length; i++)
        {
            if (patternModified[i] != patternInput[i])
            {
                return false;
            }
        }
        return true;
    }

    public void GetPattern(int[] pattern)
    {
        this.pattern = pattern;
        SetPatternModified();
        patternInput = new int[patternModified.Length];
        indicatorsArray = new GameObject[patternModified.Length];
        CreateImageInPatternIndicator();
        readyForInput = true;
    }

    private void CreateImageInPatternIndicator()
    {
        for (int i = 0; i < patternModified.Length; i++)
        {
            GameObject indicator = Instantiate(indicatorPrefab) as GameObject;
            indicator.transform.parent = patternIndicatorUI.transform;
            indicatorsArray[i] = indicator;
        }
    }

    private void SetPatternModified()
    {
        if (pattern != null)
        {
            patternModified = new int[pattern.Length];
            for (int i = 0; i < pattern.Length; i++){
                if (pattern[i] == MOVE_PAUSE)
                {
                    patternModified[i] = NOHOP;
                } else
                {
                    patternModified[i] = HOP;
                }
            }
        }
    }
}
