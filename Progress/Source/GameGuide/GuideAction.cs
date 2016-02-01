using UnityEngine;
using System.Collections;

public class GuideAction : MonoBehaviour {
	
	private bool isTapedDisplay;
	private bool isReleasedDisplay;

	public static bool isPress;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		if (isTapedDisplay && isReleasedDisplay) {
			isTapedDisplay = isReleasedDisplay = false;
			isPress = true;
		}
	}

	void DisplayPressed() { 
		isTapedDisplay = true;
	}
	
	void DisplayReleased() { 
		isReleasedDisplay = true;
	}
}
