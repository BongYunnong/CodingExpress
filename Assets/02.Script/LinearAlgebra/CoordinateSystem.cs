using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoordinateSystem : MonoBehaviour
{
    [SerializeField] LineRenderer axis_X_Line;
    [SerializeField] LineRenderer axis_Y_Line;
    [SerializeField] LineRenderer axis_Z_Line;

    [SerializeField] float maxAxisLineLength=10000000000;

    [SerializeField] MeshPlane gridPlanePrefab;
    [SerializeField] private float _initialGridWidth = 2;
    [SerializeField] private float _initialGridHeight = 2;

    [SerializeField] MyCube _myCubePrefab;
    [SerializeField] MySphere _mySpherePrefab;

    [Header("[Linear Transformation]")]
    private Matrix4x4 target_Basis_Matrix;
    private Matrix4x4 transforming_Basis_Matrix;
    public ArrayLayout BasisMatrix;

    public bool LinearTransforming = false;

    [SerializeField] Transform XYPlaneParent;
    [SerializeField] Transform XZPlaneParent;
    [SerializeField] Transform YZPlaneParent;

    private MeshPlane[] XYPlanes = new MeshPlane[4];
    private MeshPlane[] XZPlanes = new MeshPlane[4];
    private MeshPlane[] YZPlanes = new MeshPlane[4];

    public Matrix4x4 GetTargetTransformMatrix()
    {
        return target_Basis_Matrix;
    }
    public Matrix4x4 GetTransformingMatrix()
    {
        return transforming_Basis_Matrix;
    }

    private void OnValidate()
    {
        if (LinearTransforming == false)
        {
            for (int i = 0; i < 4; i++)
            {
                target_Basis_Matrix.SetRow(i, new Vector4(BasisMatrix.rows[i].row[0], BasisMatrix.rows[i].row[1], BasisMatrix.rows[i].row[2], BasisMatrix.rows[i].row[3]));
            }
            for (int i = 0; i < 4; i++)
            {
                transforming_Basis_Matrix.SetRow(i, new Vector4(i == 0 ? 1 : 0, i == 1 ? 1 : 0, i == 2 ? 1 : 0, i == 3 ? 1 : 0));
            }
        }
    }

    void Start()
    {
        axis_X_Line.SetPosition(0, -Vector3.right * maxAxisLineLength);
        axis_X_Line.SetPosition(1, Vector3.right * maxAxisLineLength);
        axis_Y_Line.SetPosition(0, -Vector3.up * maxAxisLineLength);
        axis_Y_Line.SetPosition(1, Vector3.up * maxAxisLineLength);
        axis_Z_Line.SetPosition(0, -Vector3.forward * maxAxisLineLength);
        axis_Z_Line.SetPosition(1, Vector3.forward * maxAxisLineLength);

        for (int i = 0; i < 4; i++)
        {
            XYPlanes[i] = Instantiate(gridPlanePrefab, XYPlaneParent);
            XYPlanes[i].InitializeMeshPlane(_initialGridWidth, _initialGridHeight,EGridType.XYPlane,i);
        }
        
        for (int i = 0; i < 4; i++)
        {
            XZPlanes[i] = Instantiate(gridPlanePrefab, XZPlaneParent);
            XZPlanes[i].InitializeMeshPlane(_initialGridWidth, _initialGridHeight, EGridType.XZPlane, i);
        }
        for (int i = 0; i < 4; i++)
        {
            YZPlanes[i] = Instantiate(gridPlanePrefab, YZPlaneParent);
            YZPlanes[i].InitializeMeshPlane(_initialGridWidth, _initialGridHeight, EGridType.YZPlane, i);
        }
        

        // Linear Transformation
        for (int i = 0; i < 4; i++)
        {
            target_Basis_Matrix.SetRow(i, new Vector4(BasisMatrix.rows[i].row[0], BasisMatrix.rows[i].row[1], BasisMatrix.rows[i].row[2], BasisMatrix.rows[i].row[3]));
        }
        for (int i = 0; i < 4; i++)
        {
            transforming_Basis_Matrix.SetRow(i, new Vector4(i == 0 ? 1 : 0, i == 1 ? 1 : 0, i == 2 ? 1 : 0, i == 3 ? 1 : 0));
        }

        VectorGenerator vg =FindObjectOfType<VectorGenerator>();
        vg.CreateVector(true, 0, 0, 0, 1, 0, 0, "i-hat");
        vg.CreateVector(true, 0, 0, 0, 0, 1, 0, "j-hat");
    }
    float coolTime = 1;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && LinearTransforming == false)
        {
            LinearTransforming = true;
        }

        if (LinearTransforming)
        {
            coolTime = Mathf.Lerp(coolTime, 0, Time.deltaTime);

            MyVertex[] vertexes = FindObjectsOfType<MyVertex>();
            if (coolTime <= 0.01f)
            {
                for (int i = 0; i < 4; i++)
                {
                    transforming_Basis_Matrix.SetRow(i, new Vector4(i == 0 ? 1 : 0, i == 1 ? 1 : 0, i == 2 ? 1 : 0, i == 3 ? 1 : 0));
                }
                LinearTransforming = false;
                coolTime = 1;
                for (int i = 0; i < vertexes.Length; i++)
                {
                    vertexes[i].EndUpdate();
                }

            }
            for (int i = 0; i < 4; i++)
            {
                transforming_Basis_Matrix.SetRow(i, new Vector4(
                    Mathf.Lerp(transforming_Basis_Matrix[i,0], target_Basis_Matrix[i,0],Time.deltaTime),
                    Mathf.Lerp(transforming_Basis_Matrix[i, 1], target_Basis_Matrix[i,1], Time.deltaTime),
                    Mathf.Lerp(transforming_Basis_Matrix[i, 2], target_Basis_Matrix[i,2], Time.deltaTime),
                    Mathf.Lerp(transforming_Basis_Matrix[i, 3], target_Basis_Matrix[i,3], Time.deltaTime)));
            }

            for (int i = 0; i < vertexes.Length; i++)
            {
                vertexes[i].UpdatePosition();
            }
            MyVector[] vectors = FindObjectsOfType<MyVector>();
            for (int i = 0; i < vectors.Length; i++)
            {
                vectors[i].UpdateVector();
            }
            MyCube[] cubes = FindObjectsOfType<MyCube>();
            for (int i = 0; i < cubes.Length; i++)
            {
                cubes[i].InitializeCube();
            }
            MySphere[] spheres = FindObjectsOfType<MySphere>();
            for (int i = 0; i < spheres.Length; i++)
            {
                spheres[i].InitializeSphere();
            }


        }


        if (Input.GetKeyDown(KeyCode.C))
        {
            MyCube cube = Instantiate(_myCubePrefab, this.transform);
            cube.InitializeCube();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            MySphere sphere = Instantiate(_mySpherePrefab, this.transform);
            sphere.InitializeSphere();
        }


        VectorGenerator vectorGenerator = FindObjectOfType<VectorGenerator>();
        if (Input.GetKeyDown(KeyCode.V))
        {
            for (int i = -2; i <= 2; i++)
            {
                for (int j = -2; j <= 2; j++)
                {
                    for (int k = -2; k <= 2; k++)
                    {
                        vectorGenerator.CreateVector(true,0, 0, 0, i, j, k,"newVec"+i+"-"+j+"-"+k);
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            int random = Random.Range(0, LinearAlgebraManager.GetInstance().myVectors.Count);
            MyVector tmpVec=null;
            foreach (KeyValuePair<string,MyVector> v in LinearAlgebraManager.GetInstance().myVectors)
            {
                random--;
                if (random <= 0)
                {
                    tmpVec = v.Value;
                    break;
                }
            }

            Vector3 endPos = tmpVec.GetEndPos();
            FindObjectOfType<VectorGenerator>().CreateVector(true,
                endPos.x, endPos.y, endPos.z,
                Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10));
        }
    }

    public void XYPlane(bool active)
    {
        for (int i = 0; i < 4; i++)
        {
            XYPlanes[i].gameObject.SetActive(active);
        }
    }
    public void XZPlane(bool active)
    {
        for (int i = 0; i < 4; i++)
        {
            XZPlanes[i].gameObject.SetActive(active);
        }
    }
    public void YZPlane(bool active)
    {
        for (int i = 0; i < 4; i++)
        {
            YZPlanes[i].gameObject.SetActive(active);
        }
    }


    [SerializeField] List<inputFieldRow> transformMatrixInputFields;
    [System.Serializable]
    public struct inputFieldRow
    {
        public List<TMPro.TMP_InputField> rowFields;
    }
    public void HandleTransformationBtn()
    {
        if (LinearTransforming == false)
        {
            LinearTransforming = true;
            for (int i = 0; i < 4; i++)
            {
                target_Basis_Matrix.SetRow(i, new Vector4(
                    int.Parse(transformMatrixInputFields[i].rowFields[0].text),
                    int.Parse(transformMatrixInputFields[i].rowFields[1].text),
                    int.Parse(transformMatrixInputFields[i].rowFields[2].text),
                    int.Parse(transformMatrixInputFields[i].rowFields[3].text)));
            }
            for (int i = 0; i < 4; i++)
            {
                transforming_Basis_Matrix.SetRow(i, new Vector4(i == 0 ? 1 : 0, i == 1 ? 1 : 0, i == 2 ? 1 : 0, i == 3 ? 1 : 0));
            }
        }
    }
}
