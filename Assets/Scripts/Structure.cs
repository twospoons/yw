using UnityEngine;
using System.Collections;

public abstract class Structure : MonoBehaviour {
	public enum StructureType {
		None = 0,
		Wall = 1,
		Collector = 2,
		Extractor = 3,
		Turret = 4,
		Mortar = 5,
		AntiAirGun = 6,
		Base = 7

	}

	public StructureType SType { get; set; }

	protected int level = 1;
	public int GetLevel() {
		return level;
	}

	protected int hitPoints;
	public int GetHitPoints() {
		return hitPoints;
	}

	protected int sizeX;
	public int GetSizeX() {
		return sizeX;
	}

	protected int sizeY;
	public int GetSizeY() {
		return sizeY;
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
