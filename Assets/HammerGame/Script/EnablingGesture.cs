using UnityEngine;
using System.Collections;
using Leap;

public class EnablingGesture : MonoBehaviour {

	Leap.Controller controller;

	public bool swipeRight = false;
	public bool swipeLeft = false;
	// Use this for initialization
	void Start () {

		controller = new Controller ();
		controller.EnableGesture(Gesture.GestureType.TYPECIRCLE);
		controller.EnableGesture(Gesture.GestureType.TYPEINVALID);
		controller.EnableGesture(Gesture.GestureType.TYPEKEYTAP);
		controller.EnableGesture(Gesture.GestureType.TYPESCREENTAP);
		controller.EnableGesture (Gesture.GestureType.TYPE_SWIPE);

		controller.Config.SetFloat("Gesture.Swipe.MinLength", 200.0f);
		controller.Config.SetFloat("Gesture.Swipe.MinVelocity", 750f);
		controller.Config.Save();
	}
	
	// Update is called once per frame
	void Update () {
	
		Frame frame = controller.Frame ();

		foreach (Gesture gesture in frame.Gestures())
		{
			switch(gesture.Type)
			{
			case(Gesture.GestureType.TYPECIRCLE):
			{
				Debug.Log("Circle gesture recognized.");
				break;
			}
			case(Gesture.GestureType.TYPEINVALID):
			{
				Debug.Log("Invalid gesture recognized.");
				break;
			}
			case(Gesture.GestureType.TYPEKEYTAP):
			{
				Debug.Log("Key Tap gesture recognized.");
				break;
			}
			case(Gesture.GestureType.TYPESCREENTAP):
			{
				Debug.Log("Screen tap gesture recognized.");
				break;
			}
			case(Gesture.GestureType.TYPESWIPE):
			{
				Debug.Log("Swipe gesture recognized.");
				SwipeGesture swipe = new SwipeGesture(gesture);

				Vector swipeDirection = swipe.Direction;
				Debug.Log(swipeDirection);
				if(swipeDirection.x < 0) {
					swipeRight = true;

				}
				if(swipeDirection.x > 0){
					swipeRight = false;
				}

				break;
			}
			default:
			{
				break;
			}
			}
		}

		DoSwipe (swipeRight);
	}

	void DoSwipe(bool right){

		if (right) {
			//GameManager.GameScene = true;
			Vector3 gamePos = new Vector3(47, Camera.main.transform.position.y, Camera.main.transform.position.z);
			
			if (Camera.main.transform.position.x <= gamePos.x - 0.5f) {
				
				Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, gamePos, 1 * Time.deltaTime);
			}
			else
			{
				GameManager.GameScene = true;
			}
		}
		if (!right) {
			GameManager.GameScene = false;
		
			Vector3 menuPos = new Vector3(0, Camera.main.transform.position.y, Camera.main.transform.position.z);
			
			if (Camera.main.transform.position != menuPos) {
				
				Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, menuPos, 1 * Time.deltaTime);
			}
		}
	}
}
