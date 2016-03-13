using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MouseBehaviour : MonoBehaviour {
	
	public float mouseSpeed = 1;
	public float showUp = 1;
	// Use this for initialization

	private Vector3 from;
	private Vector3 to;

	public AudioClip MouseDie;
	public AudioClip MouseUp;

	void Start () {
		AudioHelper.CreatePlayAudioObject (MouseUp);
		transform.position = new Vector3 (transform.position.x, -showUp, transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {

		StartCoroutine (ShowUp ());

	}

	IEnumerator ShowUp(){

		to = new Vector3 (transform.position.x, showUp, transform.position.z);
		
		transform.position = Vector3.Lerp(transform.position, to, mouseSpeed / 100);
		
		if (transform.position.y <= -1.5f) {
			
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
		
			GameManager.delay += 2f;
			Debug.Log(other.transform.name);
			AudioHelper.CreatePlayAudioObject (MouseDie);
			MouseDestroy ();
			if(this.transform.CompareTag("Mouse"))
			{
				GameObject scoreText = GameObject.Find("scoretext");
				GameManager.Score +=10;
				scoreText.GetComponent<Text>().text = (GameManager.Score).ToString();
			}

			if(this.transform.CompareTag("Dynamite"))
			{
				GameObject scoreText = GameObject.Find("lifetext");
				GameManager.LifePoint -= 1;
				scoreText.GetComponent<Text>().text = (GameManager.LifePoint).ToString();
			}


		}
	}
}
