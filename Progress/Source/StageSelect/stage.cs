using UnityEngine;
using System.Collections;

public class stage : MonoBehaviour {
	
	private bool isStagePressed;
	
	public static bool game_start;
	
	public static bool isCheck;

	// ステージ確認画面
	public GameObject CheckWindow;
	
	public static readonly int[] StageTrouble = new int[RollPage.PAGE_MAX * StageAction.MAX_STAGE_INFO]{
		// page1
		0,1,1,2,1,2,3,3,
		// page2
		2,4,5,7,3,4,4,5,
		// page3
		4,4,5,1,4,3,5,4, 
		// page4
		4,3,2,1,4,3,2,5,
		// page5
		2,3,4,2,5,5,3,3,
		// page6
		3,4,2,4,4,4,2,4
	};

	// Use this for initialization
	void Start () {
		GameMemory.StageRock [0] = 1;
	}
	
	// Update is called once per frame
	void Update () {

		PlayBGM.isPlayOpening = true;

		// ゲームへの遷移
		if (CheckStage.isOK) {
			CheckStage.isOK = false;
			CheckStage.isCheck = false;
			PlaySE.isPlaySelect = true;
			Fade.FadeRun(GAME_SCENE.PLAY_GAME);
			game_start = false;
		}

		if (!game_start)
			return;
		// フェード中の操作をはじく
		if (Fade.NowFade != FADE.FADE_NONE) {
			CheckStage.isCheck = false;
			CheckStage.isOK = false;
			CheckStage.isCancel = false;
			return;
		}

		// どれかのステージが選択されていれば実行
		if (game_start) {
			// 選択されたステージが解放されていれば実行
			if(GameMemory.StageRock[GameMemory.StagePage * StageAction.MAX_STAGE_INFO + GameMemory.StageGear] == 1){
				Map_Input.filePath = "StageText/PAGE" + (GameMemory.StagePage+1) + "/Stage" + (GameMemory.StagePage+1) + "_" + (GameMemory.StageGear+1);
				Map_Input.ReadTextData();
				Map_Input.Load();
			}
			// 解放されていなければ初期化
			else
				game_start = false;
		}

		if (!game_start)
			return;
		
		// ステージチェックへの遷移
		game_start = false;

		CheckStage.isCheck = true;
		CheckStage.isOK = false;
		CheckStage.isCancel = false;

		PlaySE.isPlaySelect = true;

		CheckWindow.SetActive (true);
		DrawStagePrev.isStart = true;
	}
	
	/// <summary>スタートボタン押された</summary>
	void StagePressed() {
		if (CheckStage.isCheck || GameGuide.isGuide)
			return;
		// フェード中の操作をはじく
		if (Fade.NowFade != FADE.FADE_NONE) 
			return;
		if (RollStage.difference == 0 && RollPage.difference == 0) {
			if(!RollStage.isMove && !RollPage.isMove)
				isStagePressed = true;
		}
	}
	
	/// <summary>左ボタン離れた</summary>
	void StageReleased() { 
		if (isStagePressed) {
			isStagePressed = false;
			game_start = true;
		}
		isStagePressed = false;
	}
}
