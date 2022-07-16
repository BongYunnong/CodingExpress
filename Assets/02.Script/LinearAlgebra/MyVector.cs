using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyVector : MonoBehaviour
{
    private string vectorName;
    [SerializeField] MeshRenderer vectorStart;
    [SerializeField] MeshRenderer vectorHead;
    [SerializeField] LineRenderer vectorBody;

    [SerializeField] LineRenderer vectorLengthParabola;

    [SerializeField] GameObject vectorInfoBox;
    [SerializeField] TextMesh vectorNameTextMesh;
    [SerializeField] TextMesh vectorLengthTextMesh;
    [SerializeField] TextMesh vectorPosTextMesh;

    public Vector3 startPos;
    public Vector3 endPos;

    public void InitializeVector(Vector3 _startPos, Vector3 _endPos)
    {
        this.transform.position = _startPos;
        vectorBody.SetPosition(0,Vector3.zero);
        vectorBody.SetPosition(1, _endPos- _startPos);

        vectorHead.transform.position = _endPos;
        vectorHead.transform.LookAt(_endPos + (_endPos - _startPos).normalized);
        vectorPosTextMesh.transform.position = vectorHead.transform.position;

        vectorNameTextMesh.transform.LookAt(_startPos + Vector3.Cross((_endPos - _startPos), Vector3.up));
        vectorPosTextMesh.transform.LookAt(_endPos + Vector3.Cross((_endPos - _startPos), Vector3.up));

        Vector3 dist = (_endPos - _startPos);
        vectorPosTextMesh.text = "(" + dist.x + "," + dist.y + "," + dist.z + ")";

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
        vectorLengthTextMesh.text = (_endPos - _startPos).magnitude.ToString();

        startPos = _startPos;
        endPos = _endPos;
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
        vectorStart.material.color = _color;
        vectorHead.material.color = _color;
        vectorBody.material.color = _color;
    }

    public void Activate(bool _activate)
    {
        vectorInfoBox.gameObject.SetActive(_activate);
    }
}
