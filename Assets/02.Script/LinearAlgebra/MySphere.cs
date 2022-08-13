using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MySphere : MonoBehaviour
{
	Mesh mesh;
	Vector3[] vertices;
	Vector2[] uv;
	int[] triangles;
	Vector3[] normals;
	public MyVertex vertexPrefab;
	MyVertex[] vertexes;

	MeshFilter meshFilter;
	MeshRenderer meshRenderer;

	bool initialized = false;

	public void InitializeSphere()
	{
		int m = 10; //row 
		int n = 15; //col 
		float width = 8;
		float height = 6;
		if (initialized == false)
		{
			meshFilter = gameObject.GetComponent<MeshFilter>();
			meshRenderer = gameObject.GetComponent<MeshRenderer>();
			mesh = new Mesh();

			vertices = new Vector3[(m + 1) * (n + 1)];//the positions of vertices 
			vertexes = new MyVertex[(m + 1) * (n + 1)];
			uv = new Vector2[(m + 1) * (n + 1)];
			normals = new Vector3[(m + 1) * (n + 1)];
			triangles = new int[6 * m * n];
			for (int i = 0; i < vertices.Length; i++)
			{
				float x = i % (n + 1);
				float y = i / (n + 1);
				float x_pos = x / n * width;
				float y_pos = y / m * height;
				vertices[i] = new Vector3(x_pos, y_pos, 0);
				float u = x / n;
				float v = y / m;
				uv[i] = new Vector2(u, v);
			}
		}
		for (int i = 0; i < 2 * m * n; i++)
		{
			int[] triIndex = new int[3];
			if (i % 2 == 0)
			{
				triIndex[0] = i / 2 + i / (2 * n);
				triIndex[1] = triIndex[0] + 1;
				triIndex[2] = triIndex[0] + (n + 1);
			}
			else
			{
				triIndex[0] = (i + 1) / 2 + i / (2 * n);
				triIndex[1] = triIndex[0] + (n + 1);
				triIndex[2] = triIndex[1] - 1;

			}
			triangles[i * 3] = triIndex[0];
			triangles[i * 3 + 1] = triIndex[1];
			triangles[i * 3 + 2] = triIndex[2];
		}
		int r = 2;
		for (int i = 0; i < vertices.Length; i++)
		{
			if (initialized == false)
			{
				vertexes[i] = Instantiate(vertexPrefab, this.transform);
				Vector3 v;
				v.x = r * Mathf.Cos(vertices[i].x / width * 2 * Mathf.PI) * Mathf.Cos(vertices[i].y / height * Mathf.PI - Mathf.PI / 2);
				v.y = r * Mathf.Sin(vertices[i].x / width * 2 * Mathf.PI) * Mathf.Cos(vertices[i].y / height * Mathf.PI - Mathf.PI / 2);
				v.z = r * Mathf.Sin(vertices[i].y / height * Mathf.PI - Mathf.PI / 2);
				//v = vertices[i];

				vertexes[i].InitializeVertex(EVertexType.None, v);
			}
			vertices[i] = vertexes[i].transform.position;

			normals[i] = new Vector3(0, 1, 0);
		}

		mesh.vertices = vertices;
		mesh.normals = normals;
		mesh.uv = uv;
		mesh.triangles = triangles;
		meshFilter.mesh = mesh;

		initialized = true;
	}
}
