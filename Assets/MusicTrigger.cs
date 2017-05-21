using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicTrigger : MonoBehaviour {


//	public AudioSource audio;
//	public AudioClip victoryAudio;
	// Use this for initialization
	void Start () {
//		audio.clip = victoryAudio;

	}
	
	// Update is called once per frame
	void Update () {
//		Debug.Log (audio.volume);
		GameObject.Find("BackgroundAudio").GetComponent<AudioSource>().volume = 0;
	}

	void OnTriggerEnter(Collider col){
		if (col.gameObject.tag == "Player") {
//			audio.clip = victoryAudio;
			SceneManager.LoadScene(0);
		}
	}

}
