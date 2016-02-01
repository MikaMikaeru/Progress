using UnityEngine;
using System.Collections;

public enum FADE
{
	FADE_NONE,
	FADE_IN,
	FADE_OUT,
	FADE_ANIMATION
}

public class Fade : MonoBehaviour {

	// フェード後のシーン
	public static GAME_SCENE NextScene;
	// 現在のフェード
	public static FADE NowFade;
	// オブジェクト情報
	public GameObject TopBackground;
	public GameObject BottomBackground;
	public GameObject TopGear;
	public GameObject BottomGear;

	// 移動速度
	private const float MOVE_SPEED = 0.1f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		switch (NowFade) {
		case FADE.FADE_OUT:
			if(OutAnimation()){
				NowFade = FADE.FADE_ANIMATION;
			}
			break;
		case FADE.FADE_ANIMATION:
			if(FadeAnimation()){
				GameMain.NowScene = NextScene;
				NowFade = FADE.FADE_IN;
			}
			break;
		case FADE.FADE_IN:
			if(FadeIn()){
				TopGear.transform.localEulerAngles = new Vector3(0,0,0);
				BottomGear.transform.localEulerAngles = new Vector3(0,0,0);
				NowFade = FADE.FADE_NONE;
			}
			break;
		case FADE.FADE_NONE:
			break;
		}
	}

	public static void FadeRun(GAME_SCENE scene){
		NextScene = scene;
		NowFade = FADE.FADE_OUT;
	}

	// フェードアウトアニメーション(継続:false 終了:true)
	private bool OutAnimation(){
		TopBackground.transform.Translate (0, -MOVE_SPEED, 0);
		TopGear.transform.Translate (0, -MOVE_SPEED, 0);
		BottomBackground.transform.Translate (0, MOVE_SPEED, 0);
		BottomGear.transform.Translate (0, MOVE_SPEED, 0);

		if (TopGear.transform.position.y > 0)
			return false;
		else{
			TopGear.transform.position = BottomGear.transform.position = new Vector3(0,0,TopGear.transform.position.z);
			return true;
		}
	}

	// フェードアニメーション(継続:false 終了:true)
	private bool FadeAnimation(){
		TopGear.transform.localEulerAngles += new Vector3(0,0,5);
		BottomGear.transform.localEulerAngles += new Vector3(0,0,5);
		if (TopGear.transform.localEulerAngles.z < 180)
			return false;
		else 
			return true;
	}

	// フェードアニメーション(継続:false 終了:true)
	private bool FadeIn(){
		TopBackground.transform.Translate (0, MOVE_SPEED, 0);
		TopGear.transform.Translate (0, MOVE_SPEED, 0);
		BottomBackground.transform.Translate (0, -MOVE_SPEED, 0);
		BottomGear.transform.Translate (0, -MOVE_SPEED, 0);
		
		if (TopBackground.transform.localPosition.y < 1.1f)
			return false;
		else {
			TopGear.transform.localPosition = new Vector3 (0, 0.565f, TopGear.transform.position.z);
			BottomGear.transform.localPosition = new Vector3 (0, -0.565f, BottomGear.transform.position.z);
			TopBackground.transform.localPosition = new Vector3 (0, 1.1f, TopBackground.transform.position.z);
			BottomBackground.transform.localPosition = new Vector3 (0, -1.1f, BottomBackground.transform.position.z);
			return true;
		}
	}
}
