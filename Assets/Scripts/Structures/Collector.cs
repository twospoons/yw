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
				var water = terra.FindClosestWater(new Vector2(transform.position.x, transform.position.z));

				if(water != Vector2.zero) {
					foundResources = true;
					if(target != null) {
						GameObject.Destroy(target);
					}
					target = new GameObject();
					// translate Vector2 (which is a tex coord) into world coords
					//var percentX = (water.x / terra.TextureWidth) * 1.0f;
					//var percentZ = (water.y / terra.TextureWidth) * 1.0f;

					// zero is in the center of the map, so we need to subtract half of the % number with the width
					//var x = (terra.CurrentSize.x * percentX) - (terra.CurrentSize.x / 2.0f);
					//var z = (terra.CurrentSize.z * percentZ) - (terra.CurrentSize.z / 2.0f);

					target.transform.position = new Vector3(water.x * -1, 0, water.y * -1);
					rotateToTarget = target.transform.forward * 1000;
					rotateTo = Quaternion.LookRotation(target.transform.position - transform.position);
				}
			}
			if(foundResources) {
				transform.rotation = Quaternion.RotateTowards(transform.rotation, rotateTo, turnspeed);
				transform.position = Vector3.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime);
			}
		}
	}
}
