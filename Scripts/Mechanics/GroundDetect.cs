using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GroundDetect : MonoBehaviour {


	public float dist = 0.01f;
	public List<Transform> checks;
	public int failedCount = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}

	public bool chckdist() {
		failedCount = 0;
		foreach (Transform check in checks) {
			if(Physics.Raycast(check.position, -check.transform.up, dist, Player.EnemyLOS))
			{
				return true;
			}
			else
			{
				failedCount ++;
			}
		}
		if (failedCount >= checks.Count) {
			return false;
		} else {
			return true;
		}
	}
}
