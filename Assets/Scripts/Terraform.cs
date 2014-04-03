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
		//var subStance = ground.renderer.sharedMaterial as ProceduralMaterial;
		//var props = subStance.GetProceduralPropertyDescriptions();
		//props.ToList().ForEach( o => Debug.Log("Name: " + o.name + ", value: " + o.step.ToString()));

		//subStance.SetProceduralFloat("Zoom", 1.0f);
		//subStance.SetProceduralFloat("$randomseed", UnityEngine.Random.Range(0, 10000));
		//subStance.RebuildTextures();

		var noise = new SimplexNoiseGenerator();
		// test creating a noise texture for fun
		var tex = new Texture2D(256, 256);
		for(var x = 0; x < tex.width; x++) {
			for(var y = 0; y < tex.height; y++) {
				// multiplier affects how large the patter is; the large the number the bigger the waves (noise)
				// basically a small value will leave a bunch of small specles, large number larger objects
				// noise.coherentNoise(x, y, 0, 10, 75, 1); -> looks like a star map
				// noise.coherentNoise(x, y, 0, 1, 75, 2); -> clouds
				var noi = noise.coherentNoise(x, y, 0, 1, 75, 2);
				var color = new Color();
				if(noi > 0.2f) {
					color = new Color(0, 0, noi, noi);
				} else {
					color = new Color(0, 0, 0, 0);
				}
				tex.SetPixel(x, y, color);
			}
		}
		tex.filterMode = FilterMode.Trilinear;
		tex.Apply();
		ground.renderer.sharedMaterial.mainTexture = tex;
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
