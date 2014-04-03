using UnityEngine;
using System.Collections;
using Structures;

public class CameraControl : MonoBehaviour {

	public Transform WallPrefab;
	public Transform CollectorPrefab;
	public Transform NullStructurePrefab;
	public Transform TurretPrefab;
	public Transform SelectorCube;

	private Transform selectedPrefab;

	public Transform Sun;

	float initialZoom = 15.0f;
	float newZoom = 15.0f;
	float zoomTimePassed = 0.0f;
	float totalZoomTime = 0.25f;
	bool zooming = false;
	Structure.StructureType currentlyBuilding;
	
	Transform pointerCube;
	// Use this for initialization
	void Start () {
		currentlyBuilding = Structure.StructureType.Wall;
	}

	void MoveSun() {
		//Sun.transform.Rotate(Vector3.right * Time.deltaTime);
		//Sun.transform.Rotate(Vector3.up * Time.deltaTime);
	}
	// Update is called once per frame
	void Update () {
		MoveSun();
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
			if(Input.GetKeyUp(KeyCode.Alpha3)) {
				if(pointerCube.gameObject != null) {
					GameObject.Destroy(pointerCube.gameObject);
				}
				currentlyBuilding = Structure.StructureType.Turret;
			}	

			if(Input.GetKeyUp (KeyCode.Z)) {
				var noise = new SimplexNoiseGenerator(new [] {1, 2, 3, 4, 5, 6, 7, 8});
				var f = noise.noise(0.1f, 0.1f, 0.2f);
				Debug.Log("Noise: " + f.ToString());
			}
		}

		CheckPoint();
	}

	Plane plane = new Plane(Vector3.up, Vector3.zero);
	public Vector3 CursorPoint { get; internal set; }

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
					selectedPrefab = WallPrefab;
				} else if(currentlyBuilding == Structure.StructureType.Collector) {
					pointerCube = (Transform) Instantiate(CollectorPrefab, pos, Quaternion.identity);
					selectedPrefab = CollectorPrefab;
				} else if(currentlyBuilding == Structure.StructureType.Turret) {
					pointerCube = (Transform) Instantiate(TurretPrefab, pos, Quaternion.identity);
					selectedPrefab = TurretPrefab;
				}
			}
			//Debug.Log("Plane Raycast hit at distance: " + ent);
			pointerCube.transform.position = pos;
			SelectorCube.transform.position = new Vector3(pos.x, -0.5f, pos.z);
			var stru = selectedPrefab.GetComponent<Structure>();
			SelectorCube.transform.localScale = new Vector3(stru.GetSizeX(), 0.1f, stru.GetSizeZ());
			
			//var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
			//go.transform.position = hitPoint;
			//Debug.DrawRay (ray.origin, ray.direction * ent, Color.green);

		}
		else {
			//Debug.DrawRay (ray.origin, ray.direction * 10, Color.red);
		}
	
		if(Input.GetMouseButton(0)) {
			this.GetComponent<BaseLayout>().SetStructureAt(
				(int) pointerCube.transform.position.x,
				(int) pointerCube.transform.position.z,
				currentlyBuilding,
				selectedPrefab,
				NullStructurePrefab);
		}
		// remove structure
		if(Input.GetMouseButtonUp(1)) {
			this.GetComponent<BaseLayout>().SetStructureAt(
				(int) pointerCube.transform.position.x,
				(int) pointerCube.transform.position.z,
				Structure.StructureType.None,
				selectedPrefab,
				NullStructurePrefab);
		}
	}
}
