using UnityEngine;

public class PlayerTrail : MonoBehaviour
{
	public Shader TrailShader;
	public GameObject Terrain;
	public Transform Player;
	[Range(0, 500)]
	public float trailSize = 1.0f;
	[Range(0.01f, 5)]
	public float trailStrength = 0.01f;

	private Material SnowMaterial, TrailMaterial;
	private RaycastHit Hit;
	private RenderTexture DisplacementMap;
	private int layerMask;

	void Start ()
	{
		layerMask = LayerMask.GetMask("Ground");
		TrailMaterial = new Material(TrailShader);

		Terrain = GameObject.FindGameObjectWithTag("TitleCamera");
		if (Terrain != null)
		{
			SnowMaterial = Terrain.GetComponent<MeshRenderer>().material;
		}
		else
		{
			Debug.LogWarning("Terrain not found.");
		}

		if(SnowMaterial != null)
		{
			SnowMaterial.SetTexture("_DispTex", DisplacementMap = new RenderTexture(1024, 1024, 0, RenderTextureFormat.ARGBFloat));
		}
		else
		{
			Debug.LogWarning("SnowMaterial not found.");
		}
	}

	void Update ()
	{
		if (Physics.Raycast(Player.position, -Vector3.up, out Hit, 3.0f, layerMask))
		{
			TrailMaterial.SetVector("_Coordinate", new Vector4(Hit.textureCoord.x, Hit.textureCoord.y, 0, 0));
			TrailMaterial.SetFloat("_Size", trailSize);
			TrailMaterial.SetFloat("_Strength", trailStrength);

			RenderTexture temp = RenderTexture.GetTemporary(DisplacementMap.width, DisplacementMap.height, 0, RenderTextureFormat.ARGBFloat);

			Graphics.Blit(DisplacementMap, temp);
			Graphics.Blit(temp, DisplacementMap, TrailMaterial);

			RenderTexture.ReleaseTemporary(temp);
		}
	}
}
