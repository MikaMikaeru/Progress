using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour {
	
	public static float Scale1_1;
	
	// Use this for initialization
	void Start (){
		// スケールを1:1に固定
		if (Screen.width / 9 < Screen.height / 16)
			Scale1_1 = Screen.width;
		else
			Scale1_1 = Screen.height;
		
		transform.localScale = new Vector3(Scale1_1,Scale1_1,1);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
