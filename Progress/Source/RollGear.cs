using UnityEngine;
using System.Collections;

public class RollGear : MonoBehaviour {

	public float Rotation;

	private int RollCount;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		transform.eulerAngles += new Vector3 (0, 0, Rotation);
	}
}
