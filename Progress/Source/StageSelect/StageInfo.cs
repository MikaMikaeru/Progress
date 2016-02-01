using UnityEngine;
using System.Collections;

public class StageInfo : MonoBehaviour {

	public static bool isStart;

	// 最大獲得ランク
	public const int MAX_RANK = 3;
	
	public static bool DrawInfoFlag;

	public GameObject StagePrev;

	public GameObject[] StageRank = new GameObject[MAX_RANK];

	// 総獲得ランク表示用
	public UILabel rank_number;

	public GameObject[] InfoGear = new GameObject[StageAction.MAX_STAGE_INFO-1];
	public GameObject DefaultGear;
	private readonly float[] GEAR_ROTATION = new float[StageAction.MAX_STAGE_INFO-1]{-0.668f,-0.5f,-0.5f,-0.668f,-0.5f,0.668f,-0.5f};


	// Use this for initialization
	void Start () {
		DrawInfoFlag = true;
		DefaultGear.transform.eulerAngles = new Vector3 (0,0,0);
		for (int i=0; i<StageAction.MAX_STAGE_INFO-1; i++) {
			// 回転処理
			InfoGear[i].transform.eulerAngles = new Vector3 (0, 0, 0);
		}
		InfoGear[1].transform.eulerAngles = new Vector3 (0, 0, 354.1733f);
		InfoGear[5].transform.eulerAngles = new Vector3 (0, 0, 42.36017f);
	}
	
	// Update is called once per frame
	void Update () {

		if (isStart) {
			Start ();
			isStart = false;
		}
		
		GearUpdate ();

		if (DrawInfoFlag) {
			DrawStageInfo ();
			DrawRank();
			DrawInfoFlag = false;
		}
	}

	// ステージ情報の描画
	void DrawStageInfo(){
		GameMemory.StageGear = GameMemory.StageGear % StageAction.MAX_STAGE_INFO;
		if(GameMemory.StageRock[GameMemory.StagePage * StageAction.MAX_STAGE_INFO + GameMemory.StageGear] == 1){
			StagePrev.SetActive(true);
			StagePrev.GetComponent<UISprite> ().spriteName = "stage" + (GameMemory.StagePage+1) + "-" + (GameMemory.StageGear+1);
		}
		else
			StagePrev.SetActive(false);
	}

	// 獲得ランクの描画
	void DrawRank(){
		// 総獲得ランクの描画
		rank_number.text = "" + GameMemory.TotalRank;

		// ステージ獲得ランクの描画
		for(int i=0;i<MAX_RANK;i++){
			if(GameMemory.StageRock[GameMemory.StagePage * StageAction.MAX_STAGE_INFO + GameMemory.StageGear] == 1){
				StageRank[i].SetActive(true);
				if(GameMemory.StageRank[GameMemory.StagePage * StageAction.MAX_STAGE_INFO + GameMemory.StageGear] > i)
					StageRank[i].GetComponent<UISprite> ().spriteName = "RankGear";
				else
					StageRank[i].GetComponent<UISprite> ().spriteName = "NoRankGear";
			}
			else
				StageRank[i].SetActive(false);
		}
	}

	// 歯車の更新
	void GearUpdate (){

		DefaultGear.transform.eulerAngles += new Vector3 (0,0,0.5f);

		int count = 0;
		for (int i=0; i<StageAction.MAX_STAGE_INFO-1; i++) {
			if (GameMemory.StageRank [GameMemory.StagePage * StageAction.MAX_STAGE_INFO + i] >= MAX_RANK)
				count += MAX_RANK;
		}
		for (int i=0; i<StageAction.MAX_STAGE_INFO-1; i++) {
			if(count >= i*MAX_RANK + MAX_RANK){
				// 透明度更新
				InfoGear[i].GetComponent<UISprite>().alpha = 1.0f;
				// 回転処理
				InfoGear[i].transform.eulerAngles += new Vector3 (0, 0, GEAR_ROTATION[i]);
			}
			else{
				// 透明度更新
				InfoGear[i].GetComponent<UISprite>().alpha = 0.3f;
			}
		}

	}

}
