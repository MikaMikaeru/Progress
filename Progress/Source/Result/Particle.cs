using UnityEngine;
using System.Collections;

public class Particle : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.particleSystem.isStopped) {
			transform.localPosition = new Vector3 (UnityEngine.Random.Range (-Screen.width / 2, Screen.width / 2), UnityEngine.Random.Range (-Screen.height / 2, Screen.height / 2), 0);
			transform.particleSystem.Play();
		}
	}
}
