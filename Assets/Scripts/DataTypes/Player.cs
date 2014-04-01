using System.Collections;
using System.Collections.Generic;
using Structures;

namespace DataTypes {
	public class Player {
		public int Level { get; internal set; }
		public long Power { get; internal set; }

		public List<Structure> OwnedStructures { get; internal set; }
		public Player() {
		}
	}
}