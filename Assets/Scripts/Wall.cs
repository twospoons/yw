﻿using UnityEngine;
using System.Collections;

public class Wall : Structure {

	public static readonly int SizeX = 1;
	public static readonly int SizeY = 1;

	public Wall() {
		this.SType = StructureType.Wall;
		this.level = 1;
		this.hitPoints = 10;
	}

	protected override void XStart() {
	}

	protected override void XUpdate() {
	
	}
}
