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
		//var before = DateTime.Now;
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
		var subStance = ground.renderer.sharedMaterial as ProceduralMaterial;
		//var props = subStance.GetProceduralPropertyDescriptions();
		//props.ToList().ForEach( o => Debug.Log("Name: " + o.name + ", value: " + o.step.ToString()));

		//subStance.SetProceduralFloat("Zoom", 1.0f);
		subStance.SetProceduralFloat("$randomseed", UnityEngine.Random.Range(0, 10000));
		subStance.RebuildTextures();

		//var after = DateTime.Now.Subtract(before).Milliseconds;
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
