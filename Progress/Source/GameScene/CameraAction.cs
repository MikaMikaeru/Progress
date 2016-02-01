using UnityEngine;
using System.Collections;

public class CameraAction : MonoBehaviour {

	public static bool isStart;

	private bool isPressed;
	public static bool CameraOn;

	// アイコン
	public GameObject CameraObj;

	// Use this for initialization
	void Start () {
		CameraOn = false;
		CameraObj.GetComponent<UISprite>().color = new Color(1,1,1);
	}
	
	// Update is called once per frame
	void Update () {
		if (isStart) {
			Start ();
			isStart = false;
		}
	}

	void Button_startPressed() { 
		// 更新をはじく処理
		if (Fade.NowFade != FADE.FADE_NONE || CheckStage.isCheck || stage.game_start || GameGuide.isGuide)
			return;

		isPressed = true; 
	}
	
	/// <summary>左ボタン離れた</summary>
	void Button_startReleased() { 
		if (MapAction.animation_flag)
			return;
		if (isPressed) {
			PlaySE.isPlaySelect = true;
			CameraOn ^= true;
			if(CameraOn)
				CameraObj.GetComponent<UISprite>().color = new Color(0.5f,0.5f,0.5f);
			else{
				CameraObj.GetComponent<UISprite>().color = new Color(1,1,1);
				MapAction.CenterX = MapAction.CenterY = 0;
			}
		}
		isPressed = false;
	}
}
