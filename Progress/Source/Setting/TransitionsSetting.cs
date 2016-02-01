using UnityEngine;
using System.Collections;

public class TransitionsSetting : MonoBehaviour {

	public GameObject SettingObj;
	
	private bool isButton_startPressed;
	
	// Use this for initialization
	void Start () {
		this.isButton_startPressed = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	/// <summary>スタートボタン押された</summary>
	void Button_startPressed() { this.isButton_startPressed = true; }

	/// <summary>左ボタン離れた</summary>
	void Button_startReleased() { 
		if (Setting.isSetting || GameGuide.isGuide || MapAction.animation_flag)
			return;
		// フェード中の操作をはじく
		if (Fade.NowFade != FADE.FADE_NONE)
			return;
		
		if (this.isButton_startPressed) {
			this.isButton_startPressed = false;
			Setting.isSetting = true;
			PlaySE.isPlaySelect = true;
			SettingObj.SetActive (true);
			Setting.isStart = true;
		}
		this.isButton_startPressed = false;
	}
}
