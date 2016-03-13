using UnityEngine;
using System.Collections;

public class HashID : MonoBehaviour {

	public int hammerTrigger;

	void Awake(){
	

		hammerTrigger = Animator.StringToHash("IsBang");
	}
}
