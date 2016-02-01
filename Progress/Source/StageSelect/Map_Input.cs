using UnityEngine;
using System.Collections;

public struct GIMMIC_STATUS{
	public int X,Y;
	public int startX,startY;
	public int trouble;
	public GameObject number;
}

public class Map_Input : MonoBehaviour {
	
	public static MAP_GIMMIC[,] mapdata;
	public static MAP_GIMMIC[,] start_mapdata;
	/// テキストデータのパス
	public static string filePath;
	/// 読み込んだテキストデータを格納するテキストアセット
	public static TextAsset stageTextAsset;
	/// ステージの文字列データ
	public static string stageData;
	/// 抽出した文字を格納する変数
	public static int pattern;
	/// ステージの文字列データの添え字
	public static int stageDataIndex;
	
	// 上2つのギミックの起動手数保管用変数
	public static GIMMIC_STATUS[] appear;
	public static GIMMIC_STATUS[] disappear;

	void Start () {

	}

	void Update () {
		
		
	}
	/// テキストデータの読み込み。
	/// テキストデータを読み込み、読み込んだテキストデータをステージデータに格納し、空白を削除する
	/// </remarks>
	public static void ReadTextData(){
		
		// TextAssetとして、Resourcesフォルダからテキストデータをロードする
		stageTextAsset = Resources.Load(filePath) as TextAsset;
		// 文字列を代入
		stageData = stageTextAsset.text;
		// 空白を置換で削除
		stageData = stageData.Replace("\n", "");
		// ','を置換で削除
		stageData = stageData.Replace(",", "");
	}
	
	// mapdata読み込み
	public static void Load(){
		
		stageDataIndex = 0;
		
		MapAction.Map_X = MapAction.Map_Y = 8;
		MapAction.magnet_s_count = 0;
		MapAction.magnet_s_count = 0;
		MapAction.appear_count = 0;
		MapAction.disappear_count = 0;

		mapdata = new MAP_GIMMIC[MapAction.Map_Y, MapAction.Map_X];
		start_mapdata = new MAP_GIMMIC[MapAction.Map_Y, MapAction.Map_X];

		// 各ギミックの個数確認
		for(int i=0;i<MapAction.Map_X*MapAction.Map_Y;i++){
			pattern = stageData[stageDataIndex];
			if(pattern == '6'){
				MapAction.appear_count++;
				stageDataIndex++;
			}
			if(pattern == '7'){
				MapAction.disappear_count++;
				stageDataIndex++;
			}
			// 次へ
			stageDataIndex++;
		}

		// ギミックの個数確定
		// 上2つのギミックの起動手数保管用変数
		appear = new GIMMIC_STATUS[MapAction.appear_count];
		disappear = new GIMMIC_STATUS[MapAction.disappear_count];

		stageDataIndex = 0;

		// 逆から格納するための添え字
		int Acount = 0;
		int Dcount = 0;
		// 配列に値を格納
		for (int Y=MapAction.Map_Y-1;Y>=0;Y--) 
		{
			for(int X=0;X<MapAction.Map_X;X++)
			{
				// 文字の抽出
				pattern = stageData[stageDataIndex];
				switch(pattern)
				{
				case '0':
					mapdata[Y,X] = start_mapdata[Y,X] = MAP_GIMMIC.NONE;
					break;
				case '1':
					mapdata[Y,X] = start_mapdata[Y,X] = MAP_GIMMIC.BLOCK;
					break;
				case '2':
					// プレイヤーの座標を保存して空白にしておく
					Player.X = Player.StartX = X;
					Player.Y = Player.StartY = Y;
					mapdata[Y,X] = start_mapdata[Y,X] = MAP_GIMMIC.NONE;
					break;
				case '3':
					mapdata[Y,X] = start_mapdata[Y,X] = MAP_GIMMIC.GOAL;
					break;
				case '4':
					mapdata[Y,X] = start_mapdata[Y,X] = MAP_GIMMIC.MAGNET_S;
					MapAction.magnet_s_count++;
					break;
				case '5':
					mapdata[Y,X] = start_mapdata[Y,X] = MAP_GIMMIC.MAGNET_N;
					MapAction.magnet_n_count++;
					break;
				case '6':
					mapdata[Y,X] = start_mapdata[Y,X] = MAP_GIMMIC.APPEAR;
					stageDataIndex++;
					// 手数と座標の保管
					appear[Acount].trouble = stageData[stageDataIndex]-48;
					appear[Acount].X = appear[Acount].startX = X;
					appear[Acount].Y = appear[Acount].startY = Y;
					Acount++;
					break;
				case '7':
					mapdata[Y,X] = start_mapdata[Y,X] = MAP_GIMMIC.DISAPPEAR;
					stageDataIndex++;
					// 手数と座標の保管
					disappear[Dcount].trouble = stageData[stageDataIndex]-48;
					disappear[Dcount].X = disappear[Dcount].startX = X;
					disappear[Dcount].Y = disappear[Dcount].startY = Y;
					Dcount++;
					break;
				}
				
				// 次へ
				stageDataIndex++;
			}
		}
		// ギミックの数分座標配列の確保
		MapAction.magnet_s_position = new Vector2[MapAction.magnet_s_count];
		MapAction.magnet_n_position = new Vector2[MapAction.magnet_n_count];
	}
	
	
}
