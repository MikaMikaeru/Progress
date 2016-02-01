using UnityEngine;
using System.Collections;

public class TransitionsStageSelect : MonoBehaviour {
	
	private bool isButton_startPressed;
	
	// Use this for initialization
	void Start () {
		this.isButton_startPressed = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	/// <summary>スタートボタン押された</summary>
	void Button_startPressed() { 
		if(Fade.NowFade == FADE.FADE_NONE) 
			isButton_startPressed = true;
	}
	
	/// <summary>左ボタン離れた</summary>
	void Button_startReleased() { 
		if (this.isButton_startPressed) {
			this.isButton_startPressed = false;
			PlaySE.isPlaySelect = true;
			Fade.FadeRun(GAME_SCENE.STAGE_SELECT);
		}
		this.isButton_startPressed = false;
	}
}
