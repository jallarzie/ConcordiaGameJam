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

    private int correctIndex = -1;
    private GameObject[] indicatorsArray;
    private int[][] patterns;
    private int[][] patternsModified;
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

    void Start() {

    }

    void Update() {
        if (GameManager.instance.GetPatternIndicationMode() && readyForInput)
        {
            if (Input.GetKeyDown("h"))
            {
                patternInput[currentIndexPatternInput] = HOP;
                indicatorsArray[currentIndexPatternInput].GetComponent<Image>().sprite = indicatorPrefabBlue.GetComponent<Image>().sprite;
                currentIndexPatternInput++;
                if (currentIndexPatternInput < patternInput.Length)
                {
                    patternNeutralAudio.Play();
                }
            } else if (Input.GetKeyDown("n"))
            {
                patternInput[currentIndexPatternInput] = NOHOP;
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
                    GameManager.instance.ActionsForRightCombination(correctIndex);

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
        patterns = null;
        patternsModified = null;
        patternInput = null;
        currentIndexPatternInput = 0;
    }

    private bool ComparePatterns()
    {
        bool equal = true;
        for (int i = 0; i < patternsModified.Length; i++)
        {
            equal = true;
            for (int j = 0; j < patternsModified[0].Length; j++)
            {
                if (patternsModified[i][j] != patternInput[j])
                {
                    equal = false;
                    break;
                }
            }
            if (equal)
            {
                correctIndex = i;
                return true;
            }
        }
        return false;
        
    }

    public void GetPatterns(int[][] patterns)
    {
        this.patterns = patterns;
        SetPatternModified();
        patternInput = new int[patternsModified[0].Length];
        indicatorsArray = new GameObject[patternsModified[0].Length];
        CreateImageInPatternIndicator();
        readyForInput = true;
    }

    private void CreateImageInPatternIndicator()
    {
        for (int i = 0; i < patternsModified[0].Length; i++)
        {
            GameObject indicator = Instantiate(indicatorPrefab) as GameObject;
            indicator.transform.parent = patternIndicatorUI.transform;
            indicatorsArray[i] = indicator;
        }
    }

    private void SetPatternModified()
    {
        if (patterns != null)
        {
            patternsModified = new int[patterns.Length][];          
            for (int i = 0; i < patterns.Length; i++)
            {
                int[] pattern = new int[patterns[i].Length];
                for (int j = 0; j < patterns[i].Length; j++)
                {                   
                    if (patterns[i][j] == MOVE_PAUSE)
                    {
                        pattern[j] = NOHOP;
                    }
                    else
                    {
                        pattern[j] = HOP;
                    }
                }
                patternsModified[i] = pattern;
            }
        }
    }
}
