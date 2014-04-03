using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Structures;

public class BaseLayout : MonoBehaviour {

	public enum LayoutType {
		SmallPlanet = 0,
		MediumPlanet = 1,
		LargePlanet = 2,
		SmallMinerShip = 3,
		MediumMinerShip = 4,
		LargeMinerShip = 5,
		SmallSatelite = 6,
		MediumSatelite = 7,
		LargeSatelite = 8,
		SmallDestroyer = 9,
		MediumDestroyer = 10,
		LargeDestroyer = 11,
		SmallHome = 12,
		MediumHome = 13,
		LargeHome = 14
	}

	Dictionary<Vector2,Structure> structures = new Dictionary<Vector2,Structure>();
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private bool CanPlaceStructure(int x, int z, Structure.StructureType type, Transform prefab) {
		var v = new Vector2(x,z);

		// can't place a structure on an existing structure
		if(type != Structure.StructureType.None && structures.ContainsKey(v)) {
			return false;						
		}

		var stru = prefab.GetComponent<Structure>();
		if(stru.GetSizeX() > 1 || stru.GetSizeZ() > 1) {
			for(var xx = x - stru.GetSizeX() + 1; xx < stru.GetSizeX() + x; xx++) {
				for(var zz = z - stru.GetSizeZ() + 1; zz < stru.GetSizeZ() + z; zz++) {
					var test = new Vector2(xx,zz);
					if(structures.ContainsKey(test)) {
						return false;
					}
				}
			}
		}


		return true;
	}

	private void Occupy(int x, int z, Structure structure, Transform prefab, Transform nullStructurePrefab) {
		var point = new Vector2(x, z);
		if(structure.SType == Structure.StructureType.Wall) {
			structures.Add(point, structure);
		}
		var stru = prefab.GetComponent<Structure>();

		if(stru.GetSizeX() > 1 || stru.GetSizeZ() > 1) {
			for(var xx = x - stru.GetSizeX() + 1; xx < stru.GetSizeX() + x; xx++) {
				for(var zz=  z - stru.GetSizeZ() + 1; zz < stru.GetSizeZ() + z; zz++) {
					var test = new Vector2(xx,zz);
					//Debug.Log("xx:" + xx + " zz:" + zz + " cx:" + Collector.SizeX + " cz:" + Collector.SizeY);
					if(xx == x && zz == z) {
						structures.Add(point, structure);
					} else {
						var position = new Vector3(xx, 0, zz);
						var t = (Transform) Instantiate(nullStructurePrefab, position, Quaternion.identity);
						t.GetComponent<NullStructure>().BelongsTo = point;
						structures.Add(test, t.GetComponent<Structure>());
					}
				}
			}
		}
	}

	public void SetStructureAt(int x, int z, Structure.StructureType type, 
	                           Transform prefab,
	                           Transform nullStructurePrefab) {

		if(!CanPlaceStructure(x, z, type, prefab)) {
			return;
		}
		var v = new Vector2(x,z);

		if(structures.ContainsKey(v) && type == Structure.StructureType.None) {
			var tt = structures[v];
			if(tt.gameObject != null) {
				GameObject.Destroy(tt.gameObject);
			}
			var remove = new List<Vector2>();
			// find if any object belongs to this.. and remove those..
			foreach(var key in structures.Keys) {
				NullStructure nls = structures[key] as NullStructure;
				if(nls != null && nls.BelongsTo.Equals(v)) {
					remove.Add(key);
				}
			}

			foreach(var dl in remove) {
				var rr = structures[dl];
				if(rr != null) {
					if(rr.gameObject != null) {
						GameObject.Destroy(rr.gameObject);
					}
					GameObject.Destroy(rr);
				}
				structures.Remove(dl);
			}
			GameObject.Destroy(tt);
			structures.Remove(v);
			// need to also redraw map if a wall was removed..
			return;
		}

		var position = new Vector3(x, 0, z);
		Transform t = (Transform) Instantiate(prefab, position, Quaternion.identity);

		var structure = t.GetComponent<Structure>();
		structure.Awake();
		Occupy(x, z, structure, prefab, nullStructurePrefab);

		if(structure.SType == Structure.StructureType.Wall) {
			//.. if there is a wall
			var vectorNorth = new Vector2(x + 1, z);
			if(structures.ContainsKey(vectorNorth) && structures[vectorNorth].SType == Structure.StructureType.Wall) {
				structure.gameObject.transform.Find("Level1").transform.Find("SingleWall").gameObject.SetActive(false);
				structure.gameObject.transform.Find("Level1").transform.Find("NSWall").gameObject.SetActive(true);
				structures[vectorNorth].gameObject.transform.Find("Level1").transform.Find("SingleWall").gameObject.SetActive(false);
				structures[vectorNorth].gameObject.transform.Find("Level1").transform.Find("NSWall").gameObject.SetActive(true);
			}
			var vectorSouth = new Vector2(x - 1, z);
			if(structures.ContainsKey(vectorSouth) && structures[vectorSouth].SType == Structure.StructureType.Wall) {
				structure.gameObject.transform.Find("Level1").transform.Find("SingleWall").gameObject.SetActive(false);
				structure.gameObject.transform.Find("Level1").transform.Find("NSWall").gameObject.SetActive(true);
				structures[vectorSouth].gameObject.transform.Find("Level1").transform.Find("SingleWall").gameObject.SetActive(false);
				structures[vectorSouth].gameObject.transform.Find("Level1").transform.Find("NSWall").gameObject.SetActive(true);
			}
			var vectorEast = new Vector2(x, z + 1);
			if(structures.ContainsKey(vectorEast) && structures[vectorEast].SType == Structure.StructureType.Wall) {
				structure.gameObject.transform.Find("Level1").transform.Find("SingleWall").gameObject.SetActive(false);
				structure.gameObject.transform.Find("Level1").transform.Find("EWWall").gameObject.SetActive(true);
				structures[vectorEast].gameObject.transform.Find("Level1").transform.Find("SingleWall").gameObject.SetActive(false);
				structures[vectorEast].gameObject.transform.Find("Level1").transform.Find("EWWall").gameObject.SetActive(true);
			}
			var vectorWest = new Vector2(x, z - 1);
			if(structures.ContainsKey(vectorWest) && structures[vectorWest].SType == Structure.StructureType.Wall) {
				structure.gameObject.transform.Find("Level1").transform.Find("SingleWall").gameObject.SetActive(false);
				structure.gameObject.transform.Find("Level1").transform.Find("EWWall").gameObject.SetActive(true);
				structures[vectorWest].gameObject.transform.Find("Level1").transform.Find("SingleWall").gameObject.SetActive(false);
				structures[vectorWest].gameObject.transform.Find("Level1").transform.Find("EWWall").gameObject.SetActive(true);
			}
		}
	}
}
