using UnityEngine;
using System.Collections;

namespace Structures {
	public class Turret : Structure {
		
		private static readonly int SizeX = 2;
		private static readonly int SizeZ = 2;
		
		public override int GetSizeX () {
			return SizeX;
		}
		
		public override int GetSizeZ () {
			return SizeZ;
		}

		public Turret() {
			this.SType = StructureType.Turret;
			this.level = 1;
			this.hitPoints = 10;
		}
		
		protected override void XStart() {
		}
		
		protected override void XUpdate() {
			
		}
	}
}
