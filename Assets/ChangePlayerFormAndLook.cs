using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePlayerFormAndLook : MonoBehaviour {

	public float moveSpeed = 10.0f;
	public List<Mesh> copyForm;
	public List<Material> copyStyle;
	public int currentFormIndex = 0;
	public int currentStyleIndex = 0;

	// Use this for initialization
	void Start () {
		
		copyForm.Add(gameObject.GetComponent<MeshFilter> ().mesh);
		copyStyle.Add(gameObject.GetComponent<Renderer> ().material);
		gameObject.GetComponent<MeshFilter> ().mesh = copyForm[currentFormIndex];
		gameObject.GetComponent<Renderer> ().material = copyStyle[currentStyleIndex];
		ChangeForm (currentFormIndex);
		ChangeStyle (currentStyleIndex);
	}
	
	// Update is called once per frame
	void Update () {


		/* Start Debugging */


		// Moving player
		if(Input.GetKey(KeyCode.UpArrow)){
			transform.Translate (Vector3.forward * moveSpeed * Time.deltaTime);
		}
		if(Input.GetKey(KeyCode.DownArrow)){
			transform.Translate (Vector3.back * moveSpeed * Time.deltaTime);
		}
		if(Input.GetKey(KeyCode.LeftArrow)){
			transform.Translate (Vector3.left * moveSpeed * Time.deltaTime);
		}
		if(Input.GetKey(KeyCode.RightArrow)){
			transform.Translate (Vector3.right * moveSpeed * Time.deltaTime);
		}

		// Change Form and Style using Keys
		if(Input.GetKey(KeyCode.Alpha0)){
			ChangeForm (0);
			ChangeStyle (0);
		}
		if(Input.GetKey(KeyCode.Alpha1)){
			ChangeForm (1);
			ChangeStyle (1);
		}
		if(Input.GetKey(KeyCode.Alpha2)){
			ChangeForm (2);
			ChangeStyle (2);
		}

		/* End Debugging */


	}

	void OnCollisionEnter(Collision col){
		
		Mesh currentColForm = col.gameObject.GetComponent<MeshFilter> ().mesh;
		Material currentColStyle = col.gameObject.GetComponent<Renderer> ().material;

		Debug.Log (currentColForm);
		Debug.Log (currentColStyle);


		if (!copyForm.Contains (currentColForm)) {
			copyForm.Add (currentColForm);
		}
		currentFormIndex = copyForm.IndexOf (currentColForm);
		ChangeForm (currentFormIndex);


		if(!copyStyle.Contains(currentColStyle)){
			copyStyle.Add(currentColStyle);
		}
		currentStyleIndex = copyStyle.IndexOf (currentColStyle);
		ChangeStyle(currentStyleIndex);

	}

	void ChangeForm(int formIndex){
		currentFormIndex = formIndex;
		gameObject.GetComponent<MeshFilter> ().mesh = copyForm[formIndex];
	}
	void ChangeStyle(int styleIndex){
		currentStyleIndex = styleIndex;
		gameObject.GetComponent<Renderer> ().material = copyStyle[styleIndex];
	}

}