using UnityEngine;
using System.Collections;

namespace Structures {
	public class Collector : Structure {

		private static readonly int SizeX = 2;
		private static readonly int SizeZ = 2;

		private float turnspeed = 3.0f;
		private float moveSpeed = 5.0f;
		private Quaternion rotateTo;
		private bool foundResources = false;
		private float avoidRadius = 2.0f;

		private Transform collectorModel;
		private Terraform terra;

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
			collectorModel = transform.FindChild("CollectorModel");
			if(collectorModel == null) {
				Debug.LogError("Unable to find child named 'CollectorModel'");
			}
			terra = IsPlacedOn.GetComponent<Terraform>();
			if(terra == null) {
				Debug.LogError("Unable to find terra form script");
			}
		}
		float nextUpdateTime = 0.0f;
		float period = 5.0f;

		protected override void XUpdate() {
			if(isAwake) {
				if(Time.time > nextUpdateTime) {
					nextUpdateTime += period;
					SearchForResources();
				}
				if(foundResources) {
					collectorModel.rotation = Quaternion.RotateTowards(collectorModel.rotation, rotateTo, turnspeed);
					if(collectorModel.rotation == rotateTo) {
						transform.position = Vector3.MoveTowards(transform.position, WaterTarget.transform.position, moveSpeed * Time.deltaTime);
					}
					
					if(transform.position == WaterTarget.transform.position) {
						CollectResources();
					}
				}
			}
		}

		private void SearchForResources() {
			if(!foundResources) {
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
		}

		private void CollectResources() {
			var v2 = new Vector2(WaterTarget.transform.position.x, WaterTarget.transform.position.z);
			terra.CollectWaterAt(v2);
			terra.NolongerCollectingWaterAt(v2);
			foundResources = false;
		}
	}
}
