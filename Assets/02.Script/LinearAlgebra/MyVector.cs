using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyVector : MonoBehaviour
{
    CoordinateSystem _coordinateSystem;
    private string vectorName;
    MyVertex startVertex;
    MyVertex endVertex;
    [SerializeField] LineRenderer vectorBody;

    [SerializeField] LineRenderer vectorLengthParabola;

    [SerializeField] GameObject vectorInfoBox;
    [SerializeField] TextMesh vectorNameTextMesh;
    [SerializeField] TextMesh vectorLengthTextMesh;
    [SerializeField] TextMesh vectorPosTextMesh;

    [SerializeField] private MyVertex vertexPrefab;
    public Vector3 distance { get; private set; }
    private Vector3 direction;
    public float magnitude{ get; private set; }

    public Vector3 GetStartPos()
    {
        return startVertex.transform.position;
    }
    public Vector3 GetEndPos()
    {
        return endVertex.transform.position;
    }

    public void InitializeVector(Vector3 _startPos, Vector3 _endPos, bool transforming=false)
    {
        _coordinateSystem = FindObjectOfType<CoordinateSystem>();
        if (startVertex == null)
        {
            startVertex = Instantiate(vertexPrefab, this.transform);
        }
        if (endVertex == null)
        {
            endVertex = Instantiate(vertexPrefab, this.transform);
        }
        if (transforming == false)
        {
            startVertex.InitializeVertex(EVertexType.Point, _startPos);
            endVertex.InitializeVertex(EVertexType.ArrowHead, _endPos);
        }

        // Body - LineRenderer
        this.transform.position = _startPos;
        vectorBody.SetPosition(0,Vector3.zero);
        vectorBody.SetPosition(1, _endPos- _startPos);

        endVertex.transform.LookAt(_endPos + (_endPos - _startPos).normalized);
        vectorPosTextMesh.transform.position = endVertex.transform.position;

        vectorNameTextMesh.transform.LookAt(_startPos + Vector3.Cross((_endPos - _startPos), Vector3.up));
        vectorPosTextMesh.transform.LookAt(_endPos + Vector3.Cross((_endPos - _startPos), Vector3.up));

        distance = (_endPos - _startPos);

        vectorPosTextMesh.text = "(" + distance.x.ToString("F2") + "," + distance.y.ToString("F2") + "," + distance.z.ToString("F2") + ")";

        int timer = 0;
        while (timer< vectorLengthParabola.positionCount)
        {
            Vector3 tempPos = Parabola(_startPos,_endPos, 0.5f, timer/(float)(vectorLengthParabola.positionCount-1));

            if (timer == (int)vectorLengthParabola.positionCount / 2)
            {
                vectorLengthTextMesh.transform.position = tempPos;
                vectorLengthTextMesh.transform.LookAt(tempPos + Vector3.Cross((_endPos - _startPos), Vector3.up));
            }

            vectorLengthParabola.SetPosition(timer, tempPos-_startPos);
            timer += 1;
        }
        magnitude = distance.magnitude;
        vectorLengthTextMesh.text = magnitude.ToString("F2");

        direction = distance.normalized;
    }
    protected static Vector3 Parabola(Vector3 start, Vector3 end, float height, float t)
    {
        Func<float, float> f = x => -4 * height * x * x + 4 * height * x;

        var mid = Vector3.Lerp(start, end, t);

        return new Vector3(mid.x, f(t) + Mathf.Lerp(start.y, end.y, t), mid.z);
    }


    public void SetVectorName(string _vectorName)
    {
        this.transform.name = _vectorName ;
        vectorName = _vectorName;
        vectorNameTextMesh.text = vectorName;
    }
    public void SetVectorColor(Color _color)
    {
        if (startVertex.meshRenderer.material)
        {
            _color.a = startVertex.meshRenderer.material.color.a;
            startVertex.meshRenderer.material.color = _color;
            endVertex.meshRenderer.material.color = _color;
            vectorBody.material.color = _color;
            Debug.Log(_color);
        }
    }

    public void Activate(bool _activate)
    {
        vectorInfoBox.gameObject.SetActive(_activate);
    }

    public void ChangeScale(float _value)
    {
        InitializeVector(GetStartPos(), GetStartPos() + direction * (magnitude + _value * Time.deltaTime));
    }
    public void UpdateVector()
    {
        InitializeVector(GetStartPos(), GetEndPos(),true);
    }
}
