using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeEffect : MonoBehaviour {

    [SerializeField]
    private PatternValidator patternValidator;

    [SerializeField]
    private Form form;

    public Vector3 originPositionOfPlayer;

	static float smallParticles = 0.1f;
	static float largeParticles = 2.0f;
	static float particleSizeIncrement = 0.1f;
	static float particleTimeDelay = 0.02f;

	private GameObject padParticles;
	public bool isOnPad = false;
	public bool isCorrectPattern = false;
	public bool isStartPad;
	public GameObject endPad;

	private ParticleSystem.MainModule ps0; 
	private ParticleSystem.MainModule ps1; 


	void Start(){
		if (endPad == null) {
			isStartPad = false;
		}
		padParticles = transform.GetChild(0).gameObject;
		ps0 = transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().main;
		ps1 = transform.GetChild(0).transform.GetChild(1).gameObject.GetComponent<ParticleSystem>().main;
		Debug.Log(transform.GetChild(0).transform.GetChild(0).name);

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

	IEnumerator OnTriggerEnter(Collider col){
		if(col.gameObject.tag == "Player"){
			Debug.Log ("Im on the pad");
			padParticles.SetActive(true);
			isOnPad = true;
            if (isStartPad)
            {
                originPositionOfPlayer = col.attachedRigidbody.GetComponent<PlayerController>().origin;
                GameManager.instance.SetPlayerInput(false);
                /*GameManager.instance.SetPatternIndicationMode(true);
                GameManager.instance.ActivatePad(this, patternValidator);*/

                yield return new WaitForSeconds(0.2f);

                isCorrectPattern = true;

                yield return new WaitForSeconds(0.2f);

                col.GetComponent<PlayerController>().ChangeForm(form);


                GameManager.instance.SetPlayerInput(true);
            }
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

    public IEnumerator makePlayerGoOut()
    {
        PlayerController player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

        player.Move(originPositionOfPlayer - player.transform.position); // to make the player rotate
        yield return new WaitForSeconds(0.5f);
        player.Move(originPositionOfPlayer - player.transform.position); // to make the player move
    }

}