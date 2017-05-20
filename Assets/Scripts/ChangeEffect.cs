using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeEffect : MonoBehaviour {

	private float minimum = 0.06f;
	private float maximum = 2.0f;
	static float t = 0.0f;

	private ParticleSystem.MainModule ps;


	// Use this for initialization
	void Start () {
		ps = gameObject.GetComponent<ParticleSystem> ().main;

	}

	// Update is called once per frame
	void Update () {
		ps.startSize = Mathf.Lerp (minimum, maximum, t);
		t += 0.01f * Time.deltaTime;
		if (t > 1.0f){
			float temp = maximum;
			maximum = minimum;
			minimum = temp;
			t = 0.0f;
		}
	}
}
