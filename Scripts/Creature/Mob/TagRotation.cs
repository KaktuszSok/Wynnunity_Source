using UnityEngine;
using System.Collections;

public class TagRotation : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 camPos = Camera.main.transform.position;
		camPos.y = transform.position.y;
		transform.LookAt (camPos);
	}
}
