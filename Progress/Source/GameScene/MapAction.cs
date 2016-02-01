using UnityEngine;
using System.Collections;

enum CUT
{
	NONE,
	CUT_DOWN,
	CUT_UP,
	CUT_RIGHT,
	CUT_LEFT
}

public enum MAP_GIMMIC
{
	NONE,
	PLAYER,
	BLOCK,
	GOAL,
	MAGNET_S,
	MAGNET_N,
	APPEAR,
	DISAPPEAR
}

enum GIMMIC_CHECK
{
	SandS,
	SandN
}

public class MapAction : MonoBehaviour {

	public static bool isStart;
	
	// オブジェクト情報
	public GameObject player;
	public GameObject GimmicParticle;
	private GameObject PlayMainObj;

	// クローン用
	private GameObject[] N_block = new GameObject[64];
	private GameObject N_player;

	// テキストプレハブ
	public GameObject LabelPfb;
	
	// キー情報
	private CUT cut_key;
	private CUT animation_key;
	private CUT before_key;
	
	// マップ情報
	private MAP_GIMMIC[,] next_mapdata;
	public static int Map_X,Map_Y;
	private int next_Map_X,next_Map_Y;
	
	// マップ描画サイズ
	private float BUFF_L = (Screen.height-50)/8;
	private float BUFF_M = (Screen.height-20)/16;
	private float BUFF_S = (Screen.height-20)/32;
	private float buff;
	private float next_buff;
	private float buff_difference;	// アニメーション前後のサイズの差

	// 描画する際の中心座標
	public static float CenterX,CenterY;	// カメラオフじの初期化のため参照可能
	private float CenterDiferenceX,CenterDiferenceY;
	// カメラ用座標
	private Vector2 CameraMovePos;

	// アニメーションする歯車
	public GameObject[] Gear = new GameObject[4];
	// 各ギミックの数
	public static int magnet_s_count;
	public static int magnet_n_count;
	public static int appear_count;
	public static int disappear_count;
	
	// ギミックの座標保存用
	public static Vector2[] magnet_s_position;
	public static Vector2[] magnet_n_position;
	
	// アニメーション用メンバ変数
	public static bool animation_flag;	// プレイヤーから参照必要
	private int  animation_count;
	
	// 描画するかどうかのフラグ(外部から参照可能)
	public static bool draw_flag;
	public static bool player_draw_flag;
	private bool move_flag;
	
	// 切り取った回数
	public static int Trouble;
	public UILabel trouble_number;

	// 二回呼び出さないために関数名変更
	void MapStart (){

		Destroy (PlayMainObj);
		// 親インスタンス生成
		PlayMainObj = new GameObject();
		PlayMainObj.transform.parent = transform;
		// サイズ調整
		PlayMainObj.transform.localScale = new Vector3 (1,1,1);

		// 各種初期化
		cut_key = CUT.NONE;
		buff = BUFF_L;
		move_flag = false;
		Trouble = 0;
		CenterX = CenterY = 0;
		Player.move_flag = false;
		Player.clear_flag = false;

		// マップ生成
		for (int Y=0; Y<Map_Y; Y++) {
			for (int X=0; X<Map_X; X++) {
				N_block[Y*Map_Y+X] = Instantiate (player, transform.position, Quaternion.identity)as GameObject;
				N_block[Y*Map_Y+X].transform.parent = PlayMainObj.transform;
			}
		}
		// ギミック番号生成
		for (int i=0; i<appear_count; i++) {
			Map_Input.appear[i].number = Instantiate (player, transform.position, Quaternion.identity)as GameObject;
			Map_Input.appear[i].number.transform.parent = PlayMainObj.transform;
		}
		for (int i=0; i<disappear_count; i++) {
			Map_Input.disappear[i].number = Instantiate (player, transform.position, Quaternion.identity)as GameObject;
			Map_Input.disappear[i].number.transform.parent = PlayMainObj.transform;
		}
		// プレイヤー生成
		N_player = Instantiate (player, transform.position, Quaternion.identity)as GameObject;
		N_player.transform.parent = PlayMainObj.transform;

		// 初回描画
		AppearGimmicAction ();
		Draw ();
		PlayerDraw ();

		// チュートリアルの確認
		if (GameMemory.StagePage == 0 && GameMemory.StageGear == 0 && GameMemory.GuideFlag [1] == 0) {
			GameGuide.NowTutorial = TUTORIAL.STAGE1_1;
			GameMemory.GuideFlag [1] = 1;
		}
		if (GameMemory.StagePage == 0 && GameMemory.StageGear == 1 && GameMemory.GuideFlag [3] == 0) {
			GameGuide.NowTutorial = TUTORIAL.STAGE1_2;
			GameMemory.GuideFlag [3] = 1;
		}
		if (GameMemory.StagePage == 0 && GameMemory.StageGear == 2 && GameMemory.GuideFlag [5] == 0) {
			GameGuide.NowTutorial = TUTORIAL.STAGE1_3_1;
			GameMemory.GuideFlag [5] = 1;
		}

		// チュートリアルの表示
		if (GameGuide.NowTutorial != TUTORIAL.NONE)
			GameMain.GuideFlag = true;
	}
	
	// Update is called once per frame
	void Update () {

		if (isStart) {
			MapStart ();
			isStart = false;
		}

		// リスタート時の処理
		if (Setting.isRestart) {
			buff = BUFF_L;
			Trouble = 0;
			AppearGimmicAction ();
			Draw ();
			CameraAction.isStart = true;
		}

		// 更新制御
		if (Setting.isSetting || GameGuide.isGuide || Fade.NowFade != FADE.FADE_NONE)
			return;
		if (Player.clear_flag && Fade.NowFade == FADE.FADE_OUT)
			return;

		// プレイヤーの動きを更新
		move_flag = move_flag | Player.move_flag;

		// プレイヤーの移動などで画面が更新されたら描画しなおす
		if (player_draw_flag)
			PlayerDraw ();
		if (draw_flag)
			Draw ();

		// クリア判定
		if(Player.clear_flag && Fade.NowFade == FADE.FADE_NONE)
			Fade.FadeRun(GAME_SCENE.RESULT);

		// カメラ操作
		if (CameraAction.CameraOn) {
			GameCamera ();
			return;
		}

		// 入力取得
		InputKey ();

		N_GimmickAction ();

		if(animation_flag)
		{
			// プレイヤーの位置調整を元に戻す
			Player.MoveDistance = 0;

			if(AnimationDraw(animation_key))
			{	
				Trouble++;//切った回数	
				animation_flag = false;

				Cut();
				Create(next_Map_X, next_Map_Y, true);
				Copy();
				AppearGimmicAction ();
				// 大きさを調整
				if (Map_X == 8 || Map_Y == 8)
					buff = BUFF_L;
				if (Map_X == 16 || Map_Y == 16)
					buff = BUFF_M;
				if (Map_X == 32 || Map_Y == 32)
					buff = BUFF_S;
				Draw();

				// ステージ1-3で上に切った時に追加でチュートリアル
				if(GameMemory.StagePage == 0 && GameMemory.StageGear == 2){
					if (GameMemory.GuideFlag[6] == 0 && Trouble == 1) {
						GameGuide.NowTutorial = TUTORIAL.STAGE1_3_2;
						GameMemory.GuideFlag[6] = 1;
						GameMain.GuideFlag = true;
					}
				}
			}
		}
		else
			cut_key = CUT.NONE;

		before_key = animation_key;
		move_flag = false;
	}

	// 入力取得関数
	void InputKey(){

		cut_key = CUT.NONE;

		const int MapLimit = 2;			// X軸Y軸の最小値

		// スマホ用の入力取得
		switch (TouchAction.swipe) {
		case SWIPE.NONE:
			cut_key = CUT.NONE;
			break;
		case SWIPE.SWIPE_DOWN:
			if (Map_X > MapLimit)
				cut_key = CUT.CUT_DOWN;
			break;
		case SWIPE.SWIPE_UP:
			if (Map_X > MapLimit)
				cut_key = CUT.CUT_UP;
			break;
		case SWIPE.SWIPE_RIGHT:
			if (Map_Y > MapLimit)
				cut_key = CUT.CUT_RIGHT;
			break;
		case SWIPE.SWIPE_LEFT:
			if (Map_Y > MapLimit)
				cut_key = CUT.CUT_LEFT;
			break;
		}

		// PC用の入力取得
		if (Input.GetKey (KeyCode.W)) {
			if (Map_X > MapLimit)
				cut_key = CUT.CUT_UP;
		}
		if (Input.GetKey (KeyCode.A)) {
			if (Map_Y > MapLimit)
				cut_key = CUT.CUT_LEFT;
		}
		if (Input.GetKey (KeyCode.S)) {
			if (Map_X > MapLimit)
				cut_key = CUT.CUT_DOWN;
		}
		if (Input.GetKey (KeyCode.D)) {
			if (Map_Y > MapLimit)
				cut_key = CUT.CUT_RIGHT;
		}
		// 反発のギミックが稼働していたら元に戻す
		if (magnet_s_count > 0 && !animation_flag) {
			if (S_GimmickCheck ()) {
				// 隣り合っていた場合の元の状態に切りなおす処理
				if (animation_key == CUT.CUT_DOWN) {
					cut_key = CUT.CUT_UP;
				}
				if (animation_key == CUT.CUT_UP) {
					cut_key = CUT.CUT_DOWN;
				}
				if (animation_key == CUT.CUT_LEFT) {
					cut_key = CUT.CUT_RIGHT;
				}
				if (animation_key == CUT.CUT_RIGHT) {
					cut_key = CUT.CUT_LEFT;
				}
			}
		}

		// チュートリアル中の操作制限
		// ステージ1-1をクリアしていない時は切れない
		if (GameMemory.GuideFlag [2] == 0)
			cut_key = CUT.NONE;
		// ステージ1-2は左右にしか切れない
		if (GameMemory.StagePage == 0 && GameMemory.StageGear == 1 && GameMemory.GuideFlag [4] == 0){
			if (cut_key == CUT.CUT_DOWN || cut_key == CUT.CUT_UP){
				cut_key = CUT.NONE;
			}
		}
		// ステージ1-3は上にしか切れない
		if (GameMemory.StagePage == 0 && GameMemory.StageGear == 2 && GameMemory.GuideFlag [7] == 0){
			if (cut_key == CUT.CUT_DOWN || cut_key == CUT.CUT_LEFT || cut_key == CUT.CUT_RIGHT){
				cut_key = CUT.NONE;
			}
			// ランクを獲得してもらうために1回しか切れないようにする
			if(Trouble == 1)
				cut_key = CUT.NONE;
		}

		// 入力取得
		if(cut_key != CUT.NONE && !animation_flag && !move_flag)
		{
			if(cut_key == CUT.CUT_DOWN || cut_key == CUT.CUT_UP)
			{
				// 次のマップの行列を計算
				next_Map_Y = Map_X / 2;
				next_Map_X = Map_Y * 2;
			}
			else
			{
				if(cut_key == CUT.CUT_LEFT || cut_key == CUT.CUT_RIGHT)
				{
					// 次のマップの行列を計算
					next_Map_Y = Map_X * 2;
					next_Map_X = Map_Y / 2;
				}
			}
			PlaySE.isPlayAnimation = true;
			animation_flag = true;
			animation_key = cut_key;
			
			// 切り取った後のブロックのサイズを次のマップのX軸Y軸の数で判定
			if (next_Map_X == 8 || next_Map_Y == 8)
				next_buff = BUFF_L;
			if (next_Map_X == 16 || next_Map_Y == 16)
				next_buff = BUFF_M;
			if (next_Map_X == 32 || next_Map_Y == 32)
				next_buff = BUFF_S;
			
			// サイズの差を絶対値で保存
			buff_difference = next_buff - buff;
		}
	}
	
	// mapdata描画
	void Draw(){
		
		int count = 0;
		int Scount = 0;
		int Ncount = 0;

		// 切り取った回数を更新
		trouble_number.text = "" + Trouble;

		//配列の値を描画
		for (int Y=Map_Y-1;Y>=0;Y--) 
		{
			for(int X=0;X<Map_X;X++)
			{	
				switch(Map_Input.mapdata[Y,X])
				{
				case MAP_GIMMIC.NONE:
					N_block[count].GetComponent<UISprite> ().spriteName = "Space";
					break;
				case MAP_GIMMIC.BLOCK:
					N_block[count].GetComponent<UISprite> ().spriteName = "Block";
					break;
				case MAP_GIMMIC.GOAL:
					N_block[count].GetComponent<UISprite> ().spriteName = "Goal";
					break;
				case MAP_GIMMIC.MAGNET_S:
					N_block[count].GetComponent<UISprite> ().spriteName = "Smagnet";
					// 座標の保存
					magnet_s_position[Scount].x = X;
					magnet_s_position[Scount].y = Y;
					Scount++;
					break;
				case MAP_GIMMIC.MAGNET_N:
					N_block[count].GetComponent<UISprite> ().spriteName = "Nmagnet";
					// 座標の保存
					magnet_n_position[Ncount].x = X;
					magnet_n_position[Ncount].y = Y;
					Ncount++;
					break;
				case MAP_GIMMIC.APPEAR:
					N_block[count].GetComponent<UISprite> ().spriteName = "appear";
					break;
				case MAP_GIMMIC.DISAPPEAR:
					N_block[count].GetComponent<UISprite> ().spriteName = "disapper";
					break;
				}
				// 大きさと位置調整
				N_block[count].transform.localScale = new Vector3(buff,buff,1);
				N_block[count].transform.localPosition = new Vector3((X-Map_X/2)*buff+buff/2-CenterX, (Y-Map_Y/2)*buff+buff/2-CenterY, 0);
				count++;
			}
		}
		for (int i=0; i<appear_count; i++) {
			Map_Input.appear[i].number.transform.localScale = new Vector3(buff,buff,1);
			Map_Input.appear[i].number.transform.localPosition = new Vector3((Map_Input.appear[i].X-Map_X/2)*buff+buff/2-CenterX, (Map_Input.appear[i].Y-Map_Y/2)*buff+buff/2-CenterY, 0);
			Map_Input.appear[i].number.transform.eulerAngles = new Vector3(0,0,0);
		}
		for (int i=0; i<disappear_count; i++) {
			Map_Input.disappear[i].number.transform.localScale = new Vector3(buff,buff,1);
			Map_Input.disappear[i].number.transform.localPosition = new Vector3((Map_Input.disappear[i].X-Map_X/2)*buff+buff/2-CenterX, (Map_Input.disappear[i].Y-Map_Y/2)*buff+buff/2-CenterY, 0);
			Map_Input.disappear[i].number.transform.eulerAngles = new Vector3(0,0,0);
		}

		draw_flag = false;

		// マップ更新後にプレイヤーも描画
		player_draw_flag = true;
	}

	void PlayerDraw(){
		// 隣のブロックとのＸ軸の差を計算											
		float distance = Mathf.Abs(((0 - Map_X / 2) * buff + buff / 2 - CenterX) - ((1 - Map_X / 2) * buff + buff / 2 - CenterX));

		N_player.transform.localScale = new Vector3(buff,buff,1);
		N_player.transform.localPosition = new Vector3((Player.X-Map_X/2)*buff+buff/2-CenterX + distance*Player.MoveDistance, (Player.Y-Map_Y/2)*buff+buff/2-CenterY, -1);
		N_player.transform.eulerAngles = new Vector3 (0, 0, Player.angle);

		player_draw_flag = false;
	}
	
	// マップ切り取り処理
	void Cut(){
		int next_countx = 0;
		int next_county = 0;

		// 各ギミックを一度だけ更新するためのフラグ
		bool player_update = false;
		bool[] appear_update = new bool[appear_count];
		bool[] disappear_update = new bool[disappear_count];
		
		switch (animation_key)
		{
		//------------------
		// 下に向かって切った時
		//------------------
		case CUT.CUT_DOWN:
			// 次のマップの行列を計算
			next_Map_Y = Map_X / 2;
			next_Map_X = Map_Y * 2;
			// 新マップの生成
			Create(next_Map_X,next_Map_Y,false);
			
			for (int i = 0; i<Map_X/2; i++)
			{
				// 切った後の列読み込み下から上に読み込み
				for (int y = Map_Y - 1; y >= 0; y--)
				{
					next_mapdata[i,next_countx] = Map_Input.mapdata[y,i];
					// プレイヤーの位置更新
					if(i == Player.X && y == Player.Y && !player_update){
						Player.X = next_countx;
						Player.Y = i;
						player_update = true;
					}
					// ギミック確認
					if(Map_Input.mapdata[y,i] == MAP_GIMMIC.APPEAR){
						for(int j=0;j<appear_count;j++){
							if(!appear_update[j]){
								if((i == Map_Input.appear[j].X) && (y == Map_Input.appear[j].Y)){
									Map_Input.appear[j].X = next_countx;
									Map_Input.appear[j].Y = i;
									appear_update[j] = true;
								}
							}
						}
					}
					if(Map_Input.mapdata[y,i] == MAP_GIMMIC.DISAPPEAR){
						for(int j=0;j<disappear_count;j++){
							if(!disappear_update[j]){
								if((i == Map_Input.disappear[j].X) && (y == Map_Input.disappear[j].Y)){
									Map_Input.disappear[j].X = next_countx;
									Map_Input.disappear[j].Y = i;
									disappear_update[j] = true;
								}

							}
						}
					}
					next_countx++;
				}
				// 切った後の列読み込み上から下に読み込み
				for (int y = 0; y<Map_Y; y++)
				{
					next_mapdata[i,next_countx] = Map_Input.mapdata[y,Map_X-1-i];
					// プレイヤーの位置更新
					if(Map_X-1-i == Player.X && y == Player.Y && !player_update){
						Player.X = next_countx;
						Player.Y = i;
						player_update = true;
					}
					// ギミック確認
					if(Map_Input.mapdata[y,Map_X-1-i] == MAP_GIMMIC.APPEAR){
						for(int j=0;j<appear_count;j++){
							if(!appear_update[j]){
								if((Map_X-1-i == Map_Input.appear[j].X) && (y == Map_Input.appear[j].Y)){
									Map_Input.appear[j].X = next_countx;
									Map_Input.appear[j].Y = i;
									appear_update[j] = true;
								}
							}
						}
					}
					if(Map_Input.mapdata[y,Map_X-1-i] == MAP_GIMMIC.DISAPPEAR){
						for(int j=0;j<disappear_count;j++){
							if(!disappear_update[j]){
								if((Map_X-1-i == Map_Input.disappear[j].X) && (y == Map_Input.disappear[j].Y)){
									Map_Input.disappear[j].X = next_countx;
									Map_Input.disappear[j].Y = i;
									disappear_update[j] = true;
								}
							}
						}
					}
					next_countx++;
				}
				next_countx = 0;
			}
			break;

		//------------------
		// 上に向かって切った時
		//------------------
		case CUT.CUT_UP:
			// 次のマップの行列を計算
			next_Map_Y = Map_X / 2;
			next_Map_X = Map_Y * 2;
			// 新マップの生成
			Create(next_Map_X,next_Map_Y,false);
			
			for (int i = 0; i<Map_X/2 ; i++)
			{
				// 切った後の列読み込み上から下に読み込み
				for (int y = 0; y<Map_Y; y++)
				{
					next_mapdata[i,next_countx] = Map_Input.mapdata[y,(Map_X / 2) - 1 - i];
					// プレイヤーの位置更新
					if((Map_X / 2) - 1 - i == Player.X && y == Player.Y && !player_update){
						Player.X = next_countx;
						Player.Y = i;
						player_update = true;
					}
					// ギミック確認
					switch(Map_Input.mapdata[y,(Map_X / 2) - 1 - i])
					{
					case MAP_GIMMIC.APPEAR:
						for(int j=0;j<appear_count;j++){
							if(!appear_update[j]){
								if(((Map_X / 2) - 1 - i == Map_Input.appear[j].X) && (y == Map_Input.appear[j].Y)){
									Map_Input.appear[j].X = next_countx;
									Map_Input.appear[j].Y = i;
									appear_update[j] = true;
								}
							}
						}
						break;
					case MAP_GIMMIC.DISAPPEAR:
						for(int j=0;j<disappear_count;j++){
							if(!disappear_update[j]){
								if(((Map_X / 2) - 1 - i == Map_Input.disappear[j].X) && (y == Map_Input.disappear[j].Y)){
									Map_Input.disappear[j].X = next_countx;
									Map_Input.disappear[j].Y = i;
									disappear_update[j] = true;
								}
							}
						}
						break;
					}
					next_countx++;
				}
				// 切った後の列読み込み下から上に読み込み
				for (int y = Map_Y - 1; y >= 0; y--)
				{
					next_mapdata[i,next_countx] = Map_Input.mapdata[y,(Map_X / 2) + i];
					// プレイヤーの位置更新
					if((Map_X / 2) + i == Player.X && y == Player.Y && !player_update){
						Player.X = next_countx;
						Player.Y = i;
						player_update = true;
					}
					// ギミック確認
					switch(Map_Input.mapdata[y,(Map_X / 2) + i])
					{
					case MAP_GIMMIC.APPEAR:
						for(int j=0;j<appear_count;j++){
							if(!appear_update[j]){
								if(((Map_X / 2) + i == Map_Input.appear[j].X) && (y == Map_Input.appear[j].Y)){
									Map_Input.appear[j].X = next_countx;
									Map_Input.appear[j].Y = i;
									appear_update[j] = true;
								}
							}
						}
						break;

					case MAP_GIMMIC.DISAPPEAR:
						for(int j=0;j<disappear_count;j++){
							if(!disappear_update[j]){
								if(((Map_X / 2) + i == Map_Input.disappear[j].X) && (y == Map_Input.disappear[j].Y)){
									Map_Input.disappear[j].X = next_countx;
									Map_Input.disappear[j].Y = i;
									disappear_update[j] = true;
								}
							}
						}
						break;
					}
					next_countx++;
				}
				next_countx = 0;
			}	
			break;

		//------------------
		// 右に向かって切った時
		//------------------
		case CUT.CUT_RIGHT:
			// 次のマップの行列を計算
			next_Map_Y = Map_X * 2;
			next_Map_X = Map_Y / 2;
			// 新マップの生成
			Create(next_Map_X,next_Map_Y,false);
			
			for (int x = 0; x<Map_X; x++)
			{
				// 切った後の列読み込み(中央上から右に読み込み)
				for (int y = (Map_Y / 2)-1; y >= 0; y--)
				{
					next_mapdata[next_county,next_countx] = Map_Input.mapdata[y,x];
					// プレイヤーの位置更新
					if(x == Player.X && y == Player.Y && !player_update){
						Player.X = next_countx;
						Player.Y = next_county;
						player_update = true;
					}
					// ギミック確認
					switch(Map_Input.mapdata[y,x])
					{
					case MAP_GIMMIC.APPEAR:
						for(int j=0;j<appear_count;j++){
							if(!appear_update[j]){
								if((x == Map_Input.appear[j].X) && (y == Map_Input.appear[j].Y)){
									Map_Input.appear[j].X = next_countx;
									Map_Input.appear[j].Y = next_county;
									appear_update[j] = true;
								}
							}
						}
						break;

					case MAP_GIMMIC.DISAPPEAR:
						for(int j=0;j<disappear_count;j++){
							if(!disappear_update[j]){
								if((x == Map_Input.disappear[j].X) && (y == Map_Input.disappear[j].Y)){
									Map_Input.disappear[j].X = next_countx;
									Map_Input.disappear[j].Y = next_county;
									disappear_update[j] = true;
								}
							}
						}
						break;
					}
					next_countx++;
				}
				next_county++;
				next_countx = 0;
			}
			for (int x = Map_X - 1; x >= 0; x--)
			{
				// 切った後の列読み込み(中央下から左に読み込み)
				for (int y = (Map_Y / 2); y<Map_Y; y++)
				{
					next_mapdata[next_county,next_countx] = Map_Input.mapdata[y,x];
					// プレイヤーの位置更新
					if(x == Player.X && y == Player.Y && !player_update){
						Player.X = next_countx;
						Player.Y = next_county;
						player_update = true;
					}
					// ギミック確認
					switch(Map_Input.mapdata[y,x])
					{
					case MAP_GIMMIC.APPEAR:
						for(int j=0;j<appear_count;j++){
							if(!appear_update[j]){
								if((x == Map_Input.appear[j].X) && (y == Map_Input.appear[j].Y)){
									Map_Input.appear[j].X = next_countx;
									Map_Input.appear[j].Y = next_county;
									appear_update[j] = true;
								}
							}
						}
						break;
						
					case MAP_GIMMIC.DISAPPEAR:
						for(int j=0;j<disappear_count;j++){
							if(!disappear_update[j]){
								if((x == Map_Input.disappear[j].X) && (y == Map_Input.disappear[j].Y)){
									Map_Input.disappear[j].X = next_countx;
									Map_Input.disappear[j].Y = next_county;
									disappear_update[j] = true;
								}
							}
						}
						break;
					}
					next_countx++;
				}
				next_county++;
				next_countx = 0;
			}
			break;	

		//------------------
		// 左に向かって切った時
		//------------------
		case CUT.CUT_LEFT:
			// 次のマップの行列を計算
			next_Map_Y = Map_X * 2;
			next_Map_X = Map_Y / 2;
			// 新マップの生成
			Create(next_Map_X,next_Map_Y,false);
			
			for (int x = Map_X - 1; x >= 0; x--)
			{
				// 切った後の列読み込み(中央上から左に読み込み)
				for (int y = 0; y<(Map_Y / 2); y++)
				{
					next_mapdata[next_county,next_countx] = Map_Input.mapdata[y,x];
					// プレイヤーの位置更新
					if(x == Player.X && y == Player.Y && !player_update){
						Player.X = next_countx;
						Player.Y = next_county;
						player_update = true;
					}
					// ギミック確認
					switch(Map_Input.mapdata[y,x])
					{
					case MAP_GIMMIC.APPEAR:
						for(int j=0;j<appear_count;j++){
							if(!appear_update[j]){
								if((x == Map_Input.appear[j].X) && (y == Map_Input.appear[j].Y)){
									Map_Input.appear[j].X = next_countx;
									Map_Input.appear[j].Y = next_county;
									appear_update[j] = true;
								}
							}
						}
						break;
						
					case MAP_GIMMIC.DISAPPEAR:
						for(int j=0;j<disappear_count;j++){
							if(!disappear_update[j]){
								if((x == Map_Input.disappear[j].X) && (y == Map_Input.disappear[j].Y)){
									Map_Input.disappear[j].X = next_countx;
									Map_Input.disappear[j].Y = next_county;
									disappear_update[j] = true;
								}
							}
						}
						break;
					}
					next_countx++;
				}
				next_county++;
				next_countx = 0;
			}
			for (int x = 0; x<Map_X; x++)
			{
				// 切った後の列読み込み(中央下から右に読み込み)
				for (int y = Map_Y-1; y>=(Map_Y / 2); y--)
				{
					next_mapdata[next_county,next_countx] = Map_Input.mapdata[y,x];
					// プレイヤーの位置更新
					if(x == Player.X && y == Player.Y && !player_update){
						Player.X = next_countx;
						Player.Y = next_county;
						player_update = true;
					}
					// ギミック確認
					switch(Map_Input.mapdata[y,x])
					{
					case MAP_GIMMIC.APPEAR:
						for(int j=0;j<appear_count;j++){
							if(!appear_update[j]){
								if((x == Map_Input.appear[j].X) && (y == Map_Input.appear[j].Y)){
									Map_Input.appear[j].X = next_countx;
									Map_Input.appear[j].Y = next_county;
									appear_update[j] = true;
								}
							}
						}
						break;
						
					case MAP_GIMMIC.DISAPPEAR:
						for(int j=0;j<disappear_count;j++){
							if(!disappear_update[j]){
								if((x == Map_Input.disappear[j].X) && (y == Map_Input.disappear[j].Y)){
									Map_Input.disappear[j].X = next_countx;
									Map_Input.disappear[j].Y = next_county;
									disappear_update[j] = true;
								}
							}
						}
						break;
					}
					next_countx++;
				}
				next_county++;
				next_countx = 0;
			}
			break;
		}
	}
	
	// mapdata(next_mapdata)の生成
	void Create(int x, int y, bool map_flag){
		// TRUEの場合mapdataを作成
		if (map_flag) {
			Map_Y = y;
			Map_X = x;
			Map_Input.mapdata = new MAP_GIMMIC [Map_Y,Map_X];
		} else {
			next_Map_Y = y;
			next_Map_X = x;
			next_mapdata = new MAP_GIMMIC [next_Map_Y,next_Map_X];
		}
	}
	
	// mapdataにnext_mapdataをコピー
	void Copy()
	{
		for (int y = 0; y<Map_Y; y++)
		{
			for (int x = 0; x<Map_X; x++)
			{
				Map_Input.mapdata[y,x] = next_mapdata[y,x];
			}
		}
	}
	
	
	/*------------------------------------*/
	// アニメーション
	/*------------------------------------*/
	
	// 切り取った後のアニメーション描画関数
	bool AnimationDraw(CUT key)
	{
		// 座標
		float posX = 0;
		float posY = 0;
		// アニメーション中の位置調節用変数
		float adjust_x = 0;
		float adjust_y = 0;
		// マップを切る時の支点
		float pivot_x = 0;
		float pivot_y = 0;
		// 角度
		float rot = 0;
		// アニメーションが終わる角度(演算に使う為 float )
		const float MAX_ROT = 90;
		// 円周率
		const double PI = 3.141592653589793;
		// アニメーションの速度(大きいほど遅い)
		const float ANIMATION_SPEED = 3;
		
		animation_count++;

		// 歯車のアニメーション
		Gear[0].transform.eulerAngles += new Vector3 (0, 0, 0.5f); 
		Gear[1].transform.eulerAngles += new Vector3 (0, 0, -0.665f);
		Gear[2].transform.eulerAngles += new Vector3 (0, 0, 0.5f);
		Gear[3].transform.eulerAngles += new Vector3 (0, 0, -0.5f);

		// 徐々に大きさを変える処理
		buff += buff_difference/(MAX_ROT/ANIMATION_SPEED);
		// 徐々に中心を元に戻す処理
		if (CenterX != 0) {
			CenterX -= CenterDiferenceX /(MAX_ROT/ANIMATION_SPEED);
			if(Mathf.Abs(CenterX) < buff)
				CenterX = 0;
		}
		if (CenterY != 0) {
			CenterY -= CenterDiferenceY /(MAX_ROT/ANIMATION_SPEED);
			if(Mathf.Abs(CenterY) < buff)
				CenterY = 0;
		}

		// 支点と移動距離を確定
		switch(key)
		{
		case CUT.CUT_DOWN:
			// 上から下
			pivot_x = -buff/2;
			pivot_y = -(Map_Y*buff/2+buff/2);
			adjust_y = (next_Map_Y/2+Map_Y/2)/MAX_ROT*buff;
			break;
		case CUT.CUT_UP:
			// 下から上
			pivot_x = -buff/2;
			pivot_y = Map_Y*buff/2-buff/2;
			adjust_y = -(next_Map_Y/2+Map_Y/2)/MAX_ROT*buff;
			break;
		case CUT.CUT_RIGHT:
			// 左から右
			pivot_x = Map_X*buff/2-buff/2;
			pivot_y = -buff/2;
			adjust_x = -(next_Map_X/2+Map_X/2)/MAX_ROT*buff;
			break;
		case CUT.CUT_LEFT:
			// 右から左
			pivot_x = -(Map_X*buff/2+buff/2);
			pivot_y = -buff/2;
			adjust_x = (next_Map_X/2+Map_X/2)/MAX_ROT*buff;
			break;
		}
		
		// クローン操作用変数
		int count = 0;
		
		for (int Y = Map_Y-1; Y >= 0; Y--)
		{
			for (int X = 0; X < Map_X; X++)
			{
				// 傾く方向の確定
				switch(key)
				{
				case CUT.CUT_DOWN:
					// 上から下
					if(X < Map_X/2)
						rot = animation_count*ANIMATION_SPEED;
					else
						rot = -animation_count*ANIMATION_SPEED;
					break;
				case CUT.CUT_UP:
					// 下から上
					if(X < Map_X/2)
						rot = -animation_count*ANIMATION_SPEED;
					else
						rot = animation_count*ANIMATION_SPEED;
					break;
				case CUT.CUT_RIGHT:
					// 左から右
					if(Y < Map_Y/2)
						rot = animation_count*ANIMATION_SPEED;
					else
						rot = -animation_count*ANIMATION_SPEED;
					break;
				case CUT.CUT_LEFT:
					// 右から左
					if(Y < Map_Y/2)
						rot = -animation_count*ANIMATION_SPEED;
					else
						rot = animation_count*ANIMATION_SPEED;
					break;
				}

				// 角度をラジアンに変換
				float radian = (float)(rot * PI / 180.0);
				// アニメーション中の座標に変換
				posX = ((X-Map_X/2)*buff - pivot_x) * Mathf.Cos(radian) - ((Y-Map_Y/2)*buff - pivot_y) * Mathf.Sin(radian) + pivot_x;
				posY = ((X-Map_X/2)*buff - pivot_x) * Mathf.Sin(radian) + ((Y-Map_Y/2)*buff - pivot_y) * Mathf.Cos(radian) + pivot_y;

				// ギミックのアニメーション
				if(Map_Input.mapdata[Y,X] == MAP_GIMMIC.APPEAR){
					for (int i=0; i<MapAction.appear_count; i++) {
						if(Map_Input.appear[i].X == X && Map_Input.appear[i].Y == Y){
							Map_Input.appear[i].number.transform.localPosition = new Vector3(posX+(adjust_x*animation_count*ANIMATION_SPEED)+buff/2, posY+(adjust_y*animation_count*ANIMATION_SPEED)+buff/2, -1);
							Map_Input.appear[i].number.transform.localScale = new Vector3(buff,buff,1);
							Map_Input.appear[i].number.transform.eulerAngles = new Vector3(0,0,rot);
						}
					}
				}	
				if(Map_Input.mapdata[Y,X] == MAP_GIMMIC.DISAPPEAR){
					for (int i=0; i<MapAction.disappear_count; i++) {
						if(Map_Input.disappear[i].X == X && Map_Input.disappear[i].Y == Y){
							Map_Input.disappear[i].number.transform.localPosition = new Vector3(posX+(adjust_x*animation_count*ANIMATION_SPEED)+buff/2, posY+(adjust_y*animation_count*ANIMATION_SPEED)+buff/2, -1);
							Map_Input.disappear[i].number.transform.localScale = new Vector3(buff,buff,1);
							Map_Input.disappear[i].number.transform.eulerAngles = new Vector3(0,0,rot);
						}
					}
				}
				// プレイヤーのアニメーション
				if(X == Player.X && Y == Player.Y){
					N_player.transform.localPosition = new Vector3(posX+(adjust_x*animation_count*ANIMATION_SPEED)+buff/2, posY+(adjust_y*animation_count*ANIMATION_SPEED)+buff/2, -1);
					N_player.transform.localScale = new Vector3(buff,buff,1);
					N_player.transform.eulerAngles = new Vector3(0,0,Player.angle+rot);
				}
				N_block[count].transform.localPosition = new Vector3(posX+(adjust_x*animation_count*ANIMATION_SPEED)+buff/2, posY+(adjust_y*animation_count*ANIMATION_SPEED)+buff/2, -1);
				N_block[count].transform.localScale = new Vector3(buff,buff,1);
				N_block[count].transform.eulerAngles = new Vector3(0,0,rot);
				count++;
			}
		}

		player_draw_flag = false;

		if(animation_count*ANIMATION_SPEED >= MAX_ROT)
		{
			// カウントリセット
			animation_count = 0;
			Player.angle -= rot;
			// アニメーションが終わると終了
			return true;
		}
		// アニメーションが終わっていないので繰り返し
		return false;
	}

	// S磁石のギミックチェック
	public static bool S_GimmickCheck(){
		
		// S極とS極の判定(隣り合っていればtrue)
		for(int i=0;i<magnet_s_count-1;i++){
			for(int j=i+1;j<magnet_s_count;j++){
				if((magnet_s_position[i].x == magnet_s_position[j].x && Mathf.Abs(magnet_s_position[i].y-magnet_s_position[j].y) == 1)
				   || (magnet_s_position[i].y == magnet_s_position[j].y && Mathf.Abs(magnet_s_position[i].x-magnet_s_position[j].x) == 1))
					return true;
			}
		}
		return false;
	}

	// N磁石のギミックチェック
	void N_GimmickAction(){
		int isHit = 0;
		
		// S極とN極の判定
		for (int i=0;i<magnet_n_count;i++) {
			for(int j=0; j<magnet_s_count; j++){
				// S極と隣り合っていないか
				if((magnet_n_position[i].x == magnet_s_position[j].x && Mathf.Abs(magnet_n_position[i].y - magnet_s_position[j].y) == 1)
				   || (magnet_n_position[i].y == magnet_s_position[j].y && Mathf.Abs(magnet_n_position[i].x - magnet_s_position[j].x) == 1)){
					isHit++;
				}
			}
			// 下に何もないか
			if(magnet_n_position[i].y > 0 && isHit == 0){
				if (Map_Input.mapdata [(int)magnet_n_position[i].y - 1, (int)magnet_n_position[i].x] == MAP_GIMMIC.NONE){
					draw_flag = true;
					move_flag = true;
					Map_Input.mapdata [(int)magnet_n_position[i].y - 1, (int)magnet_n_position[i].x] = MAP_GIMMIC.MAGNET_N;
					Map_Input.mapdata [(int)magnet_n_position[i].y, (int)magnet_n_position[i].x] = MAP_GIMMIC.NONE;
				}
			}
			isHit = 0;
		}
	}


	// 現れる/消えるブロックのギミックチェック
	void AppearGimmicAction(){
		int X, Y;
		// 時間経過で現れるブロックの処理
		for (int i=0; i<MapAction.appear_count; i++) {
			if(Map_Input.appear[i].trouble <= Trouble){
				Map_Input.appear[i].number.SetActive(false);
				if(Map_Input.appear[i].X >= 0 && Map_Input.appear[i].Y >= 0){
					X = Map_Input.appear[i].X;
					Y = Map_Input.appear[i].Y;

					if(Map_Input.mapdata[Y,X] == MAP_GIMMIC.APPEAR){
						Map_Input.mapdata[Y,X] = MAP_GIMMIC.BLOCK;
						// 使われないようにしておく
						Map_Input.appear[i].X = Map_Input.appear[i].Y = -1;
						if(GimmicParticle.particleSystem.isStopped)
							GimmicParticle.particleSystem.Play();
						PlaySE.isPlayGimmic = true;
					}
				}
			}

			else{
				Map_Input.appear[i].number.SetActive(true);
				Map_Input.appear[i].number.GetComponent<UISprite> ().spriteName = "Number" + (Map_Input.appear[i].trouble-Trouble);
			}
		}

		// 時間経過で消えるブロックの処理
		for (int i=0; i<MapAction.disappear_count; i++) {
			if(Map_Input.disappear[i].trouble <= Trouble){
				Map_Input.disappear[i].number.SetActive(false);
				if(Map_Input.disappear[i].X >= 0 && Map_Input.disappear[i].Y >= 0){
					X = Map_Input.disappear[i].X;
					Y = Map_Input.disappear[i].Y;
					
					if(Map_Input.mapdata[Y,X] == MAP_GIMMIC.DISAPPEAR){
						Map_Input.mapdata[Y,X] = MAP_GIMMIC.NONE;
						// 使われないようにしておく
						Map_Input.disappear[i].X = Map_Input.disappear[i].Y = -1;
						if(GimmicParticle.particleSystem.isStopped)
							GimmicParticle.particleSystem.Play();
						PlaySE.isPlayGimmic = true;
					}
				}
			}
			else{
				Map_Input.disappear[i].number.SetActive(true);
				Map_Input.disappear[i].number.GetComponent<UISprite> ().spriteName = "Number" + (Map_Input.disappear[i].trouble-Trouble);
			}
		}
	}

	// カメラ操作
	void GameCamera(){

		// カメラ移動処理
		if (Input.touchCount == 1) {
			CenterX += CameraMovePos.x-TouchAction.direction[0].x;
			CenterY += CameraMovePos.y-TouchAction.direction[0].y;

			draw_flag = true;
		}
		CameraMovePos.x = (int)TouchAction.direction [0].x;
		CameraMovePos.y = (int)TouchAction.direction [0].y;

		// ズーム処理
		if (Input.GetKey (KeyCode.Z)) {
			buff++;
			draw_flag = true;
		}
		if (TouchAction.zoom != ZOOM.NONE) {
			if(TouchAction.zoom == ZOOM.ZOOM_IN && buff < BUFF_L)
				buff++;
			if(TouchAction.zoom == ZOOM.ZOOM_OUT && buff > BUFF_S)
				buff--;
			draw_flag = true;
		}
	}
}

