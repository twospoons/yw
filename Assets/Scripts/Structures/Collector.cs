using UnityEngine;
using System.Collections;

namespace Structures {
	public class Collector : Structure {

		private static readonly int SizeX = 2;
		private static readonly int SizeZ = 2;

		private float turnspeed = 1.0f;
		private float moveSpeed = 5.0f;
		private GameObject target; 
		private Vector3 rotateToTarget;
		private Quaternion rotateTo;
		private bool foundResources = false;

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
				var found = terra.FindClosestWater(new Vector2(transform.position.x, transform.position.z), out water);

				if(found) {
					foundResources = true;
					if(target != null) {
						GameObject.Destroy(target);
					}
					target = new GameObject();
					target.transform.position = new Vector3(water.x, 0, water.y);
					rotateToTarget = target.transform.forward * 1000;
					rotateTo = Quaternion.LookRotation(target.transform.position - transform.position);
				}
			}
			if(foundResources) {
				transform.rotation = Quaternion.RotateTowards(transform.rotation, rotateTo, turnspeed);
				if(transform.rotation == rotateTo) {
					transform.position = Vector3.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime);
				}
			}
		}
	}
}
