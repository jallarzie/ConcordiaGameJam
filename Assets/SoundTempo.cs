using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTempo : MonoBehaviour {

	public AudioSource gameMusic;
	public float gameMusicTime;
	public string[] pattern;
	public int patternPos = 0;
	// Use this for initialization
	void Start () {
		gameMusic.Play ();
	}
	
	// Update is called once per frame
	void Update () {


		/* Debugging Play Pause */
		if(Input.GetKeyUp(KeyCode.Space)){
			if (gameMusic.isPlaying) {
				gameMusic.Pause ();
			} else {
				gameMusic.Play ();
			}
		}
		/* End Debugging */

		float time = gameMusic.time;
		Debug.Log (pattern[(int)time % pattern.Length] + " : " + Mathf.Floor(gameMusic.time));
	}
}
