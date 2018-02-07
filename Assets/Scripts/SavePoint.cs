using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour {
	public GameObject save;
	void Start(){

	}

	public void revSave(){
		save.SetActive(true);
	}

	public void hideSave(){
		save.SetActive(false);
	}
}
