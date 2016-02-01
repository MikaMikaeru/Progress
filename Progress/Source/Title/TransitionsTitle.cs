using UnityEngine;
using System.Collections;

public class TransitionsTitle : MonoBehaviour {
	
	private bool isButton_startPressed;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	/// <summary>スタートボタン押された</summary>
	void Button_startPressed() { 

		// 更新をはじく処理
		if (Fade.NowFade != FADE.FADE_NONE || stage.game_start || GameGuide.isGuide || StageAction.ExtraOpenFlag || CheckStage.isCheck)
			return;

		isButton_startPressed = true; 
	}
	
	/// <summary>左ボタン離れた</summary>
	void Button_startReleased() { 

		if (isButton_startPressed) {
			isButton_startPressed = false;
			
			PlaySE.isPlaySelect = true;
			Fade.FadeRun(GAME_SCENE.TITLE);
		}
		isButton_startPressed = false;
	}
}
