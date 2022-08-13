using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent (typeof(MeshRenderer))]
public class MyCube : MonoBehaviour
{
	CoordinateSystem coordinateSystem;
	[SerializeField] MyVertex _myVertexPrefab;
	MyVertex[] myVertex = new MyVertex[8];

	bool initialized = false;

    public void InitializeCube()
	{
		coordinateSystem = FindObjectOfType<CoordinateSystem>();
		if (initialized == false)
		{
			initialized = true;
			for (int i = 0; i < 8; i++)
			{
				myVertex[i] = Instantiate(_myVertexPrefab, transform);
			}

			myVertex[0].InitializeVertex(EVertexType.None, new Vector3(0, 0, 0));
			myVertex[1].InitializeVertex(EVertexType.None, new Vector3(1, 0, 0));
			myVertex[2].InitializeVertex(EVertexType.None, new Vector3(1, 1, 0));
			myVertex[3].InitializeVertex(EVertexType.None, new Vector3(0, 1, 0));
			myVertex[4].InitializeVertex(EVertexType.None, new Vector3(0, 1, 1));
			myVertex[5].InitializeVertex(EVertexType.None, new Vector3(1, 1, 1));
			myVertex[6].InitializeVertex(EVertexType.None, new Vector3(1, 0, 1));
			myVertex[7].InitializeVertex(EVertexType.None, new Vector3(0, 0, 1));
		}


		Mesh mesh = GetComponent<MeshFilter>().mesh;
		mesh.Clear();
		Vector3[] verts = GetComponent<MeshFilter>().mesh.vertices;
		verts = new Vector3[]
			{
					 myVertex[0].transform.position,
					 myVertex[1].transform.position,
					 myVertex[2].transform.position,
					 myVertex[3].transform.position,
					 myVertex[4].transform.position,
					 myVertex[5].transform.position,
					 myVertex[6].transform.position,
					 myVertex[7].transform.position,
			};
		mesh.vertices = verts;
		mesh.triangles = new int[]{
			0, 2, 1, //face front
			0, 3, 2,
			2, 3, 4, //face top
			2, 4, 5,
			1, 2, 5, //face right
			1, 5, 6,
			0, 7, 4, //face left
			0, 4, 3,
			5, 4, 7, //face back
			5, 7, 6,
			0, 6, 7, //face bottom
			0, 1, 6
		};
		mesh.Optimize();
		mesh.RecalculateNormals();
	}
}
