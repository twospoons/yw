﻿using UnityEngine;
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

	private List<Vector2> waterBeingCollectedAt;

	public List<Vector2> GetWaterCoords() {
		return waterCoords;
	}

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
	void GenerateWaterTest(Transform ground) {
		water = new float[TextureWidth, TextureHeight];
		waterCoords = new List<Vector2>();
		var noise = new SimplexNoiseGenerator();
		// test creating a noise texture for water
		var tex = new Texture2D(TextureWidth, TextureHeight);
		for(var x = 0; x < tex.width; x++) {
			for(var y = 0; y < tex.height; y++) {
				var noi = noise.coherentNoise(x, y, 0, 2, 125, 2);
				var color = new Color();
				if(x == 100 && (y >= 100 && y <= 158)) {
					waterCoords.Add(TextureToWorldCoords(x, y));
				}
				water[x, y] = noi;
				tex.SetPixel(x, y, color);
			}
		}
		tex.filterMode = FilterMode.Trilinear;
		tex.Apply();
		ground.renderer.sharedMaterials[0].mainTexture = tex;
		
	}

	void UpdateWaterTexture() {
		waterCoords = new List<Vector2>();
		// test creating a noise texture for water
		var tex = new Texture2D(TextureWidth, TextureHeight);
		for(var x = 0; x < tex.width; x++) {
			for(var y = 0; y < tex.height; y++) {
				// multiplier affects how large the patter is; the large the number the bigger the waves (noise)
				// basically a small value will leave a bunch of small specles, large number larger objects
				// noise.coherentNoise(x, y, 0, 10, 75, 1); -> looks like a star map
				// noise.coherentNoise(x, y, 0, 1, 75, 2); -> clouds
				var color = new Color();
				var noi = water[x, y];
				if(noi > 0.5f) {
					// water
					color = new Color(0, 0, noi, noi);
					waterCoords.Add(TextureToWorldCoords(x, y));
				} else if(noi > 0.2f) {
					// grass
					//color = new Color(0, noi, 0, 0.8f);
				}
				tex.SetPixel(x, y, color);
			}
		}
		tex.filterMode = FilterMode.Trilinear;
		tex.Apply();
		transform.renderer.sharedMaterials[0].mainTexture = tex;
	}

	void GenerateWater(Transform ground) {
		water = new float[TextureWidth, TextureHeight];
		waterCoords = new List<Vector2>();
		waterBeingCollectedAt = new List<Vector2>();
		worldToTexture = new Dictionary<Vector2,Vector2>();
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
					waterCoords.Add(TextureToWorldCoords(x, y));
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

	Dictionary <Vector2, Vector2> worldToTexture = new Dictionary<Vector2,Vector2>();

	public bool WorldCoordsToTextureCoords(Vector2 coord, out int x, out int y) {

		x = 0;
		y = 0;

		if(worldToTexture.ContainsKey(coord)) {
			var v1 = worldToTexture[coord];
			x = (int) v1.x;
			y = (int) v1.y;
		} else {
			Debug.LogError("Cannot find world to tex coords");
			return false;
		}

		return true;
	}

	public Vector2 TextureToWorldCoords(int x, int y) {
		float percentX = x * 1.0f / TextureWidth * 1.0f;
		float percentZ = y * 1.0f / TextureHeight * 1.0f;
		
		// zero is in the center of the map, so we need to subtract half of the % number with the width
		float xx = (CurrentSize.x * percentX) - (CurrentSize.x / 2.0f);
		float zz = (CurrentSize.z * percentZ) - (CurrentSize.z / 2.0f);

		xx *= -1;
		zz *= -1;

		var world = new Vector2(xx, zz);
		if(!worldToTexture.ContainsKey(world)) {
			worldToTexture.Add(world, new Vector2(x, y));
		}
		return world;
	
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

	float nextUpdateTime = 0.0f;
	float period = 5.0f;
	// Update is called once per frame
	void Update () {
		if(Time.time > nextUpdateTime) {
			nextUpdateTime += period;
			UpdateWaterTexture();

		}
	}

	public void NolongerCollectingWaterAt(Vector2 position) {
		if(waterBeingCollectedAt != null) {
			waterBeingCollectedAt.RemoveAll(o => o.Equals(position));
		}
	}

	public void CollectWaterAt(Vector2 position) {
		int x = 0;
		int y = 0;
		if(WorldCoordsToTextureCoords(position, out x, out y)) {
			// just collecting all water now.. it's going to be really fast but no biggie
			for(var xx = x - 5; xx < x + 5; xx++) {
				for(var yy = y - 5; yy < y + 5; yy++) {
					if(xx >= 0 && xx < TextureWidth && yy >=0 && yy < TextureHeight) {
						water[xx, yy] = 0;
					}
				}
			}
		} else {
			Debug.Log("Cannot find world to tex..");
		}
	}

	public bool FindClosestWater(Vector2 from, out Vector2 to, float avoidRadius) {
		to = Vector2.zero;
		if(waterCoords.Count == 0) {
			return false;
		}

		var index = 0;
		var dist = float.MaxValue;
		for(var x = 0; x < waterCoords.Count; x++) {
			var canMineThisPoint = true;
			// we try to avoid mining right next to some other collector
			// when there are very few spots left, they will still clump up
			// however, which is probably desireded.
			foreach(var t in waterBeingCollectedAt) {
				if(Vector2.Distance(t, waterCoords[x]) < avoidRadius) {
					canMineThisPoint = false;
					break;
				}
			}

			if(canMineThisPoint) {
				var dd = Vector2.Distance(waterCoords[x], from);
				if(dd < dist) {
					dist = dd;
					index = x;
				}
			}
		}

		to = waterCoords[index];
		if(!waterBeingCollectedAt.Any(o => o.Equals(waterCoords[index]))) {
			waterBeingCollectedAt.Add(waterCoords[index]);
		}
		return true;
	}

	public Vector3 FindClosestOre(Vector3 from) {
		if(oreCoords.Count == 0) {
			return Vector2.zero;
		}
		var ret = oreCoords.OrderBy(o => Vector2.Distance(o, from)).First();
		return ret;
	}


}
