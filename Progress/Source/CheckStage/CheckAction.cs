using UnityEngine;
using System.Collections;

public class CheckAction : MonoBehaviour {
	
	private bool isOKPressed;
	private bool isCancelPressed;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	// OK
	void OKPressed() { isOKPressed = true; }
	void OKReleased() { 
		
		if (isOKPressed) {
			isOKPressed = false;			
			CheckStage.isOK = true;
		}
		isOKPressed = false;
	}
	
	// キャンセル
	void CancelPressed() { isCancelPressed = true; }
	void CancelReleased() { 
		
		if (isCancelPressed) {
			isCancelPressed = false;			
			CheckStage.isCancel = true;
		}
		isCancelPressed = false;
	}
}
