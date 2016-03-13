using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public static int LifePoint = 3;
	public static int Score = 0;

	public bool mouseUse = true;

	public Transform hammer;
	public float distance = 1.5f;
	public Transform[] holePlace;
	public Transform[] objects;

	public static float delay;
	
	public int DynamiteAvail = 1;
	public int MouseAvail = 2;
	public int spaceAvailable;

	private Animator anim;
	private HashID hash;

	public float mousePersentase = 80;

	public static bool GameScene = true;

	public Sprite[] countSprite;

	public bool ready = false;

	private int count = 0;
	private int countObj = 0;
	private ArrayList available = new ArrayList ();

	private GameObject panelCountDown;

	// Use this for initialization
	void Start () {

		BaseManager.globalIntroMusic = AudioHelper.CreateGetFadeAudioObject (
			BaseManager.instance.introMusic, 
			true, 
			BaseManager.instance.fadeClip,
			"Audio-GlobalMusic");

		StartCoroutine (AudioHelper.FadeAudioObject (BaseManager.globalIntroMusic, 0.25f));

		anim = hammer.gameObject.GetComponent<Animator>();
		hash = GameObject.Find ("GameManager").GetComponent<HashID> ();
		Score = 0;
		//Instantiate (hammer);

		panelCountDown = GameObject.Find ("Countdown");

		StartCoroutine(AudioHelper.FadeAudioObject(BaseManager.globalIntroMusic, 0.25f));
	}
	
	// Update is called once per frame
	void Update () {

		//GameScene = true;

		if (GameScene) {

			DoCountdown();

			if(ready) {
			
				PlayOn();
			}
		}
		else
		{
			delay = Time.fixedTime;
			ready = false;
			panelCountDown.SetActive(true);
			panelCountDown.GetComponent<Image> ().sprite = countSprite [4];
		}
	}

	int FindAvailableHole() {

		for (int i = 0; i < holePlace.Length; i++) {
		
			if(holePlace[i].childCount < 1)
			{
				available.Add(holePlace[i]);
			}
		} 

		return available.Count;
	}

	void DoCountdown() {

		if(Time.fixedTime > delay && count >= 4)
		{
			count = 0;
			ready = true;
			panelCountDown.SetActive(false);
		}

		if (Time.fixedTime > delay && count < 4 && !ready) {

			panelCountDown.GetComponent<Image> ().sprite = countSprite [count];
			delay += 1;
			count++;
		}
		
		Debug.Log (panelCountDown.name);
	}

	void PlayOn() {

		if (mouseUse) {
			MouseInput ();
		}
		
		spaceAvailable = FindAvailableHole ();
		
		int rand = Random.Range (0, spaceAvailable);
		
		if (Time.fixedTime > delay) {

			Transform hole = (Transform)available [rand];

			int randObj = Random.Range(0, objects.Length);



			if((mousePersentase * 10 / 100) != countObj && randObj == 0 && spaceAvailable > holePlace.Length - MouseAvail) {

				Transform obj = Instantiate(objects[0], hole.position, Quaternion.identity) as Transform;
				obj.parent = hole;

				countObj++;
			}
			else if(((100 - mousePersentase) * 10 / 100) != countObj && randObj == 1 && spaceAvailable > holePlace.Length - DynamiteAvail)
			{
				Transform obj = Instantiate(objects[1], hole.position, Quaternion.identity) as Transform;
				obj.parent = hole;

				countObj++;
			}

			if(countObj > 10) {
				countObj = 0;
			}


		}
		
		available.Clear ();
	}
	
	void MouseInput(){

		Ray ray;

		RaycastHit hit;

		Transform hamRay = hammer.FindChild("HammerRay");

		if (Physics.Raycast (hamRay.position, hamRay.forward, out hit, 1000)) {
				
			Debug.DrawRay (hamRay.position, hamRay.forward * 1000, Color.red);

			if(hit.collider.CompareTag("Hole")){			
				Debug.Log("Target Position: "+ hit.collider.gameObject.transform.position);

				distance = hit.collider.transform.position.z + 19;
			}
			else
			{
				distance = 1.5f;
			}
		}

		/*
		for (int i = 0; i < holePlace.Length; i++) {
		
			float closer = Mathf.Abs(holePlace[i].position.x - ray.origin.x);

			if(closer < 1) {
			
				ray.direction = holePlace[i].position;
			}
		}
		*/
		ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		Vector3 point = ray.origin + (ray.direction * distance);
		hammer.position = point;
		
		if (Input.GetButtonDown ("Fire1")) {

			anim.SetTrigger(hash.hammerTrigger);
		}
	}
}
