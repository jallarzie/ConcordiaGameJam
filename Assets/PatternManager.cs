using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PatternManager : MonoBehaviour {

    public GameObject patternIndicatorUI;
    public GameObject indicatorPrefab;
	public GameObject indicatorPrefabBlue;
	public GameObject indicatorPrefabRed;
	public GameObject indicatorPrefabGreen;

    private Form selectedForm;
    private GameObject[] indicatorsArray;
    private PatternValidator  patternValidator;
    private MoveAction[] patternInput;
    private int currentIndexPatternInput = 0;
    private bool readyForInput = false;
	
	void Update () {
		if (GameManager.instance.GetPatternIndicationMode() && readyForInput)
        {
            if (Input.GetKeyDown("h"))
            {
                patternInput[currentIndexPatternInput] = MoveAction.Hop;
                Debug.Log("HOP input");
                indicatorsArray[currentIndexPatternInput].GetComponent<Image>().color = Color.green;
                currentIndexPatternInput++;
            } else if (Input.GetKeyDown("n"))
            {
                patternInput[currentIndexPatternInput] = MoveAction.NoHop;
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
                    GameManager.instance.ActionsForRightCombination(selectedForm);
                    
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
        patternValidator = null;
        patternInput = null;
        currentIndexPatternInput = 0;
    }

    private bool ComparePatterns()
    {
        bool equal = true;
        for (int i = 0; i < 4; i++)
        {
            equal = true;
            MoveAction[] pattern = patternValidator.GetPatternForForm((Form)i);
            for (int j = 0; j < pattern.Length; j++)
            {
                if (pattern[j] != patternInput[j])
                {
                    equal = false;
                    break;
                }
            }
            if (equal)
            {
                selectedForm = (Form)i;
                return true;
            }
        }
        return false;
        
    }

    public void SetPatterValidator(PatternValidator validator)
    {
        patternValidator = validator;
        patternInput = new MoveAction[validator._patternLenght];
        indicatorsArray = new GameObject[validator._patternLenght];
        CreateImageInPatternIndicator();
        readyForInput = true;
    }

    private void CreateImageInPatternIndicator()
    {
        for (int i = 0; i < patternValidator._patternLenght; i++)
        {
            GameObject indicator = Instantiate(indicatorPrefab) as GameObject;
            indicator.transform.parent = patternIndicatorUI.transform;
            indicatorsArray[i] = indicator;
        }
    } 
}
