using UnityEngine;
using System.Collections;
public enum PLAY_MODE{
	NONE,
	NEW_GAME,
	LOAD_GAME
};

public class TitleAction : MonoBehaviour {
	private bool isNewPressed;
	private bool isLoadPressed;

	public static PLAY_MODE PlayMobe = PLAY_MODE.NONE;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void NewPressed() { 
		if(Fade.NowFade == FADE.FADE_NONE) 
			isNewPressed = true; 
	}

	void NewReleased() { 
		if (isNewPressed)
			PlayMobe = PLAY_MODE.NEW_GAME;
			isNewPressed = false;
	}

	void LoadPressed() { 
		if(Fade.NowFade == FADE.FADE_NONE) 
			isLoadPressed = true; 
	}
	
	void LoadReleased() { 
		if (isLoadPressed)
			PlayMobe = PLAY_MODE.LOAD_GAME;
			isLoadPressed = false;
	}
}
