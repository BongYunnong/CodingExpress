using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EGridType
{
    XYPlane,
    XZPlane,
    YZPlane
}
public class MeshPlane : MonoBehaviour
{
	CoordinateSystem coordinateSystem;
    [SerializeField] MyVertex _myVertexPrefab;
    MyVertex[] myVertex= new MyVertex[4];

	MeshFilter meshFilter;
    public void InitializeMeshPlane(float width, float height, EGridType gridType,int part=-1)
    {
		coordinateSystem = FindObjectOfType<CoordinateSystem>();
		meshFilter = GetComponent<MeshFilter>();

        for(int i = 0; i < 4; i++)
        {
            myVertex[i] = Instantiate(_myVertexPrefab, transform);
        }


        int[] dx = { 1, 1, -1, -1 };
        int[] dy = { 1, -1, -1, 1 };
        switch (gridType)
        {
            case EGridType.XYPlane:

                myVertex[0].InitializeVertex(EVertexType.None, new Vector3(dx[part]*0       , dy[part]*0));
                myVertex[1].InitializeVertex(EVertexType.None, new Vector3(dx[part]*width   , dy[part]*0));
                myVertex[2].InitializeVertex(EVertexType.None, new Vector3(dx[part]*width   , dy[part]*height));
                myVertex[3].InitializeVertex(EVertexType.None, new Vector3(dx[part]*0       , dy[part]*height));
                break;
            case EGridType.XZPlane:
                myVertex[0].InitializeVertex(EVertexType.None, new Vector3(dx[part]*0       , 0, dy[part]*0));
                myVertex[1].InitializeVertex(EVertexType.None, new Vector3(dx[part]*width   , 0, dy[part]*0));
                myVertex[2].InitializeVertex(EVertexType.None, new Vector3(dx[part]*width   , 0, dy[part]*height));
                myVertex[3].InitializeVertex(EVertexType.None, new Vector3(dx[part]*0       , 0, dy[part]*height));
                break;
            case EGridType.YZPlane:
                myVertex[0].InitializeVertex(EVertexType.None, new Vector3(0, dy[part]*0        , dx[part]*0));
                myVertex[1].InitializeVertex(EVertexType.None, new Vector3(0, dy[part]*width    , dx[part]*0));
                myVertex[2].InitializeVertex(EVertexType.None, new Vector3(0, dy[part]*width    , dx[part]*height));
                myVertex[3].InitializeVertex(EVertexType.None, new Vector3(0, dy[part]*0        , dx[part]*height));
                break;
        }

        Mesh m = new Mesh();
        m.name = "MeshPlane";
        Vector3[] verts = meshFilter.mesh.vertices;
        verts = new Vector3[]
            {
                     myVertex[0].transform.position,
                     myVertex[1].transform.position,
                     myVertex[2].transform.position,
                     myVertex[3].transform.position,
            };
        m.vertices = verts;
        m.uv = new Vector2[]
        {
             new Vector2 (0, 0),
             new Vector2 (0, 1),
             new Vector2(1, 1),
             new Vector2 (1, 0)
        };
        m.triangles = new int[] { 0, 1, 2, 0, 2, 3 };

        m.RecalculateNormals();
        meshFilter.mesh = m;
    }
    void Update()
	{
		Vector3[] verts = meshFilter.mesh.vertices;
		verts = new Vector3[]
			{
					 myVertex[0].transform.position,
					 myVertex[1].transform.position,
					 myVertex[2].transform.position,
					 myVertex[3].transform.position,
			};
		meshFilter.mesh.vertices = verts;
	}
}
