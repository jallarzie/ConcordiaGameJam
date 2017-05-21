using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeEffect : MonoBehaviour {

	static float smallParticles = 0.1f;
	static float largeParticles = 2.0f;
	static float particleSizeIncrement = 0.1f;
	static float particleTimeDelay = 0.02f;

	public GameObject padParticles;
	public bool isOnPad = false;
	public bool isCorrectPattern = false;
	public bool isStartPad;
	public GameObject endPad;
//	public bool isTeleporting = false;


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
				isCorrectPattern = false;
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
		ParticleSystem.MainModule ps0 = ps[0].main;
		ParticleSystem.MainModule ps1 = ps[1].main;
		for (float f = smallParticles; f < largeParticles; f += particleSizeIncrement) {
			ps0.startSize = f;
			ps1.startSize = f;
			yield return new WaitForSeconds(particleTimeDelay);
		}
		ps0.startSize = smallParticles;
		ps1.startSize = smallParticles;
		padParticles.SetActive(false);
		Debug.Log ("TELEPORT");
		GameObject.FindWithTag ("Player").transform.position = endPad.transform.position;
	}

	IEnumerator EffectOut(){
		ParticleSystem.MainModule ps0 = ps[0].main;
		ParticleSystem.MainModule ps1 = ps[1].main;
		for (float f = largeParticles; f > smallParticles; f -= particleSizeIncrement) {
			ps0.startSize = f;
			ps1.startSize = f;
			yield return new WaitForSeconds(particleTimeDelay);
		}
		ps0.startSize = largeParticles;
		ps1.startSize = largeParticles;
		padParticles.SetActive(false);
		Debug.Log ("TELEPORTED");
	}

}