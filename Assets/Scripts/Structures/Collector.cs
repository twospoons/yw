using UnityEngine;
using System.Collections;

namespace Structures {
	public class Collector : Structure {

		private static readonly int SizeX = 2;
		private static readonly int SizeZ = 2;

		private float turnspeed = 1.0f;
		private float moveSpeed = 1.0f;
		private GameObject target; 
		private Vector3 rotateToTarget;
		private Quaternion rotateTo;

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
			target = new GameObject();
			target.transform.position = new Vector3(10, 0, 10);
			rotateToTarget = target.transform.forward * 1000;
			rotateTo = Quaternion.LookRotation(target.transform.position - transform.position);
		}

		protected override void XUpdate() {
			if(isAwake) {
				SearchForResources();
			}
		}

		private void SearchForResources() {

			transform.rotation = Quaternion.RotateTowards(transform.rotation, rotateTo, turnspeed);
			transform.position = Vector3.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime);
			//transform.RotateAround(new Vector3(10,0,10), Vector3.left, 0.34f);

		}
	}
}
