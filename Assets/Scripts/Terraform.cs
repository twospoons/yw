using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

[ExecuteInEditMode]
public class Terraform : MonoBehaviour {

	public enum TerraformSize {
		Small,
		Medium,
		Large
	}

	public TerraformSize Size;
	public Vector3 Position  = new Vector3(0, -0.8f, 0);
	public Vector3 SmallSize = new Vector3(50, 0.5f, 50);
	public Vector3 MediumSize = new Vector3(100, 0.5f, 100);
	public Vector3 LargeSize = new Vector3(150, 0.5f, 150);

	//public Material GroundMaterial;

	public void BuildTerrain() {
		//if(GroundMaterial == null) {
		//	Debug.Log("No material found, cannot render");
		//	return;
		//}

		var before = DateTime.Now;
		// create the world
		
		var ground = transform;
		var size = MediumSize;
		if(Size == TerraformSize.Small) {
			size = SmallSize;
		} else if(Size == TerraformSize.Medium) {
			size = MediumSize;
		} else if(Size == TerraformSize.Large) {
			size = LargeSize;
		}
		
		ground.transform.localScale = size;
		ground.transform.position = Position;
		//GroundMaterial.SetInt("Flow", 1000);
		//ground.renderer.sharedMaterial = GroundMaterial;

		var subStance = ground.renderer.sharedMaterial as ProceduralMaterial;
		var props = subStance.GetProceduralPropertyDescriptions();
		//props.ToList().ForEach( o => Debug.Log("Name: " + o.name + ", value: " + o.step.ToString()));

		subStance.SetProceduralFloat("Zoom", 1.0f);
		subStance.RebuildTextures();

		var after = DateTime.Now.Subtract(before).Milliseconds;
		Debug.Log("Done in: " + after.ToString() + "ms");
		//Debug.Log (ground.renderer.material.GetFloat("Flow"));
		//ground.renderer.material.SetFloat("Zoom", 1.0f);
	}
	
	GameObject pointLight;
	// Use this for initialization
	void Start () {
		BuildTerrain();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
