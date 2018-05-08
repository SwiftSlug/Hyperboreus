using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSpawnerController : MonoBehaviour {

    public Transform GridCollider_Obj;
    public GameObject GridCollider;
    public Transform GridCollTran;
    public Vector3 currentPosition;

    public int gridSizeX;
    public int gridSizeZ;

	// Use this for initialization
	void Start ()
    {
        currentPosition.x = transform.position.x;
        currentPosition.y = transform.position.y;
        currentPosition.z = transform.position.z;

        currentPosition.x = currentPosition.x - gridSizeX;
        currentPosition.z = currentPosition.z - gridSizeZ;


        for (int i = -gridSizeZ; i < gridSizeZ; i++)
        {
            for (int j = -gridSizeX; j < gridSizeX; j++)
            {
                GridCollider_Obj = (Transform)Instantiate(GridCollider_Obj, currentPosition, GridCollider_Obj.rotation);

                currentPosition.z = currentPosition.z + 1;
                if (currentPosition.z == gridSizeZ)
                {
                    currentPosition.z = currentPosition.z - (gridSizeZ * 2);
                }
            }

            GridCollider_Obj = (Transform)Instantiate(GridCollider_Obj, currentPosition, GridCollider_Obj.rotation);
            currentPosition.x = currentPosition.x + 1;
        }
	}
	
    /*
	// Update is called once per frame
	void Update ()
    {
		
	}
    */
}
