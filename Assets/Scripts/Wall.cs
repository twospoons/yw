﻿using UnityEngine;
using System.Collections;

public class Wall : Structure {
	
	protected override void XStart() {
		this.sizeX = 1;
		this.sizeY = 1;
		this.level = 1;
		this.hitPoints = 10;
	}

	protected override void XUpdate() {
	
	}
}
