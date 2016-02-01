using UnityEngine;
using System.Collections;

public class result : MonoBehaviour {

	public static bool isStart;
	public GameObject[] RankObj = new GameObject[2];

	// 親指定用
	public GameObject Rank2,Rank3;
	
	private int rank_gear;

	// Use this for initialization
	void Start () {

		// ランクを確定
		rank_gear = StageInfo.MAX_RANK + stage.StageTrouble[GameMemory.StagePage * StageAction.MAX_STAGE_INFO + GameMemory.StageGear] - MapAction.Trouble;

		// 最低獲得ランクを保障
		if (rank_gear <= 0)
			rank_gear = 1;
		// 最大獲得ランクを3に調整
		if (rank_gear >= StageInfo.MAX_RANK)
			rank_gear = StageInfo.MAX_RANK;

		// ランクと総獲得ランクを更新
		if (GameMemory.StageRank [GameMemory.StagePage * StageAction.MAX_STAGE_INFO + GameMemory.StageGear] < rank_gear) {
			GameMemory.TotalRank -= GameMemory.StageRank [GameMemory.StagePage * StageAction.MAX_STAGE_INFO + GameMemory.StageGear];
			GameMemory.StageRank [GameMemory.StagePage * StageAction.MAX_STAGE_INFO + GameMemory.StageGear] = rank_gear;
			GameMemory.TotalRank += GameMemory.StageRank [GameMemory.StagePage * StageAction.MAX_STAGE_INFO + GameMemory.StageGear];
		}
		// 次のステージを解放
		if (GameMemory.StageGear < 2) {
			// 1~3は順番に解放
			GameMemory.StageRock [GameMemory.StagePage * StageAction.MAX_STAGE_INFO + GameMemory.StageGear + 1] = 1;
		}
		else {
			// 3クリアで7まで + 各ページの1を解放
			for(int i=3;i<StageAction.MAX_STAGE_INFO-1;i++){
				GameMemory.StageRock [GameMemory.StagePage * StageAction.MAX_STAGE_INFO + i] = 1;
			}
			for(int i=0;i<RollPage.PAGE_MAX;i++){
				GameMemory.StageRock [i * StageAction.MAX_STAGE_INFO] = 1;
			}
		}
		if (rank_gear >= 2)
			Rank2.GetComponent<UISprite> ().spriteName = "RankGear";
		else {
			Rank2.GetComponent<UISprite> ().spriteName = "NoRankGear";
			PlaySE.isPlayGear1 = true;
		}
		if (rank_gear >= 3) {
			Rank3.GetComponent<UISprite> ().spriteName = "RankGear";
			PlaySE.isPlayGear3 = true;
		}
		else {
			Rank3.GetComponent<UISprite> ().spriteName = "NoRankGear";
			PlaySE.isPlayGear2 = true;
		}
	}
	
	// Update is called once per frame
	void Update () {

		if (isStart) {
			Start ();
			isStart = false;
		}

		PlayBGM.isPlayGamePlay = true;
	}
}
