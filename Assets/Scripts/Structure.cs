using UnityEngine;
using System.Collections;

public abstract class Structure : MonoBehaviour {

	public abstract int GetSizeX();
	public abstract int GetSizeZ();

	public enum StructureType {
		None = 0,
		NullStructure = 1,
		Wall = 2,
		Collector = 3,
		Extractor = 4,
		Turret = 5,
		Mortar = 6,
		AntiAirGun = 7,
		Base = 8

	}

	public StructureType SType { get; protected set; }

	protected int level = 1;
	public int GetLevel() {
		return level;
	}

	protected int hitPoints;
	public int GetHitPoints() {
		return hitPoints;
	}

	protected abstract void XStart();
	protected abstract void XUpdate();
	void Start() {
		XStart();
	}

	void Update() {
		XUpdate();
	}
}
