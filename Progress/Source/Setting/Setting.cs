using UnityEngine;
using System.Collections;

public class Setting : MonoBehaviour {

	public static bool isStart;

	public static bool isSetting;
	public static bool isRetire;
	public static bool isRestart;
	public static bool isClose;

	public GameObject BGMObj;
	private UISlider BGM_Slider;
	public GameObject SEObj;
	private UISlider SE_Slider;

	private UILabel StageLevel;
	
	// Use this for initialization
	void Start () {
		BGM_Slider = BGMObj.GetComponent ("UISlider")as UISlider;
		BGM_Slider.sliderValue = PlayBGM.max_volume;
		SE_Slider = SEObj.GetComponent ("UISlider")as UISlider;
		SE_Slider.sliderValue = PlaySE.max_volume;

		StageLevel = GameObject.Find ("LEVEL").GetComponent<UILabel> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (isStart) {
			Start ();
			isStart = false;
		}

		if (!isSetting)
			this.gameObject.SetActive (false);
		
		if (isRetire) {
			PlayBGM.isPlayOpening = true;
			Fade.FadeRun (GAME_SCENE.STAGE_SELECT);
			isRetire = false;
			isSetting = false;
			GameGuide.isGuide = false;
		}
		
		if (isRestart) {
			
			MapAction.Map_X = MapAction.Map_Y = 8;
			Map_Input.mapdata = new MAP_GIMMIC[MapAction.Map_Y,MapAction.Map_X];

			// 初期マップに戻す
			Player.X = Player.StartX;
			Player.Y = Player.StartY;

			for (int Y=0; Y<MapAction.Map_Y; Y++) {
				for (int X=0; X<MapAction.Map_X; X++) {
					Map_Input.mapdata [Y, X] = Map_Input.start_mapdata [Y, X];
				}
			}

			for(int i=0;i<MapAction.appear_count;i++){
				Map_Input.appear[i].X = Map_Input.appear[i].startX;
				Map_Input.appear[i].Y = Map_Input.appear[i].startY;
			}
			for(int i=0;i<MapAction.disappear_count;i++){
				Map_Input.disappear[i].X = Map_Input.disappear[i].startX;
				Map_Input.disappear[i].Y = Map_Input.disappear[i].startY;
			}

			MapAction.draw_flag = true;
			isRestart = false;
			isSetting = false;
		}
		
		if (isClose) {
			isClose = false;
			isSetting = false;
		}

		// 各音量の更新
		PlayBGM.max_volume = BGM_Slider.sliderValue;
		PlaySE.max_volume = SE_Slider.sliderValue;
		StageLevel.text = (GameMemory.StagePage+1) + " - " + (GameMemory.StageGear + 1);
	}
}
