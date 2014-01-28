using UnityEngine;
using System.Collections;

public abstract class Structure : MonoBehaviour {
	
	protected int level;
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
