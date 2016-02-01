using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour {

	public static bool isStart;
	
	private const float OVER_ACCELERATION = 0.4f;
	
	public static bool move_left_flag;
	public static bool move_right_flag;
	
	// Use this for initialization
	void Start () {
		move_left_flag = false;
		move_right_flag = false;
	}
	
	// Update is called once per frame
	void Update () 
	{		

		if (isStart) {
			Start ();
			isStart = false;
		}

		if (Player.clear_flag && Fade.NowFade == FADE.FADE_OUT)
			return;
		move_left_flag = false;
		move_right_flag = false;
		
		if(Input.acceleration.x < -OVER_ACCELERATION || Input.GetKey(KeyCode.LeftArrow))
			move_left_flag = true;
		if(Input.acceleration.x > OVER_ACCELERATION || Input.GetKey(KeyCode.RightArrow))
			move_right_flag = true;

		// ステージ1-3は一度切るまで動けない
		if (GameMemory.StagePage == 0 && GameMemory.StageGear == 2 && GameMemory.GuideFlag [7] == 0 && MapAction.Trouble == 0) {
			move_left_flag = false;
			move_right_flag = false;
		}
	}
}
