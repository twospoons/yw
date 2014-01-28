using UnityEngine;
using System.Collections;

public class Collector : Structure {

	protected override void XStart() {
		this.sizeX = 2;
		this.sizeY = 2;
		this.level = 1;
		this.hitPoints = 10;
	}

	protected override void XUpdate() {
	
	}
}
