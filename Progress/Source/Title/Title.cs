using UnityEngine;
using System.Collections;

public class Title : MonoBehaviour {
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		PlayBGM.isPlayOpening = true;
		switch (TitleAction.PlayMobe) {
		case PLAY_MODE.NEW_GAME:
			GameMemory.Init ();
			PlaySE.isPlaySelect = true;
			Fade.FadeRun (GAME_SCENE.STAGE_SELECT);
			TitleAction.PlayMobe = PLAY_MODE.NONE;
			break;
		case PLAY_MODE.LOAD_GAME:
			GameMemory.Load ();
			PlaySE.isPlaySelect = true;
			Fade.FadeRun (GAME_SCENE.STAGE_SELECT);
			TitleAction.PlayMobe = PLAY_MODE.NONE;
			break;
		default:
			break;
		}
	}
}
