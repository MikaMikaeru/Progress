using UnityEngine;
using System.Collections;

public class DrawStagePrev : MonoBehaviour {

	public static bool isStart;

	private bool isDraw;

	// Use this for initialization
	void Start () {
		isDraw = false;
	}
	
	// Update is called once per frame
	void Update () {

		if (isStart) {
			Start ();
			isStart = false;
		}

		if (!isDraw) {
			// スプライトの変更
			transform.GetComponent<UISprite> ().spriteName = "stage" + (GameMemory.StagePage+1) + "-" + (GameMemory.StageGear+1);;
			isDraw = true;
		}
	}
}
