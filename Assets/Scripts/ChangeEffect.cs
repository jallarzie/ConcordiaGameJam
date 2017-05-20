using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeEffect : MonoBehaviour {

	public GameObject padParticles;
	public bool isOnPad = false;
	public bool isCorrectPattern = false;
	public bool isStartPad;
	public GameObject endPad;
	public bool isTeleporting = false;


	public ParticleSystem[] ps;


	void Start(){
		if (endPad == null) {
			isStartPad = false;
		}
	}

	void Update () {

		// IF THE CODE IS RIGHT ////////////////////////
			if (Input.GetKeyUp (KeyCode.A)) {
				if (isCorrectPattern) {
					isCorrectPattern = false;
				} else {
					isCorrectPattern = true;
				}
			}
		////////////////////////////////////////////////

		if(isOnPad && isStartPad){
			if(isCorrectPattern){
				StartCoroutine (EffectIn());
			}
		}

		if(isOnPad && !isStartPad){
			if(isCorrectPattern){
				StartCoroutine (EffectOut());
				isCorrectPattern = false;
			}
		}

	}

	void OnTriggerEnter(Collider col){
		if(col.gameObject.tag == "Player"){
			Debug.Log ("Im on the pad");
			padParticles.SetActive(true);
			isOnPad = true;
		}
	}
	void OnTriggerExit(Collider col){
		if(col.gameObject.tag == "Player"){
			Debug.Log ("Im off the pad");
			padParticles.SetActive(false);
			isOnPad = false;
		}
	}

	IEnumerator EffectIn(){
		for (float f = 0.06f; f < 4.0f; f += 0.1f) {
			ParticleSystem.MainModule ps0 = ps[0].main;
			ParticleSystem.MainModule ps1 = ps[1].main;
			ps0.startSize = f;
			ps1.startSize = f;
			yield return new WaitForSeconds(.1f);
		}
		padParticles.SetActive(false);
		Debug.Log ("TELEPORT");
		GameObject.FindWithTag ("Player").transform.position = endPad.transform.position;
	}

	IEnumerator EffectOut(){
		for (float f = 4.0f; f > 0.06f; f -= 0.1f) {
			ParticleSystem.MainModule ps0 = ps[0].main;
			ParticleSystem.MainModule ps1 = ps[1].main;
			ps0.startSize = f;
			ps1.startSize = f;
			yield return new WaitForSeconds(.1f);
		}
		padParticles.SetActive(false);
		Debug.Log ("TELEPORTED");
	}

}
