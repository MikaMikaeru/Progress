using UnityEngine;
using System.Collections;

enum SOUND_NUMBER
{
	NONE,
	OPENING,
	GAMEPLAY
}

public class PlayBGM : MonoBehaviour {
	
	private SOUND_NUMBER NowPlaying;
	
	private AudioSource BGM_Opening;
	private AudioSource BGM_GamePlay;
	
	public static float max_volume = 1;
	private float before_max_volume = max_volume;
	private const float FADE_VOLUME = 0.01f;
	
	public static bool isPlayOpening;
	private bool isFadeOpening;
	
	public static bool isPlayGamePlay;
	private bool isFadeGamePlay;
	
	// Use this for initialization
	void Start () {
		// AudioSourceコンポーネントを取得し、変数に格納
		AudioSource[] audioSources = GetComponents<AudioSource>();
		BGM_Opening = audioSources[0];
		BGM_GamePlay = audioSources[1];
		
		NowPlaying = SOUND_NUMBER.NONE;
	}
	
	// Update is called once per frame
	void Update () {
		// フェード中に遷移した時の初期化
		if (isPlayOpening && isPlayGamePlay) {
			isPlayOpening = isPlayGamePlay = false;
			isFadeOpening = isFadeGamePlay = false;
		}
		//----------------------------
		// オープニング
		//----------------------------
		// フラグが立っていて再生中でなければ再生
		if(isPlayOpening && !isPlayGamePlay){
			if(NowPlaying != SOUND_NUMBER.OPENING) {
				NowPlaying = SOUND_NUMBER.OPENING;
				isFadeOpening = true; 
				BGM_Opening.time = 0;
			}
			isPlayOpening = false;
			if(before_max_volume != max_volume){
				isFadeOpening = true; 
				before_max_volume = max_volume;
			}
		}
		
		// フェード処理
		if (isFadeOpening){
			if(BGM_Opening.volume < max_volume) {
				BGM_Opening.volume += FADE_VOLUME;
				BGM_GamePlay.volume -= FADE_VOLUME;
			} else {
				BGM_Opening.volume = max_volume;
				BGM_GamePlay.volume = 0;
				isFadeOpening = false;
			}
		}
		
		//----------------------------
		// ゲーム中
		//----------------------------
		// フラグが立っていて再生中でなければ再生
		if(isPlayGamePlay && !isPlayOpening){
			if(NowPlaying != SOUND_NUMBER.GAMEPLAY) {
				NowPlaying = SOUND_NUMBER.GAMEPLAY;
				isFadeGamePlay = true; 
				BGM_GamePlay.time = 0;
			}
			isPlayGamePlay = false;
			if(before_max_volume != max_volume){
				isFadeGamePlay = true; 
				before_max_volume = max_volume;
			}
		}
		
		// フェード処理
		if (isFadeGamePlay){
			if(BGM_GamePlay.volume < max_volume) {
				BGM_GamePlay.volume += FADE_VOLUME;
				BGM_Opening.volume -= FADE_VOLUME;
			} else {
				BGM_GamePlay.volume = max_volume;
				BGM_Opening.volume = 0;
				isFadeGamePlay = false;
			}
		}
	}
}
