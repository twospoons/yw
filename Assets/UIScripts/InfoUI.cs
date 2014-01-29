using UnityEngine;
using System.Collections;

public class InfoUI : MonoBehaviour {
	private Rect windowRect = new Rect(10, 10, 400, 210);
	private bool renderUI = true;
	private CameraControl ccScript;

	private void OnGUI() {
		if(Input.GetKeyUp(KeyCode.Alpha4) && Input.GetKey(KeyCode.LeftShift)) {
			renderUI = false;
		} else if(Input.GetKeyUp(KeyCode.Alpha4) && !Input.GetKey(KeyCode.LeftShift)) {
			renderUI = true;
		}
		
		if(renderUI) {
			windowRect = GUI.Window(4, windowRect, WindowFunc, "(4) Device Info");
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
		
		GUI.Label(new Rect(10, 35, 100, 35), "DM:");
		GUI.Label(new Rect(10, 50, 100, 50), "DN:");		
		GUI.Label(new Rect(10, 65, 100, 65), "DI:");		
		GUI.Label(new Rect(10, 80, 100, 80), "MEM:");		
		GUI.Label(new Rect(10, 95, 100, 95), "OS:");
		GUI.Label(new Rect(10, 110, 100, 110), "GF:");
		GUI.Label(new Rect(10, 125, 100, 125), "GFM:");
		GUI.Label(new Rect(10, 140, 100, 140), "X:" + ccScript.CursorPoint.x + " Z:" + ccScript.CursorPoint.z);

		GUI.Label(new Rect(50, 35, 400, 35), SystemInfo.deviceModel);
		GUI.Label(new Rect(50, 50, 400, 50), SystemInfo.deviceName);
		GUI.Label(new Rect(50, 65, 400, 65), SystemInfo.deviceUniqueIdentifier);
		GUI.Label(new Rect(50, 80, 400, 80), SystemInfo.systemMemorySize.ToString() + " MB");
		GUI.Label(new Rect(50, 95, 400, 95), SystemInfo.operatingSystem);
		GUI.Label(new Rect(50, 110, 400, 110), SystemInfo.graphicsDeviceName);
		GUI.Label(new Rect(50, 125, 400, 125),SystemInfo.graphicsMemorySize.ToString() + " KB");
	}
	
	void Start() {
		ccScript = camera.GetComponent<CameraControl>();
	}
	
	void Update() {
	}
	
}
