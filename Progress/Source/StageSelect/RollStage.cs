using UnityEngine;
using System.Collections;

public class RollStage : MonoBehaviour {
	
	private const int SELECT_ANGLE = 45;

	public static bool isStart;
	
	private int before_stage_gear;
	public static int gear_angle;

	private int move_pos;

	public static int difference;
	public static bool isMove;

	public GameObject StageParticle;
	public GameObject ParticleInitPos;
	public GameObject SmokeParticle;
	
	public GameObject ExtraObject;

	private int ParticleCount;

	private int WorkStageGear;

	// Use this for initialization
	void Start () {
		transform.eulerAngles = new Vector3 (0, 0, gear_angle = GameMemory.StageGear * SELECT_ANGLE);
		StageParticle.transform.localPosition = ParticleInitPos.transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {

		if (isStart) {
			Start ();
			isStart = false;
		}

		if (CheckStage.isCheck || GameGuide.isGuide)
			return;

		isMove = false;

		// エクストラステージ解放のフラグが立っていると操作を出来ないようにする
		if (StageAction.ExtraOpenFlag) {
			// エクストラステージが解放された時の演出
			if(StageParticle.transform.particleSystem.isStopped)
				StageParticle.transform.particleSystem.Play();
			// stage_gearを7に設定、回転し終えたらパーティクルの移動
			if(gear_angle == (WorkStageGear = StageAction.MAX_STAGE_INFO-1) * SELECT_ANGLE){
				float differenceX,differenceY;
				differenceX = (StageParticle.transform.position.x - ExtraObject.transform.position.x) / 10;
				differenceY = (StageParticle.transform.position.y - ExtraObject.transform.position.y) / 10;
				StageParticle.transform.Translate(-differenceX,-differenceY,0);

				// パーティクルの位置を7の位置に移動できたら煙のパーティクルを起動
				if(differenceX + differenceY < 0.01f){
					if(SmokeParticle.transform.particleSystem.isStopped){
						StageParticle.transform.particleSystem.Stop();
						SmokeParticle.transform.particleSystem.Play();
						ParticleCount = 0;
					}
					else{
						ParticleCount++;
						// 調度良いタイミングで次の処理へ
						if(ParticleCount > 10){
							// 次回からの演出を制限
							GameMemory.ExtraAnimationFlag [GameMemory.StagePage] = 1;
							GameMemory.StageRock [GameMemory.StagePage * StageAction.MAX_STAGE_INFO + StageAction.MAX_STAGE_INFO-1] = 1;
							PlaySE.isPlayGimmic = true;
							// エクストラステージを含めた描画フラグOn
							StageAction.isDrawStageNumber = true;
							StageInfo.DrawInfoFlag = true;
							StageAction.ExtraOpenFlag = false;
						}
					}
				}
			}

		} else {
			// エクストラステージが解放されない通常時の処理
			// PC用
			if (Input.GetKey (KeyCode.UpArrow)) {
				gear_angle++;
				isMove = true;
			}
			if (Input.GetKey (KeyCode.DownArrow)) {
				gear_angle--;
				isMove = true;
			}
			
			// スマホ用
			if (TouchAction.direction [0].y != 0 && TouchAction.startPos[0].x < Screen.width/2 && TouchAction.startPos[0].x > Screen.width/4) {
				gear_angle -= move_pos - (int)TouchAction.direction [0].y/5;
				isMove = true;
			}
			move_pos = (int)TouchAction.direction [0].y/5;
			
			// 角度を0~359に調整
			if (gear_angle < 0)
				gear_angle = 360 + gear_angle;
			if (gear_angle != 0)
				gear_angle = gear_angle % 360;
			
			// 選択されたステージに回転軸を補正
			if (Mathf.Abs (gear_angle % SELECT_ANGLE) < SELECT_ANGLE / 2)
				WorkStageGear = gear_angle / SELECT_ANGLE;
			else 
				WorkStageGear = ((gear_angle / SELECT_ANGLE) + 1);
		}

		// 回転更新処理
		if (isMove)
			transform.eulerAngles = new Vector3 (0, 0, gear_angle);
		else {

			difference = WorkStageGear * SELECT_ANGLE - gear_angle;
			if(difference > 180)
				difference -= 360;

			if(difference == 0){
				transform.eulerAngles = new Vector3 (0, 0, gear_angle = WorkStageGear * SELECT_ANGLE);
				GameMemory.StageGear = WorkStageGear%StageAction.MAX_STAGE_INFO;
				if(before_stage_gear != GameMemory.StageGear){
					StageInfo.DrawInfoFlag = true;
					before_stage_gear = GameMemory.StageGear;
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
