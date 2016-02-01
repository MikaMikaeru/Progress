using UnityEngine;
using System.Collections;

public enum SWIPE{
	NONE,
	SWIPE_DOWN,
	SWIPE_UP,
	SWIPE_LEFT,
	SWIPE_RIGHT
}

public enum ZOOM{
	NONE,
	ZOOM_IN,
	ZOOM_OUT
}

public class TouchAction : MonoBehaviour {
	
	// スワイプされたと判断する距離
	private const int JUDGE_DIRECTION = 300;
	// ボタンが押されたと判断するスワイプ距離
	public const int BUTTON_DISTANCE = 30;
	// 反応する指の数
	private const int MAX_TAP = 2;
	// ボタン用座標
	public static Touch pressPos;
	// スワイプ開始時の座標
	public static Vector2[] startPos;
	// スワイプの移動距離
	public static Vector2[] direction;
	// スワイプの終了時の座標
	public Vector2[] endPos;
	// 確定したスワイプ方向
	public static SWIPE swipe;
	// ２本の指の距離
	private float distance;
	private float oldDistance;
	// ズーム情報
	public static ZOOM zoom;
	
	// Use this for initialization
	void Start () {
		swipe = SWIPE.NONE;
		startPos = new Vector2[MAX_TAP];
		direction = new Vector2[MAX_TAP];
		endPos = new Vector2[MAX_TAP];
	}
	
	// Update is called once per frame
	void Update () {

		zoom = ZOOM.NONE;

		for (int i=0; i<MAX_TAP; i++) {
			// 指一本での処理
			if (Input.touchCount == i+1) {
				var touch = Input.GetTouch (i);
				
				switch (touch.phase) {
					// 開始
				case TouchPhase.Began:
					startPos[i] = touch.position;
					break;

					// 移動中
				case TouchPhase.Moved:
					direction[i] = touch.position - startPos[i];
					break;
					
					// 終了時
				case TouchPhase.Ended:
					endPos[i] = touch.position;
					if(i == 0)
						swipe = GetSwipe (i);
					startPos[i] = new Vector2 (0, 0);
					direction[i] = new Vector2 (0, 0);
					endPos[i] = new Vector2 (0, 0);
					break;
				}
			}

		}

		if (Input.touchCount == 0) {
			swipe = SWIPE.NONE;
			distance = 0;
		}
		if (Input.touchCount >= 2) {				
			distance = Vector2.Distance (Input.GetTouch (0).position, Input.GetTouch (1).position);
		
			if(oldDistance - distance < 0)
				zoom = ZOOM.ZOOM_IN;
			if(oldDistance - distance > 0)
				zoom = ZOOM.ZOOM_OUT;

			oldDistance = distance;

			swipe = SWIPE.NONE;
		}

	}
	
	SWIPE GetSwipe(int TouchNumber){
		// 上下の判定
		if(Mathf.Abs(direction[TouchNumber].x) < Mathf.Abs(direction[0].y)){
			if(direction[TouchNumber].y < -JUDGE_DIRECTION){
				return SWIPE.SWIPE_DOWN;
			}
			if(direction[TouchNumber].y > JUDGE_DIRECTION){
				return SWIPE.SWIPE_UP;
			}
		}
		// 左右の判定
		else{
			if(direction[TouchNumber].x < -JUDGE_DIRECTION){
				return SWIPE.SWIPE_LEFT;
			}
			if(direction[TouchNumber].x > JUDGE_DIRECTION){
				return SWIPE.SWIPE_RIGHT;
			}
		}
		return SWIPE.NONE;
	}
	
	public static void GetTouch(){
		pressPos = Input.GetTouch (0);
	}
}
