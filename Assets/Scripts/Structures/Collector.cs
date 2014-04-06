using UnityEngine;
using System.Collections;

namespace Structures {
	public class Collector : Structure {

		private static readonly int SizeX = 2;
		private static readonly int SizeZ = 2;

		private float turnspeed = 1.0f;
		private float moveSpeed = 5.0f;
		private Quaternion rotateTo;
		private bool foundResources = false;
		private float avoidRadius = 2.0f;

		public GameObject WaterTarget;

		public override int GetSizeX () {
			return SizeX;
		}

		public override int GetSizeZ () {
			return SizeZ;
		}

		public Collector() {
			this.SType = StructureType.Collector;
			this.level = 1;
			this.hitPoints = 10;
			this.CanCollide = false;
		}

		protected override void XStart() {
			// create some random target.
		}

		protected override void XUpdate() {
			if(isAwake) {
				SearchForResources();
			}
		}

		private void SearchForResources() {
			if(!foundResources) {
				var terra = IsPlacedOn.GetComponent<Terraform>();
				var water = Vector2.zero;
				var found = terra.FindClosestWater(
					new Vector2(transform.position.x, transform.position.z), 
					out water,
					avoidRadius);

				if(found) {
					foundResources = true;
					if(WaterTarget == null) {
						WaterTarget = new GameObject();
						WaterTarget.name = "Water Target";
					}
					WaterTarget.transform.position = new Vector3(water.x, 0, water.y);
					rotateTo = Quaternion.LookRotation(WaterTarget.transform.position - transform.position);
				}
			}
			if(foundResources) {
				transform.rotation = Quaternion.RotateTowards(transform.rotation, rotateTo, turnspeed);
				if(transform.rotation == rotateTo) {
					transform.position = Vector3.MoveTowards(transform.position, WaterTarget.transform.position, moveSpeed * Time.deltaTime);
				}
			}
		}
	}
}
