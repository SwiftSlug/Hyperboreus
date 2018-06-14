using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailTest : MonoBehaviour
{
	public Camera trailCamera;
	public Shader trailShader;

	[Range(1, 500)]
	public float trailSize = 1.0f;
	[Range(0.01f, 1)]
	public float trailStrength = 0.01f;

	private RenderTexture displacementMap;
	private Material snowMaterial, trailMaterial;

	private RaycastHit hit;



	void Start ()
	{
		trailMaterial = new Material(trailShader);
		trailMaterial.SetVector("_Color", Color.red);

		snowMaterial = GetComponent<MeshRenderer>().material;

		snowMaterial.SetTexture("_DispTex", displacementMap = new RenderTexture(1024, 1024, 0, RenderTextureFormat.ARGBFloat));
	}
	
	void FixedUpdate ()
	{
		if (Input.GetKey(KeyCode.Mouse0))
		{
			if (Physics.Raycast(trailCamera.ScreenPointToRay(Input.mousePosition), out hit))
			{
				trailMaterial.SetVector("_Coordinate", new Vector4(hit.textureCoord.x, hit.textureCoord.y, 0, 0));
				trailMaterial.SetFloat("_Size", trailSize);
				trailMaterial.SetFloat("_Strength", trailStrength);

				RenderTexture temp = RenderTexture.GetTemporary(displacementMap.width, displacementMap.height, 0, RenderTextureFormat.ARGBFloat);

				Graphics.Blit(displacementMap, temp);
				Graphics.Blit(temp, displacementMap, trailMaterial);

				RenderTexture.ReleaseTemporary(temp);
			}
		}
	}

	private void OnGUI()
	{
		GUI.DrawTexture(new Rect(0, 0, 256, 256), displacementMap, ScaleMode.ScaleToFit, false, 1);
	}
}
