using UnityEngine;
using System.Collections;

public class RollPage : MonoBehaviour {
	
	private const int SELECT_ANGLE = 60;

	public static bool isStart;
	
	// 最大のページ
	public const int PAGE_MAX = 6;
	private int before_stage_page;
	public static int gear_angle;

	public static bool isMove;
	private int move_pos;
	public static int difference;
	
	private int WorkStagePage;

	// Use this for initialization
	void Start () {
		transform.eulerAngles = new Vector3 (0, 0, gear_angle = GameMemory.StagePage * SELECT_ANGLE);
	}

	// Update is called once per frame
	void Update () {
		if (isStart) {
			Start ();
			isStart = false;
		}
		if (CheckStage.isCheck || GameGuide.isGuide || StageAction.ExtraOpenFlag)
			return;

		isMove = false;


		// PC用
		if (Input.GetKey (KeyCode.LeftArrow)) {
			gear_angle++;
			isMove = true;
		}
		if (Input.GetKey (KeyCode.RightArrow)) {
			gear_angle--;
			isMove = true;
		}
		// スマホ用
		if (TouchAction.direction [0].y != 0 && TouchAction.startPos [0].x < Screen.width / 4) {
			gear_angle -= move_pos - (int)TouchAction.direction [0].y / 5;
			isMove = true;
		}
		move_pos = (int)TouchAction.direction [0].y/5;

		// 角度を0~359に調整
		if (gear_angle < 0)
			gear_angle = 360 + gear_angle;
		if (gear_angle != 0)
			gear_angle = gear_angle % 360;


		// 選択されたページに回転軸を補正
		if (Mathf.Abs(gear_angle % SELECT_ANGLE) < SELECT_ANGLE/2)
			WorkStagePage = gear_angle / SELECT_ANGLE;
		else
			WorkStagePage = (gear_angle / SELECT_ANGLE) + 1;
		
		// 回転更新処理
		if (isMove)
			transform.eulerAngles = new Vector3 (0, 0, gear_angle);
		else {
			difference = WorkStagePage * SELECT_ANGLE - gear_angle;
			if(difference > 180)
				difference -= 360;
			if(difference == 0){
				transform.eulerAngles = new Vector3 (0, 0, gear_angle = WorkStagePage * SELECT_ANGLE);
				GameMemory.StagePage = WorkStagePage%PAGE_MAX;
				if(GameMemory.StagePage != before_stage_page){
					StageAction.isDrawStageNumber = true;
					StageInfo.DrawInfoFlag = true;
					before_stage_page = GameMemory.StagePage;
					BackgroundDraw.DrawFlag = true;
				}
			}
			else{
				if(difference > 0)
					difference = -1;
				else
					difference = 1;
				transform.eulerAngles = new Vector3 (0, 0, gear_angle = gear_angle-difference);
			}
		}
	}
}
