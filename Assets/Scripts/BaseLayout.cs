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

	public void SetStructureAt(int x, int y, Structure structure) {
		var v = new Vector2(x,y);
		if(!structures.ContainsKey(v)) {
			structures.Add(v, structure);
		} else {
			structures[v] = structure;
		}
	}
}
