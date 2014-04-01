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
	public Vector3 SmallSize;
	public Vector3 MediumSize;
	public Vector3 LargeSize;

	public Material GroundMaterial;

	GameObject ground;
	GameObject pointLight;
	// Use this for initialization
	void Start () {
		// create the world
		ground = GameObject.CreatePrimitive(PrimitiveType.Cube);
		var size = MediumSize;
		if(Size == TerraformSize.Small) {
			size = SmallSize;
		} else if(Size == TerraformSize.Medium) {
			size = MediumSize;
		} else if(Size == TerraformSize.Large) {
			size = LargeSize;
		}

		ground.transform.localScale = size;
		ground.transform.position = Position;
		GroundMaterial.SetInt("Flow", 1000);
		ground.renderer.material = GroundMaterial;
		Debug.Log (ground.renderer.material.GetFloat("Flow"));
		//ground.renderer.material.SetFloat("Zoom", 1.0f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
