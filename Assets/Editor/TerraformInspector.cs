using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Terraform))]
public class TerraformInspector : Editor {
	public override void OnInspectorGUI() {
		DrawDefaultInspector();

		if(GUILayout.Button("Regenerate")) {
			var t = (Terraform) target;
			t.BuildTerrain();
		}
	}	
}
