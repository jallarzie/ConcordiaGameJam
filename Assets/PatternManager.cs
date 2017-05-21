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

    public AudioSource patternFailureAudio;
    public AudioSource patternSucceedAudio;
    public AudioSource patternNeutralAudio;
    public AudioSource rufflingPaperAudio;

    private Form selectedForm = Form.Paper;
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
                indicatorsArray[currentIndexPatternInput].GetComponent<Image>().sprite = indicatorPrefabBlue.GetComponent<Image>().sprite;
                currentIndexPatternInput++;
                if (currentIndexPatternInput < patternInput.Length)
                {
                    patternNeutralAudio.Play();
                }
            } else if (Input.GetKeyDown("n"))
            {
                patternInput[currentIndexPatternInput] = MoveAction.NoHop;
                indicatorsArray[currentIndexPatternInput].GetComponent<Image>().sprite = indicatorPrefabBlue.GetComponent<Image>().sprite;
                currentIndexPatternInput++;
                if (currentIndexPatternInput < patternInput.Length)
                {
                    patternNeutralAudio.Play();
                }
            }
            if (currentIndexPatternInput >= patternInput.Length)
            {
                Debug.Log("check validity of pattern input");
                bool equals = ComparePatterns();
                StartCoroutine("DisplayAndResetUI", equals);
                Reset();
                PlaySound(true, equals);
                if (equals)
                {
                    GameManager.instance.ActionsForRightCombination(selectedForm);
                } else
                {                 
                    GameManager.instance.ActionsForWrongCombination();
                }
            } 
        }

    }

    private void PlaySound(bool finalInput, bool equals)
    {
        if (finalInput)
        {
            if (equals)
            {
                patternSucceedAudio.Play();
            }
            else
            {
                patternFailureAudio.Play();
            }
        } else
        {
            patternNeutralAudio.Play();
        }
    }

    private IEnumerator DisplayAndResetUI(bool equal)
    {
        if (equal)
        {
            for (int i = 0; i < indicatorsArray.Length; i++)
            {
                indicatorsArray[i].GetComponent<Image>().sprite = indicatorPrefabGreen.GetComponent<Image>().sprite;
            }
        } else
        {
            for (int i = 0; i < indicatorsArray.Length; i++)
            {
                indicatorsArray[i].GetComponent<Image>().sprite = indicatorPrefabRed.GetComponent<Image>().sprite;
            }
        }
        yield return new WaitForSeconds(2);
        for (int i = 0; i < indicatorsArray.Length; i++)
        {
            Destroy(indicatorsArray[i]);
        }
    }

    private void Reset()
    {
        readyForInput = false;
        patternValidator = null;
        patternInput = null;
        currentIndexPatternInput = 0;
    }

    private bool ComparePatterns()
    {
        bool equal = true;
        for (int i = 1; i < 5; i++)
        {
            equal = true;
            MoveAction[] pattern = patternValidator.GetPatternForForm((Form)i);
            if (pattern.Length == patternValidator._patternLenght)
            {
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
