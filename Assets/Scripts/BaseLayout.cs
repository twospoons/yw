using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

	private bool CanPlaceStructure(int x, int z, Structure.StructureType type) {
		var v = new Vector2(x,z);

		// can't place a structure on an existing structure
		if(type != Structure.StructureType.None && structures.ContainsKey(v)) {
			return false;						
		}

		if(type == Structure.StructureType.Collector) {
			for(var xx = x - Collector.SizeX + 1; xx < Collector.SizeX + x; xx++) {
				for(var zz = z - Collector.SizeY + 1; zz < Collector.SizeY + z; zz++) {
					var test = new Vector2(xx,zz);
					if(structures.ContainsKey(test)) {
						return false;
					}
				}
			}
		}


		return true;
	}

	private void Occupy(int x, int z, Structure structure, Transform nullStructurePrefab) {
		var point = new Vector2(x, z);
		if(structure.SType == Structure.StructureType.Wall) {
			Debug.Log("adding wall");
			structures.Add(point, structure);
		}
		if(structure.SType == Structure.StructureType.Collector) {
			for(var xx = x - Collector.SizeX + 1; xx < Collector.SizeX + x; xx++) {
				for(var zz=  z - Collector.SizeY + 1; zz < Collector.SizeY + z; zz++) {
					var test = new Vector2(xx,zz);
					//Debug.Log("xx:" + xx + " zz:" + zz + " cx:" + Collector.SizeX + " cz:" + Collector.SizeY);
					if(xx == x && zz == z) {
						structures.Add(point, structure);
					} else {
						var position = new Vector3(xx, 0, zz);
						var t = (Transform) Instantiate(nullStructurePrefab, position, Quaternion.identity);
						structures.Add(test, t.GetComponent<Structure>());
					}
				}
			}
		}
	}

	public void SetStructureAt(int x, int z, Structure.StructureType type, 
	                           Transform wallPrefab, 
	                           Transform collectorPrefab,
	                           Transform nullStructurePrefab) {

		if(!CanPlaceStructure(x, z, type)) {
			return;
		}
		var v = new Vector2(x,z);

		if(structures.ContainsKey(v) && type == Structure.StructureType.None) {
			var tt = structures[v];
			GameObject.Destroy(tt);
			structures.Remove(v);
			// need to also redraw map if a wall was removed..
			return;
		}

		var position = new Vector3(x, 0, z);
		Transform t = null;

		if(type == Structure.StructureType.Wall) {
			t = (Transform) Instantiate(wallPrefab, position, Quaternion.identity);
		} else if(type == Structure.StructureType.Collector) {
			t = (Transform) Instantiate(collectorPrefab, position, Quaternion.identity);
		}

		var structure = t.GetComponent<Structure>();
		Occupy(x, z, structure, nullStructurePrefab);

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
