using UnityEngine;
using System.Collections;

public class Options : MonoBehaviour {

	public static bool ThreeDSmoke = false;
	public static bool ThreeDTrail = false;

	public void ThreeDSmokeFX (bool isEnabled) {
		ThreeDSmoke = isEnabled;
	}

	public void ThreeDTrailFX (bool isEnabled) {
		ThreeDTrail = isEnabled;
	}
}
