using UnityEngine;
using System.Collections;

public class TrailFX : MonoBehaviour {

	private ParticleSystemRenderer FX;

	// Use this for initialization
	void Start () {
		FX = GetComponent<ParticleSystemRenderer> ();
		if(Options.ThreeDTrail)
			FX.renderMode = ParticleSystemRenderMode.Mesh;
		else
			FX.renderMode = ParticleSystemRenderMode.Billboard;
	}
	
	// Update is called once per frame
	void Update () {
		if(Options.ThreeDTrail)
			FX.renderMode = ParticleSystemRenderMode.Mesh;
		else
			FX.renderMode = ParticleSystemRenderMode.Billboard;
	}
}
