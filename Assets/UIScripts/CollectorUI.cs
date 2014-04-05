using UnityEngine;
using System.Collections;

public class CollectorUI : MonoBehaviour {
	private Rect windowRect = new Rect(10, 230, 400, 440);
	private bool renderUI = false;
	private CameraControl ccScript;

	public GUIStyle Style;
	public Transform PlanetSurface;
	private Terraform terraform;
	
	private void OnGUI() {
		if(Input.GetKeyUp(KeyCode.Alpha0) && Input.GetKey(KeyCode.LeftShift)) {
			renderUI = false;
		} else if(Input.GetKeyUp(KeyCode.Alpha0) && !Input.GetKey(KeyCode.LeftShift)) {
			renderUI = true;
		}
		
		if(renderUI) {
			windowRect = GUI.Window(1, windowRect, WindowFunc, "(0) Collector");
		}
	}
	
	private void WindowFunc(int id) {
		
		if(GUI.Button(new Rect(5, 5, 15, 15), "_")) {
			if(windowRect.width == 26) {
				windowRect.width = 260;
				windowRect.height = 210;
			} else {
				windowRect.width = 26;
				windowRect.height = 26;
			}
		}
		var coords = terraform.GetWaterCoords();
		if(coords != null) {
			var xx = 35;
			var cursor = new Vector2(ccScript.CursorPoint.x, ccScript.CursorPoint.z);
			foreach(var t in coords) {
				GUI.Label(new Rect(10, xx, 100, 35), t.ToString() + " c: " + cursor + " d: " + Vector2.Distance(t, cursor), Style);
				xx += 10;
			}
		}
	}
	
	void Start() {
		ccScript = camera.GetComponent<CameraControl>();
		terraform = PlanetSurface.GetComponent<Terraform>();
	}
	
	void Update() {
	}
	
}
