using UnityEngine;
using System.Collections;

public class SettingAction : MonoBehaviour {
	
	private bool isRestartPressed;
	private bool isRetirePressed;
	private bool isClosePressed;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	// リスタート
	void RestartPressed() { isRestartPressed = true; }
	void RestartReleased() { 
		
		if (isRestartPressed) {
			PlaySE.isPlaySelect = true;

			isRestartPressed = false;			
			Setting.isRestart = true;
		}
		isRestartPressed = false;
	}
	
	// リタイア
	void RetirePressed() { isRetirePressed = true; }
	void RetireReleased() { 
		
		if (isRetirePressed) {
			PlaySE.isPlaySelect = true;
			
			isRetirePressed = false;			
			Setting.isRetire = true;
		}
		isRetirePressed = false;
	}

	// クローズ
	void ClosePressed() { isClosePressed = true; }
	void CloseReleased() { 
		
		if (isClosePressed) {
			PlaySE.isPlaySelect = true;
			
			isClosePressed = false;			
			Setting.isClose = true;
		}
		isClosePressed = false;
	}
}
