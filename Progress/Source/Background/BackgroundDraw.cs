using UnityEngine;
using System.Collections;

public class BackgroundDraw : MonoBehaviour {
	// スプライト情報
	public GameObject Background;

	public static bool DrawFlag;

	// Use this for initialization
	void Start () {
		DrawFlag = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (DrawFlag) {
			Draw ();
			DrawFlag = false;
		}
	}

	void Draw (){
		// 背景の描画
		if (GameMain.NowScene == GAME_SCENE.STAGE_SELECT)
			Background.GetComponent<UISprite> ().spriteName = "StageBackground";
		else
			Background.GetComponent<UISprite> ().spriteName = "RBackground";
		
		// 色の変更
		Background.GetComponent<UISprite>().color = GameMemory.BackgroundColor[GameMemory.StagePage];
	}
}
