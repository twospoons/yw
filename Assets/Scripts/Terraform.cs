using UnityEngine;
using System.Collections;

public class Terraform : MonoBehaviour {

	public enum TerraformSize {
		Small,
		Medium,
		Large
	}

	public TerraformSize Size;
	public Vector3 Position;
	GameObject ground;
	GameObject pointLight;
	// Use this for initialization
	void Start () {
		// create the world
		ground = GameObject.CreatePrimitive(PrimitiveType.Cube);
		var size = new Vector3(100, 0.5f, 100);
		if(Size == TerraformSize.Small) {
			size = new Vector3(50, 0.5f, 50);
		} else if(Size == TerraformSize.Medium) {
			size = new Vector3(100, 0.5f, 100);
		} else if(Size == TerraformSize.Large) {
			size = new Vector3(150, 0.5f, 150);
		}

		ground.transform.localScale = size;
		ground.transform.position = Position;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
