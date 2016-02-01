using UnityEngine;
using System.Collections;

public class GameMemory : MonoBehaviour {
	
	// 総獲得ランク
	public static int TotalRank;
	// 現在の選択ページ
	public static int StagePage;
	// 現在の選択ステージ
	public static int StageGear;
	// 裏ステージの解放アニメーション用のフラグ
	public static int[] ExtraAnimationFlag = new int[RollPage.PAGE_MAX];
	// 各ステージごとの獲得ランク
	public static int[] StageRank = new int[StageAction.MAX_STAGE_INFO*RollPage.PAGE_MAX];
	// ステージロック情報
	public static int[] StageRock = new int[StageAction.MAX_STAGE_INFO*RollPage.PAGE_MAX];
	// チュートリアルが行われたか
	private const int MAX_GUIDE = 10;
	public static int[] GuideFlag = new int[MAX_GUIDE];
	/*	0:ステージセレクト(ステージ選択方法)
	 * 	1:ステージ1-1(プレイヤーの動かし方)
	 * 	2:ステージセレクト(ステージ解放条件)
	 * 	3:ステージ1-2(ステージの切り方)
	 * 	4:ステージセレクト(クリア報酬について)
	 *  5:ステージ1-3(性質の変わるブロック)
	 *  6:ステージ1-3(性質の変わるブロック2)
	 *  7:ステージセレクト(チュートリアルの終了の通知)
	 */

	// 色情報
	public static Color[] BackgroundColor = new Color[RollPage.PAGE_MAX]{new Color(1,1,1),new Color(1,1,0),new Color(0.5f,1,0.5f),new Color(0,1,1),new Color(0.5f,0.75f,1),new Color(0.5f,0.5f,1)};

	void Start () {
	}
	
	void Update () {
	}

	// データのロード
	public static void Load()
	{
		// 総獲得ランク
		TotalRank = 0;
		// 現在のページ
		StagePage = 0;
		// 現在のステージ
		StageGear = 0;
		// ステージランクとロック情報
		for(int i=0; i<StageRank.Length; i++)
		{
			StageRank[i] = 0;
			StageRock[i] = 1;
		}
		// チュートリアル表示情報
		for(int i=0; i<GuideFlag.Length; i++)
		{
			GuideFlag[i] = 1;
		}
		// エクストラステージの解放アニメーションフラグ
		for (int i=0; i<ExtraAnimationFlag.Length; i++) {
			ExtraAnimationFlag[i] = 1;
		}
	}

	// データのセーブ
	public static void Save()
	{

	}

	// データを初期化
	public static void Init(){
		// 総獲得ランク
		TotalRank = 0;
		// 現在のページ
		StagePage = 0;
		// 現在のステージ
		StageGear = 0;
		// ステージランクとロック情報
		for(int i=0; i<StageRank.Length; i++)
		{
			StageRank[i] = 0;
			StageRock[i] = 0;
		}
		StageRock[0] = 1;
		// チュートリアル表示情報
		for(int i=0; i<GuideFlag.Length; i++)
		{
			GuideFlag[i] = 0;
		}
		// エクストラステージの解放アニメーションフラグ
		for (int i=0; i<ExtraAnimationFlag.Length; i++) {
			ExtraAnimationFlag[i] = 0;
		}
	}
}
