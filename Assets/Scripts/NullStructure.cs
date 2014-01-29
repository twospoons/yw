using UnityEngine;
using System.Collections;

public class NullStructure : Structure {
	public static readonly int SizeX = 1;
	public static readonly int SizeY = 1;


	public Structure BelongsTo { get; set; }

	public NullStructure() {
		this.SType = StructureType.NullStructure;
		this.level = 1;
		this.hitPoints = 10;
	}

	protected override void XStart() {
	}	

	protected override void XUpdate() {
		
	}
}
