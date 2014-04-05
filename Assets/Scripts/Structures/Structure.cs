using UnityEngine;
using System.Collections;

namespace Structures {
	public abstract class Structure : MonoBehaviour {

		public abstract int GetSizeX();
		public abstract int GetSizeZ();
		public Transform IsPlacedOn { get; set; }

		public enum StructureType {
			None = 0,
			NullStructure = 1,
			Wall = 2,
			Collector = 3,
			Extractor = 4,
			Turret = 5,
			Mortar = 6,
			AntiAirGun = 7,
			Base = 8

		}

		public bool CanContainOtherStructures { get; protected set; }
		public StructureType SType { get; protected set; }

		protected int level = 1;
		public int GetLevel() {
			return level;
		}

		protected int hitPoints;
		public int GetHitPoints() {
			return hitPoints;
		}

		protected bool isAwake = false;
		public void SetAlive() {
			isAwake = true;
		}

		public void SleepMe() {
			isAwake = false;
		}


		protected abstract void XStart();
		protected abstract void XUpdate();
		void Start() {
			IsPlacedOn = GameObject.Find("PlanetSurface").transform;
			if(IsPlacedOn == null) {
				Debug.LogError("Unable to find object named 'PlanetSurface'");
			}
			
			XStart();
		}

		void Update() {
			XUpdate();
		}
	}
}