using UnityEngine;
using System.Collections;

public enum TUTORIAL{
	NONE,
	STAGE1_1,
	STAGE1_2,
	STAGE1_3_1,
	STAGE1_3_2,
	STAGE_SELECT,
	STAGE_CLEAR1,
	STAGE_CLEAR2,
	STAGE_CLEAR3
}

public class GameGuide : MonoBehaviour {

	// チュートリアル中かどうか
	public static bool isGuide;
	
	private int GuideNumber;	// ガイドのページ番号

	public UILabel GuideLabel;	// ガイド用文字列

	public GameObject GuideWindow;	// ガイド表示枠

	public static TUTORIAL NowTutorial;	// 現在のチュートリアル
	
	// ガイド番号管理用
	private int GuideIndex = 0;
	private int MaxGuideNumber;
	private string GuideText;

	private const int TEXT_MAX_1_1 = 2;
	private string[] Guide1_1 = new string[TEXT_MAX_1_1]{
		"歯車を動かしてみよう!",
		"端末を左右に傾けると歯車が左右に動くよ!\n※上下の傾きでは移動できません!"};
	private const int TEXT_MAX_1_2 = 1;
	private string[] Guide1_2 = new string[TEXT_MAX_1_2]{
		"次はステージを横に切ってみよう!\n上下左右からスワイプするとステージが切れるよ!"};

	private const int TEXT_MAX_1_3_1 = 2;
	private string[] Guide1_3_1 = new string[TEXT_MAX_1_3_1]{
		"このステージでは、時間によって性質が変わるブロックがあります。\nステージ中央にある1と書いてあるブロックです。",
		"今回の場合はブロックが中央に現れるようです。\n上に向かってステージを切ってみましょう!"};

	private const int TEXT_MAX_1_3_2 = 3;
	private string[] Guide1_3_2 = new string[TEXT_MAX_1_3_2]{
		"切り替えて時間を進めることにより、ブロックが現れました!\nブロックに書いてある数字は,\n残り何回切ると性質が変わるかを示しています。",
		"右側のカメラボタンを押すとカメラモードになります。\nカメラモードの時はピンチイン、ピンチアウトでズームができます。",
		"カメラボタンを押している間はステージを切ることができません。\nもう一度カメラボタンを押すとカメラモードが解除されます。"};

	private const int TEXT_MAX_STAGE_SELECT = 3;
	private string[] GuideStageSelect = new string[TEXT_MAX_STAGE_SELECT]{
		"ようこそprogressの世界へ!\nここでは、時間によってステージが変化していきます。\n最短の手でステージをクリアし、より多くの歯車を集めましょう!!",
		"まずはステージを選択してみましょう!\n左側の歯車を回すと、遊べるステージが切り替わります。",
		"まずは1を選んでみましょう!\n右側のステージをタップしてOKを選択してみてください!"};

	private const int TEXT_MAX_STAGE_CLEAR1 = 2;
	private string[] GuideStageClear1 = new string[TEXT_MAX_STAGE_CLEAR1]{
		"無事最短手でクリアできたようですね!!",
		"最短でクリアして歯車を3つ集めたことにより、\n新たなステージが解放されました。\n次は2を選んでみましょう!"};
			
	private const int TEXT_MAX_STAGE_CLEAR2 = 3;
	private string[] GuideStageClear2 = new string[TEXT_MAX_STAGE_CLEAR2]{
		"ここも最短手でクリアできたようですね!!\nこのゲームでは切ってステージを切り替えることにより時間が進みます。",
		"切る回数が多すぎると\nクリアした時にもらえる歯車も少なくなってしまいます。\nそんな時はもう一度同じステージを遊んでみるのも良いかもしれません。",
		"最後に3を選んでみましょう!"};

	private const int TEXT_MAX_STAGE_CLEAR3 = 1;
	private string[] GuideStageClear3 = new string[TEXT_MAX_STAGE_CLEAR3]{
		"これでチュートリアルはおしまいです!\n最短クリアを目指して頑張ってください!!"};

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

		// フェード中とセッティング中の操作をはじく
		if ((Fade.NowFade != FADE.FADE_NONE) && Setting.isSetting) 
			return;

		// 終了条件
		if (NowTutorial == TUTORIAL.NONE) {
			isGuide = false;
			GameMain.GuideFlag = false;
			GuideNumber = 0;
			return;
		}

		isGuide = true;

		// チュートリアル実行時の処理
		switch (NowTutorial) {
		case TUTORIAL.STAGE_SELECT:
			// テキストサイズを設定
			MaxGuideNumber = TEXT_MAX_STAGE_SELECT;
			// テキストページを設定
			if (GuideNumber < MaxGuideNumber)
				GuideText = GuideStageSelect [GuideNumber];
			else
				NowTutorial = TUTORIAL.NONE;
			break;
		case TUTORIAL.STAGE1_1:
			// テキストサイズを設定
			MaxGuideNumber = TEXT_MAX_1_1;
			// テキストページを設定
			if (GuideNumber < MaxGuideNumber)
				GuideText = Guide1_1 [GuideNumber];
			else
				NowTutorial = TUTORIAL.NONE;
			break;
		case TUTORIAL.STAGE1_2:
			// テキストサイズを設定
			MaxGuideNumber = TEXT_MAX_1_2;
			// テキストページを設定
			if (GuideNumber < MaxGuideNumber)
				GuideText = Guide1_2 [GuideNumber];
			else
				NowTutorial = TUTORIAL.NONE;
			break;
		case TUTORIAL.STAGE1_3_1:
			// テキストサイズを設定
			MaxGuideNumber = TEXT_MAX_1_3_1;
			// テキストページを設定
			if (GuideNumber < MaxGuideNumber)
				GuideText = Guide1_3_1 [GuideNumber];
			else
				NowTutorial = TUTORIAL.NONE;
			break;
		
		case TUTORIAL.STAGE1_3_2:
			// テキストサイズを設定
			MaxGuideNumber = TEXT_MAX_1_3_2;
			// テキストページを設定
			if (GuideNumber < MaxGuideNumber)
				GuideText = Guide1_3_2 [GuideNumber];
			else
				NowTutorial = TUTORIAL.NONE;
			break;

		case TUTORIAL.STAGE_CLEAR1:
			// テキストサイズを設定
			MaxGuideNumber = TEXT_MAX_STAGE_CLEAR1;
			// テキストページを設定
			if (GuideNumber < MaxGuideNumber)
				GuideText = GuideStageClear1 [GuideNumber];
			else
				NowTutorial = TUTORIAL.NONE;
			break;

		case TUTORIAL.STAGE_CLEAR2:
			// テキストサイズを設定
			MaxGuideNumber = TEXT_MAX_STAGE_CLEAR2;
			// テキストページを設定
			if (GuideNumber < MaxGuideNumber)
				GuideText = GuideStageClear2 [GuideNumber];
			else
				NowTutorial = TUTORIAL.NONE;
			break;

		case TUTORIAL.STAGE_CLEAR3:
			// テキストサイズを設定
			MaxGuideNumber = TEXT_MAX_STAGE_CLEAR3;
			// テキストページを設定
			if (GuideNumber < MaxGuideNumber)
				GuideText = GuideStageClear3 [GuideNumber];
			else
				NowTutorial = TUTORIAL.NONE;
			break;

		default:
			NowTutorial = TUTORIAL.NONE;
			break;
		}

		// テキストの読み込み
		GuideLabel.text = GuideText.Substring (0, GuideIndex);
		// テキストアニメーション処理
		if (GuideIndex < GuideText.Length)
			GuideIndex++;

		// 画面にタップすると次のガイドへ
		if(GuideAction.isPress){
			// チュートリアルを飛ばせないようにする
			if (GuideIndex >= GuideText.Length){
				GuideNumber++;
				GuideIndex = 0;
			}
			else// メッセージの早送り
				GuideIndex = GuideText.Length;

			GuideAction.isPress = false;
		}
	}
}