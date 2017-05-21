using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour {


	private ParticleSystem ps;

	// Use this for initialization
	void Start () {
		ps = GetComponentInChildren<ParticleSystem>();
		ps.Stop();
	}
	
	// Update is called once per frame
	void Update () {
		
			if (!ps.isEmitting) {
				// DEATH HAPPENS HERE/////////////////////////
				if (Input.GetKeyUp (KeyCode.D)) {
					Debug.Log ("The bunny is unfolded!!!!");
					ps.Play ();
				}
				//////////////////////////////////////////////////
			} else {
				ps.Stop ();
			}
		}

}
