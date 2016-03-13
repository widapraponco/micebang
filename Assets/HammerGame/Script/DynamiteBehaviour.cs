using UnityEngine;
using System.Collections;

public class DynamiteBehaviour : MonoBehaviour {

	public float dynamiteSpeed = 1;
	public float showUp = 1;
	// Use this for initialization
	
	private Vector3 from;
	private Vector3 to;
	
	public AudioClip DynamiteDie;
	public AudioClip DynamiteUp;
	
	void Start () {
		AudioHelper.CreatePlayAudioObject (DynamiteUp);
		transform.position = new Vector3 (transform.position.x, -showUp, transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {
		
		StartCoroutine (ShowUp ());
		
	}
	
	IEnumerator ShowUp(){
		
		to = new Vector3 (transform.position.x, showUp, transform.position.z);
		
		transform.position = Vector3.Lerp(transform.position, to, dynamiteSpeed / 100);
		
		if (transform.position.y <= -1f) {
			
			MouseDestroy();
		}
		
		yield return new WaitForSeconds (3);
		
		showUp = -2f;
	}
	
	void MouseDestroy() {
		Destroy (gameObject);
	}
	
	void OnTriggerEnter(Collider other){
		
		if (other.transform.CompareTag ("Hammer")) {
			
			Debug.Log(other.transform.name);
			AudioHelper.CreatePlayAudioObject (DynamiteDie);
			MouseDestroy ();
		}
	}
}
