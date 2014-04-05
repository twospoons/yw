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

	public Vector2 TextureToWorldCoords(int x, int y) {
		float percentX = x * 1.0f / TextureWidth * 1.0f;
		float percentZ = y * 1.0f / TextureHeight * 1.0f;
		
		// zero is in the center of the map, so we need to subtract half of the % number with the width
		float xx = (CurrentSize.x * percentX) - (CurrentSize.x / 2.0f);
		float zz = (CurrentSize.z * percentZ) - (CurrentSize.z / 2.0f);

		xx *= -1;
		zz *= -1;

		return new Vector2(xx, zz);
	
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
	private bool FindRequrse(Vector2 from, out Vector2 to, Vector3[] avoid, float avoidRadius, List<Vector2> avoidCheckedPoints) {
		var index = 0;
		var dist = float.MaxValue;
		var avoidCoords = new List<Vector2>();
		var ret = true;
		for(var x = 0; x < waterCoords.Count; x++) {
			var dd = Vector2.Distance(waterCoords[x], from);
			if(!avoidCoords.Exists(waterCoords[x])) {
				if(dd < dist) {
					dist = dd;
					index = x;
				}
			}
		}
		to = waterCoords[index];
		foreach(var t in avoid) {
			Debug.Log ("avoid...");
			var v2 = new Vector2(t.x, t.z);
			if(Vector2.Distance(to, v2) < avoidRadius) {
				avoidCoords.Add(to);
				// .. need to find another point since it's too close to other collectors
				Debug.Log("too close to another target..");
				ret = false;
			}
		}
		ret = true;
	}

	public bool FindClosestWater(Vector2 from, out Vector2 to, Vector3[] avoid, float avoidRadius) {
		to = Vector2.zero;
		if(waterCoords.Count == 0) {
			return false;
		}
		var avoidCoords = new List<Vector2>();
		while(FindRequrse(from, out to, avoid, avoidRadius, avoidCoords)) {
			if(avoidCoords.Count == waterCoords.Count) {
				Debug.Log("All water is being occupied");
				return false;
			}
		}

		return true;
		/*var index = 0;
		var dist = float.MaxValue;
		var avoidCoords = new List<Vector2>();
		for(var x = 0; x < waterCoords.Count; x++) {
			var dd = Vector2.Distance(waterCoords[x], from);
			if(dd < dist) {
				dist = dd;
				index = x;
			}
		}
		to = waterCoords[index];
		foreach(var t in avoid) {
			Debug.Log ("avoid...");
			var v2 = new Vector2(t.x, t.z);
			if(Vector2.Distance(to, v2) < avoidRadius) {
				avoidCoords.Add(to);
				// .. need to find another point since it's too close to other collectors
				Debug.Log("too close to another target..");
			}
		}*/
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
