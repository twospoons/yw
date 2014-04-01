using UnityEngine;
using System.Collections;

namespace Structures {
	public class Wall : Structure {

		private static readonly int SizeX = 1;
		private static readonly int SizeZ = 1;
		
		public override int GetSizeX () {
			return SizeX;
		}
		
		public override int GetSizeZ () {
			return SizeZ;
		}
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
}