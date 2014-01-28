using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	float initialZoom = 15.0f;
	float newZoom = 15.0f;
	float zoomTimePassed = 0.0f;
	float totalZoomTime = 0.25f;
	bool zooming = false;
	Structure.StructureType currentlyBuilding;

	public Transform WallPrefab;
	public Transform CollectorPrefab;

	Transform pointerCube;
	// Use this for initialization
	void Start () {
		currentlyBuilding = Structure.StructureType.Wall;
	}
	
	// Update is called once per frame
	void Update () {
		zoomTimePassed += Time.deltaTime;
		float t = zoomTimePassed / totalZoomTime;	
		float to = Mathf.SmoothStep(initialZoom, newZoom, t);
		if(to != newZoom) {
			zooming = true;
			camera.orthographicSize = to;
		} else {
			zooming = false;
			initialZoom = newZoom;
		}

		if(!zooming) {
			if(Input.GetKeyUp(KeyCode.Q)) {
				zoomTimePassed = 0.0f;
				newZoom += 2.0f;
			}
			if(Input.GetKeyUp(KeyCode.W)) {
				zoomTimePassed = 0.0f;
				newZoom -= 2.0f;
			}
			if(Input.GetKey(KeyCode.LeftArrow)) {
				camera.transform.position = new Vector3(
					camera.transform.position.x - 0.1f, 
					camera.transform.position.y, 
					camera.transform.position.z - 0.1f);
			}
			if(Input.GetKey(KeyCode.RightArrow)) {
				camera.transform.position = new Vector3(
					camera.transform.position.x + 0.1f, 
					camera.transform.position.y, 
					camera.transform.position.z + 0.1f);
			}
			if(Input.GetKey(KeyCode.UpArrow)) {
				camera.transform.position = new Vector3(
					camera.transform.position.x - 0.1f, 
					camera.transform.position.y, 
					camera.transform.position.z + 0.1f);
			}
			if(Input.GetKey(KeyCode.DownArrow)) {
				camera.transform.position = new Vector3(
					camera.transform.position.x + 0.1f, 
					camera.transform.position.y, 
					camera.transform.position.z - 0.1f);
			}
			if(Input.GetKeyUp(KeyCode.Alpha1)) {
				if(pointerCube.gameObject != null) {
					GameObject.Destroy(pointerCube.gameObject);
				}
				currentlyBuilding = Structure.StructureType.Wall;
			}
			if(Input.GetKeyUp(KeyCode.Alpha2)) {
				if(pointerCube.gameObject != null) {
					GameObject.Destroy(pointerCube.gameObject);
				}
				currentlyBuilding = Structure.StructureType.Collector;
			}
		}

		CheckPoint();
	}

	Plane plane = new Plane(Vector3.up, Vector3.zero);
	
	private void CheckPoint() {
		var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		var ent = 100.0f;
		if (plane.Raycast(ray, out ent)) {
			var hitPoint = ray.GetPoint(ent);
			var pos = new Vector3(
				(int) hitPoint.x,
				0,
				(int) hitPoint.z);

			if(pointerCube == null || pointerCube.gameObject == null) {
				if(currentlyBuilding == Structure.StructureType.Wall) {
					pointerCube = (Transform) Instantiate(WallPrefab, pos, Quaternion.identity);
				} else if(currentlyBuilding == Structure.StructureType.Collector) {
					pointerCube = (Transform) Instantiate(CollectorPrefab, pos, Quaternion.identity);
				}
			}
			//Debug.Log("Plane Raycast hit at distance: " + ent);
			pointerCube.transform.position = pos;
			//var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
			//go.transform.position = hitPoint;
			//Debug.DrawRay (ray.origin, ray.direction * ent, Color.green);
		}
		else {
			//Debug.DrawRay (ray.origin, ray.direction * 10, Color.red);
		}

		if(Input.GetMouseButtonDown(0)) {
			Transform t = null;
			if(currentlyBuilding == Structure.StructureType.Wall) {
				t = (Transform) Instantiate(WallPrefab, pointerCube.transform.position, Quaternion.identity);
			} else if(currentlyBuilding == Structure.StructureType.Collector) {
				t = (Transform) Instantiate(CollectorPrefab, pointerCube.transform.position, Quaternion.identity);
			}

			if(t != null) {
				this.GetComponent<BaseLayout>().SetStructureAt(
					(int) pointerCube.transform.position.x,
					(int) pointerCube.transform.position.y,
					t.GetComponent<Structure>());


			}
		}
		

	}
}
