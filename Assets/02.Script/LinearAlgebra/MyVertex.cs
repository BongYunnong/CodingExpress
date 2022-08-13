using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EVertexType
{
	None,
	Point,
	ArrowHead
}
public class MyVertex : MonoBehaviour
{
	CoordinateSystem coordinateSystem;
	BoxCollider boxCollider;

	MeshFilter meshFilter;
	public MeshRenderer meshRenderer{get; private set;}
	[SerializeField] Mesh[] meshes;
	[SerializeField] Material[] meshMaterials;

	Vector3 originPos;
    public void InitializeVertex(EVertexType vertexType, Vector3 pos)
	{
		boxCollider = GetComponent<BoxCollider>();
		meshFilter = GetComponent<MeshFilter>();
		meshRenderer = GetComponent<MeshRenderer>();
		coordinateSystem = FindObjectOfType<CoordinateSystem>();
        switch (vertexType)
		{
			case EVertexType.Point:
				break;
			case EVertexType.ArrowHead:
				break;
			default:
				break;
		}

		meshFilter.mesh = meshes[(int)vertexType];
		meshRenderer.material = meshMaterials[(int)vertexType];
		this.transform.position = pos;
		originPos = pos;
	}
    
	public void UpdatePosition()
    {
		Matrix4x4 transformingMatrix = coordinateSystem.GetTransformingMatrix();
		this.transform.position = transformingMatrix.MultiplyPoint(originPos);
	}
	public void EndUpdate()
	{
		originPos = this.transform.position;
	}
}
