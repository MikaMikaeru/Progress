using UnityEngine;
using System.Collections;

public enum GAME_SCENE{
	TITLE,
	STAGE_SELECT,
	PLAY_GAME,
	RESULT
};

public class GameMain : MonoBehaviour {

	public static GAME_SCENE NowScene;
	private GAME_SCENE BeforeScene;

	// 各シーン
	public GameObject Title;
	public GameObject StageSelect;
	public GameObject PlayGame;
	public GameObject PlayGameUI;
	public GameObject Result;

	// ガイド
	public GameObject GameGuide;
	public static bool GuideFlag;

	// 各パーティクル
	public GameObject StageSelectParticle;
	public GameObject PlayGameParticle;
	public GameObject ResultParticle;
	// Use this for initialization
	void Start () {
		NowScene = GAME_SCENE.TITLE;
	}
	
	// Update is called once per frame
	void Update () {

		switch (NowScene) {
		case GAME_SCENE.TITLE:
			PlayBGM.isPlayOpening = true;
			Title.SetActive(true);
			StageSelect.SetActive(false);
			StageSelectParticle.SetActive(false);
			PlayGame.SetActive(false);
			PlayGameUI.SetActive(false);
			PlayGameParticle.SetActive(false);
			Result.SetActive(false);
			ResultParticle.SetActive(false);
			break;
		case GAME_SCENE.STAGE_SELECT:
			PlayBGM.isPlayOpening = true;
			Title.SetActive(false);
			StageSelect.SetActive(true);
			StageSelectParticle.SetActive(true);
			PlayGame.SetActive(false);
			PlayGameUI.SetActive(false);
			PlayGameParticle.SetActive(false);
			Result.SetActive(false);
			ResultParticle.SetActive(false);
			break;
		case GAME_SCENE.PLAY_GAME:
			PlayBGM.isPlayGamePlay = true;
			Title.SetActive(false);
			StageSelect.SetActive(false);
			StageSelectParticle.SetActive(false);
			PlayGame.SetActive(true);
			PlayGameUI.SetActive(true);
			PlayGameParticle.SetActive(true);
			Result.SetActive(false);
			ResultParticle.SetActive(false);
			break;
		case GAME_SCENE.RESULT:
			PlayBGM.isPlayGamePlay = true;
			Title.SetActive(false);
			StageSelect.SetActive(false);
			StageSelectParticle.SetActive(false);
			PlayGame.SetActive(false);
			PlayGameUI.SetActive(false);
			PlayGameParticle.SetActive(false);
			Result.SetActive(true);
			ResultParticle.SetActive(true);
			break;
		}

		if (BeforeScene != NowScene) {
			init ();
			BackgroundDraw.DrawFlag = true;
		}
		
		BeforeScene = NowScene;

		GameGuide.SetActive (GuideFlag);
	}

	void init(){
		switch (NowScene) {
		case GAME_SCENE.TITLE:
			break;
		case GAME_SCENE.STAGE_SELECT:
			StageAction.isStart = true;
			RollPage.isStart = true;
			RollStage.isStart = true;
			StageInfo.isStart = true;
			break;
		case GAME_SCENE.PLAY_GAME:
			MapAction.isStart = true;
			Player.isStart = true;
			PlayerMove.isStart = true;
			CameraAction.isStart = true;
			break;
		case GAME_SCENE.RESULT:
			result.isStart = true;
			break;
		}
	}
}
