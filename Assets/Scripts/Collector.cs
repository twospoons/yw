using UnityEngine;
using System.Collections;

public class Collector : Structure {

	public static readonly int SizeX = 2;
	public static readonly int SizeY = 2;

	public Collector() {
		this.SType = StructureType.Collector;
		this.level = 1;
		this.hitPoints = 10;
	}

	protected override void XStart() {
	}

	protected override void XUpdate() {
	
	}
}
