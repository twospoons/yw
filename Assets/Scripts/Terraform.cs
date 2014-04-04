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
	public Vector3 CurrentSize;

	public Vector3 Position  = new Vector3(0, -0.8f, 0);
	public Vector3 SmallSize = new Vector3(50, 0.5f, 50);
	public Vector3 MediumSize = new Vector3(100, 0.5f, 100);
	public Vector3 LargeSize = new Vector3(150, 0.5f, 150);

	public int TextureWidth = 256;
	public int TextureHeight = 256;

	private float[,] water;
	private float[,] ore;
	private List<Vector2> waterCoords;
	private List<Vector2> oreCoords;

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
		CurrentSize = size;
		ground.transform.localScale = size;
		ground.transform.position = Position;
		GenerateWater(ground);
		GenerateOre(ground);

		//var subStance = ground.renderer.sharedMaterial as ProceduralMaterial;
		//var props = subStance.GetProceduralPropertyDescriptions();
		//props.ToList().ForEach( o => Debug.Log("Name: " + o.name + ", value: " + o.step.ToString()));

		//subStance.SetProceduralFloat("Zoom", 1.0f);
		//subStance.SetProceduralFloat("$randomseed", UnityEngine.Random.Range(0, 10000));
		//subStance.RebuildTextures();


		//var after = DateTime.Now.Subtract(before).Milliseconds;
	}
	void GenerateWater(Transform ground) {
		water = new float[TextureWidth, TextureHeight];
		waterCoords = new List<Vector2>();
		var noise = new SimplexNoiseGenerator();
		// test creating a noise texture for water
		var tex = new Texture2D(TextureWidth, TextureHeight);
		for(var x = 0; x < tex.width; x++) {
			for(var y = 0; y < tex.height; y++) {
				// multiplier affects how large the patter is; the large the number the bigger the waves (noise)
				// basically a small value will leave a bunch of small specles, large number larger objects
				// noise.coherentNoise(x, y, 0, 10, 75, 1); -> looks like a star map
				// noise.coherentNoise(x, y, 0, 1, 75, 2); -> clouds
				var noi = noise.coherentNoise(x, y, 0, 2, 125, 2);
				var color = new Color();
				if(noi > 0.5f) {
					// water
					color = new Color(0, 0, noi, noi);
					float percentX = x * 1.0f / TextureWidth * 1.0f;
					float percentZ = y * 1.0f / TextureHeight * 1.0f;
					
					// zero is in the center of the map, so we need to subtract half of the % number with the width
					float xx = (CurrentSize.x * percentX) - (CurrentSize.x / 2.0f);
					float zz = (CurrentSize.z * percentZ) - (CurrentSize.z / 2.0f);
					waterCoords.Add(new Vector2(xx, zz));
				} else if(noi > 0.2f) {
					// grass
					//color = new Color(0, noi, 0, 0.8f);
				}

				water[x, y] = noi;
				tex.SetPixel(x, y, color);
			}
		}
		tex.filterMode = FilterMode.Trilinear;
		tex.Apply();
		ground.renderer.sharedMaterials[0].mainTexture = tex;

	}

	void GenerateOre(Transform ground) {
		ore = new float[TextureWidth, TextureHeight];
		oreCoords = new List<Vector2>();
		var noise = new SimplexNoiseGenerator();
		// test creating a noise texture for water
		var tex = new Texture2D(TextureWidth, TextureHeight);
		for(var x = 0; x < tex.width; x++) {
			for(var y = 0; y < tex.height; y++) {
				// multiplier affects how large the patter is; the large the number the bigger the waves (noise)
				// basically a small value will leave a bunch of small specles, large number larger objects
				// noise.coherentNoise(x, y, 0, 10, 75, 1); -> looks like a star map
				// noise.coherentNoise(x, y, 0, 1, 75, 2); -> clouds
				var noi = noise.coherentNoise(x, y, 0, 4, 75, 1);
				var color = new Color();
				if(noi > 0.4f) {
					color = new Color(noi, noi, noi, noi * 2);
				}

				ore[x, y] = noi;
				oreCoords.Add(new Vector2(x, y));
				tex.SetPixel(x, y, color);
			}
		}
		tex.filterMode = FilterMode.Trilinear;
		tex.Apply();
		ground.renderer.sharedMaterials[1].mainTexture = tex;

	}

	GameObject pointLight;
	// Use this for initialization
	void Start () {
		BuildTerrain();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public Vector2 FindClosestWater(Vector2 from) {
		if(waterCoords.Count == 0) {
			return Vector2.zero;
		}
		//var dists = waterCoords.OrderBy(o => Vector2.Distance(o, from)).Select(o => Vector2.Distance(o, from)).ToList(); 
 		var ret = waterCoords.OrderByDescending(o => Vector2.Distance(o, from)).First();
		return ret;
	}

	public Vector3 FindClosestOre(Vector3 from) {
		if(oreCoords.Count == 0) {
			return Vector2.zero;
		}
		var ret = oreCoords.OrderBy(o => Vector2.Distance(o, from)).First();
		return ret;
	}


}
