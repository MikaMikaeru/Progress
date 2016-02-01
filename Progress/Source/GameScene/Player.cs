using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public static bool isStart;
	
	public static bool move_flag;
	private bool fall_flag;
	public static bool clear_flag;

	private MAP_GIMMIC beforeGimick;

	// プレイヤーの細かい位置表示用
	public static float MoveDistance;
	// 座標
	public static int X = 0;
	public static int Y = 0;
	// ステージスタート時の座標
	public static int StartX = 0;
	public static int StartY = 0;

	public static float angle = 0;
	
	// Use this for initialization
	void Start () {
		move_flag = false;
		clear_flag = false;

		MoveDistance = 0;

		beforeGimick = MAP_GIMMIC.NONE;
	}
	
	// Update is called once per frame
	void Update () {

		if (isStart) {
			Start ();
			isStart = false;
		}

		// 操作をはじく判定
		if (GameGuide.isGuide || Setting.isSetting)
			return;
		if (Player.clear_flag && Fade.NowFade == FADE.FADE_OUT)
			return;

		fall_flag = false;
		move_flag = false;
		const float MOVE_SPEED = 0.5f;
		const float ROLL_SPEED = 20;
		
		if (!clear_flag) {
			// 落ちる処理
			if (Y > 0 && !MapAction.draw_flag && !MapAction.animation_flag) {
				if (Map_Input.mapdata [Y - 1, X] == MAP_GIMMIC.NONE || Map_Input.mapdata [Y - 1, X] == MAP_GIMMIC.GOAL || Map_Input.mapdata [Y - 1, X] == MAP_GIMMIC.APPEAR) {
					// 位置ずれ修正
					MoveDistance = 0;
					// ステージクリア判定
					if (Map_Input.mapdata [Y - 1, X] == MAP_GIMMIC.GOAL)
						clear_flag = true;
					MapAction.player_draw_flag = true;
					move_flag = true;

					// 行動先のブロックを記憶
					beforeGimick = Map_Input.mapdata [Y - 1, X];
					Y--;
					fall_flag = true;
				}
				else
					fall_flag = false;
			}
			// 落ちているときのプレイヤーの移動を制限
			if(fall_flag)
				return;
			// S極のギミックが起動している時の左右移動の制限
			if (MapAction.S_GimmickCheck ())
				return;

			// 移動処理
			if (!MapAction.animation_flag && !MapAction.draw_flag) {
				// 左への移動
				if (PlayerMove.move_left_flag){
					if(X > 0) {
						if (Map_Input.mapdata [Y, X - 1] == MAP_GIMMIC.NONE || Map_Input.mapdata [Y, X - 1] == MAP_GIMMIC.GOAL || Map_Input.mapdata [Y, X - 1] == MAP_GIMMIC.APPEAR) {
							MoveDistance -= MOVE_SPEED;
							angle += ROLL_SPEED;
							MapAction.player_draw_flag = true;

							if(MoveDistance <= -0.5f){
								// ステージクリア判定
								if (Map_Input.mapdata [Y, X - 1] == MAP_GIMMIC.GOAL){
									MoveDistance = 0;
									clear_flag = true;
								}
								else
									MoveDistance = 0.5f;
								move_flag = true;
								PlaySE.isPlayMove = true;
								// 行動先のブロックを記憶
								beforeGimick = Map_Input.mapdata [Y, X - 1];

								X--;
							}
						}
						else  if((MoveDistance) > 0){
							MoveDistance -= MOVE_SPEED;
							MapAction.player_draw_flag = true;
							angle += ROLL_SPEED;
						}
					}
					else  if((MoveDistance) > 0){
						MoveDistance -= MOVE_SPEED;
						MapAction.player_draw_flag = true;
						angle += ROLL_SPEED;
					}
				}
				// 右への移動
				if (PlayerMove.move_right_flag){
					if(X < MapAction.Map_X - 1) {
						if (Map_Input.mapdata [Y, X + 1] == MAP_GIMMIC.NONE || Map_Input.mapdata [Y, X + 1] == MAP_GIMMIC.GOAL || Map_Input.mapdata [Y, X + 1] == MAP_GIMMIC.APPEAR) {

							MoveDistance += MOVE_SPEED;
							angle -= ROLL_SPEED;
							MapAction.player_draw_flag = true;

							if(MoveDistance >= 0.5f){
								// ステージクリア判定
								if (Map_Input.mapdata [Y, X + 1] == MAP_GIMMIC.GOAL){
									MoveDistance = 0;
									clear_flag = true;
								}
								else
									MoveDistance = -0.5f;
								move_flag = true;
								
								PlaySE.isPlayMove = true;
							
								// 行動先のブロックを記憶
								beforeGimick = Map_Input.mapdata [Y, X + 1];

								X++;
							}
						}
						else if(MoveDistance < 0){
							MoveDistance += MOVE_SPEED;
							MapAction.player_draw_flag = true;
							angle -= ROLL_SPEED;
						}
					}
					else if(MoveDistance < 0){
						MoveDistance += MOVE_SPEED;
						MapAction.player_draw_flag = true;
						angle -= ROLL_SPEED;
					}
				}
			}
			PlayerMove.move_left_flag = false;
			PlayerMove.move_right_flag = false;
		}
	}
}
