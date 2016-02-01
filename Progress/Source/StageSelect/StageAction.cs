using UnityEngine;
using System.Collections;

public class StageAction : MonoBehaviour {

	public static bool isStart;
	
	// 最大描画ステージ
	public const int MAX_STAGE_INFO = 8;

	// オブジェクト情報
	public GameObject[] StageIcon = new GameObject[MAX_STAGE_INFO];
	// 現在のページ表示用
	public UILabel page_number;
	// ステージアイコンの番号
	public UILabel[] stage_number = new UILabel[MAX_STAGE_INFO-1];
	
	public static bool isDrawStageNumber;

	// 外部参照可能な保存不要のアニメーションフラグ
	public static bool ExtraOpenFlag;

	void Start(){
		isDrawStageNumber = true;

		// チュートリアルの設定
		// 初めてステージセレクトに遷移した時
		if (GameMemory.GuideFlag[0] == 0) {
			GameGuide.NowTutorial = TUTORIAL.STAGE_SELECT;
			GameMemory.GuideFlag[0] = 1;
		}
		// ステージ1-1をクリアした後
		if (GameMemory.GuideFlag[2] == 0 && GameMemory.StageRank[0] == 3) {
			GameGuide.NowTutorial = TUTORIAL.STAGE_CLEAR1;
			GameMemory.GuideFlag[2] = 1;
		}
		// ステージ1-2をクリアした後
		if (GameMemory.GuideFlag[4] == 0 && GameMemory.StageRank[1] == 3) {
			GameGuide.NowTutorial = TUTORIAL.STAGE_CLEAR2;
			GameMemory.GuideFlag[4] = 1;
		}
		// ステージ1-3をクリアした後
		if (GameMemory.GuideFlag[7] == 0 && GameMemory.StageRank[2] == 3) {
			GameGuide.NowTutorial = TUTORIAL.STAGE_CLEAR3;
			GameMemory.GuideFlag[7] = 1;
		}
		// チュートリアルの表示
		if (GameGuide.NowTutorial != TUTORIAL.NONE)
			GameMain.GuideFlag = true;
		
		// アニメーションの確認
		if (GameMemory.ExtraAnimationFlag [GameMemory.StagePage] == 0) {
			int count = 0;
			for(int i=0;i<MAX_STAGE_INFO-1;i++){
				// 現在のページの総獲得ランクを計算
				count += GameMemory.StageRank[GameMemory.StagePage*MAX_STAGE_INFO+i];
			}	
			if(count >= StageInfo.MAX_RANK * (MAX_STAGE_INFO-1)){
				ExtraOpenFlag = true;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (isStart) {
			Start ();
			isStart = false;
		}
		if (CheckStage.isCheck)
			return;
		if (isDrawStageNumber) {
			DrawStageNumber ();
			isDrawStageNumber = false;
		}
	}
	
	// 各ステージの描画関数
	public void DrawStageNumber(){

		// 大ステージのテキストの更新
		page_number.text = "STAGE " + (GameMemory.StagePage + 1);

		int StageIndex = 0;
		for(int i=0;i<MAX_STAGE_INFO;i++){
			// ステージ用の添え字を確定
			StageIndex = GameMemory.StagePage * MAX_STAGE_INFO + i;

			if(i == MAX_STAGE_INFO-1){
				// エクストラステージの描画(ステージがロックされていれば表示しない)
				if(GameMemory.StageRock[StageIndex] == 1 && GameMemory.ExtraAnimationFlag [GameMemory.StagePage] == 1){
					StageIcon[i].SetActive(true);
					StageIcon[i].GetComponent<UISprite>().color = GameMemory.BackgroundColor[GameMemory.StagePage];
				}
				else
					StageIcon[i].SetActive(false);
			}
			else{
				// ステージアンロック時の描画
				if(GameMemory.StageRock[StageIndex] == 1){
					// ステージ背景の描画
					StageIcon[i].GetComponent<UISprite> ().spriteName = "StageBackground";
					// ステージ番号の更新
					stage_number[i].text = "" + (i+1);
				}else{
					// ステージロック時の描画
					StageIcon[i].GetComponent<UISprite> ().spriteName = "RockStage";
					// ステージ番号を描画しない
					stage_number[i].text = "";
				}
				// 色の更新
				StageIcon[i].GetComponent<UISprite>().color = GameMemory.BackgroundColor[GameMemory.StagePage];
			}
		}
	}
}