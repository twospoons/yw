using UnityEngine;
using System.Collections;

namespace Structures {
	public class NullStructure : Structure {

		private static readonly int SizeX = 1;
		private static readonly int SizeZ = 1;
		
		public override int GetSizeX () {
			return SizeX;
		}
		
		public override int GetSizeZ () {
			return SizeZ;
		}

		public Vector2 BelongsTo { get; set; }

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
}
