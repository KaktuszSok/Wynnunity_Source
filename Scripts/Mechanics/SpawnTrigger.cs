using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnTrigger : MonoBehaviour {

	public List<GameObject> MobsInTrigger = new List<GameObject>();
	public SpawnArea SA;
	
	void Start () {
		if (!SA) {
			SA = transform.parent.GetComponent<SpawnArea>();
		}
	}

	void Update () {
		for (int i = 0; i < MobsInTrigger.Count; i++) {
			if(MobsInTrigger[i] == null) {
				MobsInTrigger.RemoveAt (i);
			}
		}
	}

	void OnTriggerStay (Collider col) {
		if (col.transform.root.GetComponent<MobsSpawnArea>() && col.transform.root.GetComponent<MobsSpawnArea>().MSA == SA && !MobsInTrigger.Contains(col.transform.root.gameObject)) {
			MobsInTrigger.Add (col.transform.root.gameObject);
		}

		if (col.transform.root.tag == "Player" && col.transform.root.GetComponent<Health> ().health != 0) {
			SA.activated = true;
		} else if (col.transform.root.tag == "Player") {
			SA.activated = false;
		}
	}

	void OnTriggerExit (Collider col) {
		if(MobsInTrigger.Contains (col.transform.root.gameObject)) {
			MobsInTrigger.Remove (col.transform.root.gameObject);
		}

		if (col.transform.root.tag == "Player") {
			SA.activated = false;
		}
	}
}