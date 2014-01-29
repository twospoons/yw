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

	public void SetStructureAt(int x, int z, Structure structure) {

		var v = new Vector2(x,z);
		if(!structures.ContainsKey(v)) {
			structures.Add(v, structure);
		} else {
			structures[v] = structure;
		}

		if(structure.SType == Structure.StructureType.Wall) {
			//.. if there is a wall
			var vectorNorth = new Vector2(x + 1, z);
			if(structures.ContainsKey(vectorNorth) && structures[vectorNorth].SType == Structure.StructureType.Wall) {
				structure.gameObject.transform.localScale = new Vector3(1,1,0.5f);
				structures[vectorNorth].gameObject.transform.localScale = new Vector3(1,1,0.5f);
			}
			var vectorSouth = new Vector2(x - 1, z);
			if(structures.ContainsKey(vectorSouth) && structures[vectorSouth].SType == Structure.StructureType.Wall) {
				structure.gameObject.transform.localScale = new Vector3(1,1,0.5f);
				structures[vectorSouth].gameObject.transform.localScale = new Vector3(1,1,0.5f);
			}
			var vectorEast = new Vector2(x, z + 1);
			if(structures.ContainsKey(vectorEast) && structures[vectorEast].SType == Structure.StructureType.Wall) {
				structure.gameObject.transform.localScale = new Vector3(0.5f,1,1);
				structures[vectorEast].gameObject.transform.localScale = new Vector3(0.5f,1,1);
			}
			var vectorWest = new Vector2(x, z - 1);
			if(structures.ContainsKey(vectorWest) && structures[vectorWest].SType == Structure.StructureType.Wall) {
				structure.gameObject.transform.localScale = new Vector3(0.5f,1,1);
				structures[vectorWest].gameObject.transform.localScale = new Vector3(0.5f,1,1);
			}
		}
	}
}
