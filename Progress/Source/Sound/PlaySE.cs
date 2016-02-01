using UnityEngine;
using System.Collections;

public class PlaySE : MonoBehaviour {

	private AudioSource SE_Gear1;
	private AudioSource SE_Gear2;
	private AudioSource SE_Gear3;
	private AudioSource SE_Select;
	private AudioSource SE_Move;
	private AudioSource SE_Animation;
	private AudioSource SE_Gimmic;

	public static bool isPlayGear1;
	public static bool isPlayGear2;
	public static bool isPlayGear3;
	public static bool isPlaySelect;
	public static bool isPlayMove;
	public static bool isPlayAnimation;
	public static bool isPlayGimmic;

	public static float max_volume = 1;
	private float before_max_volume = max_volume;
	
	// Use this for initialization
	void Start () {
		//AudioSourceコンポーネントを取得し、変数に格納
		AudioSource[] audioSources = GetComponents<AudioSource>();
		SE_Gear1 = audioSources[0];
		SE_Gear2 = audioSources[1];
		SE_Gear3 = audioSources[2];
		SE_Select = audioSources[3];
		SE_Move = audioSources[4];
		SE_Animation = audioSources[5];
		SE_Gimmic = audioSources[6];
	}
	
	// Update is called once per frame
	void Update () {
		if(isPlayGimmic) {
			isPlayGimmic = false;
			if(!SE_Gimmic.isPlaying)
				SE_Gimmic.PlayOneShot(SE_Gimmic.clip);
		}
		
		if(isPlayGear1) {
			isPlayGear1 = false;
			if(!SE_Gear1.isPlaying)
				SE_Gear1.PlayOneShot(SE_Gear1.clip);
		}
		
		if(isPlayGear2) {
			isPlayGear2 = false;
			if(!SE_Gear2.isPlaying)
				SE_Gear2.PlayOneShot(SE_Gear2.clip);
		}
		
		if(isPlayGear3) {
			isPlayGear3 = false;
			if(!SE_Gear3.isPlaying)
				SE_Gear3.PlayOneShot(SE_Gear3.clip);
		}
		
		if(isPlaySelect) {
			isPlaySelect = false;
			if(!SE_Select.isPlaying)
				SE_Select.PlayOneShot(SE_Select.clip);
		}
		
		if(isPlayMove) {
			isPlayMove = false;
			if(!SE_Move.isPlaying)
				SE_Move.PlayOneShot(SE_Move.clip);
		}
		
		if(isPlayAnimation) {
			isPlayAnimation = false;
			if(!SE_Animation.isPlaying)
				SE_Animation.PlayOneShot(SE_Animation.clip);
		}

		// 音量調整
		if(before_max_volume != max_volume){
			// 確認用SE再生
			isPlaySelect = true;
			SE_Gear1.volume = max_volume;
			SE_Gear2.volume = max_volume;
			SE_Gear3.volume = max_volume;
			SE_Select.volume = max_volume;
			SE_Move.volume = max_volume;
			SE_Animation.volume = max_volume;
			SE_Gimmic.volume = max_volume;

			before_max_volume = max_volume;
		}
	}
}
