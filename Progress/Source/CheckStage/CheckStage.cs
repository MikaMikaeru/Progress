using UnityEngine;
using System.Collections;

public class CheckStage : MonoBehaviour {
	
	public static bool isCheck;
	public static bool isOK;
	public static bool isCancel;
	
	// Use this for initialization
	void Start () {
		isOK = false;
		isCancel = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (!isCheck) {
			// 確認が完了したら自身のアクティブを切る
			PlaySE.isPlaySelect = true;
			this.gameObject.SetActive(false);
		}
		
		if (isOK)
			isCheck = false;
		
		if (isCancel)
			isCheck = false;
	}
}
