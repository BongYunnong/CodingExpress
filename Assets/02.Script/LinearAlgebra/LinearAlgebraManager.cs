using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;


public class LinearAlgebraManager : MonoBehaviour
{
    private static LinearAlgebraManager instance;
    public static LinearAlgebraManager GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<LinearAlgebraManager>();
        }
        return instance;
    }

    [SerializeField] CoordinateSystem currentCoordinateSystem;
    public CoordinateSystem CurrentCoordinateSystem { get { return currentCoordinateSystem; } private set { }}

    public Dictionary<string, MyVector> myVectors = new Dictionary<string, MyVector>();

    [SerializeField] MyVector vectorPrefab;
    public List<MyVector> selectedVectors=new List<MyVector>();

    [SerializeField] GameObject DetailPanel;

    [SerializeField] MyVector[] axisBasedPresentationVectors= new MyVector[3];
    bool initialized = false;

    [SerializeField] TMPro.TMP_Text calculatedResultTxt;

    private void Start()
    {
        if (vectorPrefab)
        {
            axisBasedPresentationVectors[0] = Instantiate(vectorPrefab, transform);
            axisBasedPresentationVectors[0].InitializeVector(Vector3.zero, Vector3.right * 100);
            axisBasedPresentationVectors[0].SetVectorName("baseAxisX");
            axisBasedPresentationVectors[0].SetVectorColor(new Color(1, 0, 0, 0.5f));

            axisBasedPresentationVectors[1] = Instantiate(vectorPrefab, transform);
            axisBasedPresentationVectors[1].InitializeVector(Vector3.zero, Vector3.up * 100);
            axisBasedPresentationVectors[1].SetVectorName("baseAxisY");
            axisBasedPresentationVectors[1].SetVectorColor(new Color(0, 1, 0, 0.5f));

            axisBasedPresentationVectors[2] = Instantiate(vectorPrefab, transform);
            axisBasedPresentationVectors[2].InitializeVector(Vector3.zero, Vector3.forward * 100);
            axisBasedPresentationVectors[2].SetVectorName("baseAxisZ");
            axisBasedPresentationVectors[2].SetVectorColor(new Color(0, 0, 1, 0.5f));
            for (int i = 0; i < axisBasedPresentationVectors.Length; i++)
            {
                axisBasedPresentationVectors[i].gameObject.SetActive(false);
            }
        }
        initialized = true;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject()==false)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                MyVector myVector = hit.transform.GetComponentInParent<MyVector>();
                if (myVector)
                {
                    if (Input.GetKey(KeyCode.LeftShift))
                        UpdateSelectedVectors(0, myVector);
                    else
                    {
                        UpdateSelectedVectors(1, myVector);
                    }
                }
                else
                {
                    UpdateSelectedVectors(1, null);
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {

            UpdateSelectedVectors(2, null);
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            Camera.main.transform.position = new Vector3(0, 0, -3);
            Camera.main.transform.LookAt(Vector3.zero);
        }

        if (Input.GetKeyDown(KeyCode.Space) )
        {
            CalculateVectorSum();
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            CalculateVectorDotProduct();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            CalculateVectorCrossProduct();
        }
    }


    public void RegisterVector(string tmpName, MyVector tmpVector)
    {
        myVectors.Add(tmpName, tmpVector);
        UpdateSelectedVectors(1, tmpVector);
        //Debug.Log(myVectors.ContainsKey(tmpName) + " is " + tmpName);
    }
    public string GetUniqueName(string _input)
    {
        int index = 0;
        //Debug.Log("... : " + _input);
        do
        {
            //Debug.Log("in while : " + _input);
            if (_input.Contains("_"))
            {
                int.TryParse(_input.Split('_')[1], out index);
                index++;
                _input = _input.Split('_')[0] + "_" + index;
            }
            else
            {
                _input = _input + "_" + index;
            }
        } while (myVectors.ContainsKey(_input));
        return _input;
    }
    private void UpdateSelectedVectors(int _index,MyVector _newVector)
    {
        if(_index== 1 || _index== 2)
        {
            for (int i = 0; i < selectedVectors.Count; i++)
            {
                selectedVectors[i].Activate(false);
            }
            selectedVectors.Clear();
        }

        if (_newVector != null)
        {
            selectedVectors.Add(_newVector);
            for (int i = 0; i < selectedVectors.Count; i++)
            {
                selectedVectors[i].Activate(true);
            }
        }

        bool openDetailPanel = selectedVectors.Count == 1;
        DetailPanel.SetActive(selectedVectors.Count == 1);
        for (int i = 0; i < 3; i++)
            axisBasedPresentationVectors[i]?.gameObject.SetActive(openDetailPanel);
        if (openDetailPanel)
        {
            DetailPanel.GetComponent<VectorDetail>().SelectVector(selectedVectors[0]);
            UpdateAxisBasedPresentationVectors();
        }
    }

    public void UpdateAxisBasedPresentationVectors()
    {
        bool openDetailPanel = selectedVectors.Count == 1;
        if (initialized && openDetailPanel && axisBasedPresentationVectors[0]!=null)
        {
            axisBasedPresentationVectors[0].InitializeVector(
                selectedVectors[0].GetStartPos(),
                new Vector3(selectedVectors[0].GetEndPos().x, selectedVectors[0].GetStartPos().y, selectedVectors[0].GetStartPos().z));

            axisBasedPresentationVectors[1].InitializeVector(
                Vector3.zero,
                new Vector3(
                    0, selectedVectors[0].GetEndPos().y - axisBasedPresentationVectors[0].GetEndPos().y, 0)
                );
            axisBasedPresentationVectors[1].transform.position = axisBasedPresentationVectors[0].GetEndPos();
            
            axisBasedPresentationVectors[2].InitializeVector(
                Vector3.zero,
                new Vector3(
                    0 , 0 , selectedVectors[0].GetEndPos().z- axisBasedPresentationVectors[1].GetEndPos().z));
            axisBasedPresentationVectors[2].transform.position = axisBasedPresentationVectors[1].GetEndPos();
        }
    }

    public void CalculateVectorSum()
    {
        if (selectedVectors.Count >= 2)
        {
            VectorGenerator vg = FindObjectOfType<VectorGenerator>();

            Vector3 guidStartPos = selectedVectors[0].GetStartPos();
            for (int i = 1; i < selectedVectors.Count; i++)
            {
                Vector3 VectorA = selectedVectors[i - 1].GetEndPos() - selectedVectors[i - 1].GetStartPos();
                Vector3 VectorB = selectedVectors[i].GetEndPos() - selectedVectors[i].GetStartPos();
                MyVector calculatedVec = vg.CreateVector(false,
                    guidStartPos.x, guidStartPos.y, guidStartPos.z,
                    guidStartPos.x + VectorA.x + VectorB.x,
                    guidStartPos.y + VectorA.y + VectorB.y,
                    guidStartPos.z + VectorA.z + VectorB.z,
                    "VectorSumGuide"
                    );
                guidStartPos = calculatedVec.GetEndPos();
            }
        }
    }

    public void CalculateVectorDotProduct()
    {
        if (selectedVectors.Count == 2)
        {
            calculatedResultTxt.text = selectedVectors[0].name + " Dot " + selectedVectors[1].name +
                "<br> = " + Vector3.Dot(selectedVectors[0].distance, selectedVectors[1].distance) +
                "<br> = ||" + selectedVectors[0].name + "|| * ||" + selectedVectors[1].name + "|| * Cos(" + Vector3.Angle(selectedVectors[0].distance, selectedVectors[1].distance) + ")" +
                "<br> = " + selectedVectors[0].magnitude + " * " + selectedVectors[1].magnitude + " * " + Mathf.Cos(Vector3.Angle(selectedVectors[0].distance, selectedVectors[1].distance) * Mathf.PI / 180);
            VectorGenerator vg = FindObjectOfType<VectorGenerator>();

            Vector3 projectedVec = Vector3.Project(selectedVectors[0].distance, selectedVectors[1].distance);

            Vector3 tmpStartPos = selectedVectors[0].GetStartPos();
            Vector3 tmpEndPos = selectedVectors[0].GetEndPos();
            MyVector calculatedVec = vg.CreateVector(false,
                tmpEndPos.x, tmpEndPos.y, tmpEndPos.z,
                tmpStartPos.x + projectedVec.x, tmpStartPos.y + projectedVec.y, tmpStartPos.z + projectedVec.z,
                "VectorDotProductGuide"
                );
        }
    }
    public void CalculateVectorCrossProduct()
    {
        if (selectedVectors.Count == 2)
        {
            Vector3 crossedVec = Vector3.Cross(selectedVectors[0].distance, selectedVectors[1].distance);
            calculatedResultTxt.text = selectedVectors[0].name + " Cross " + selectedVectors[1].name +
                "<br> = " + crossedVec +
                "<br> Vector's length = ||" + selectedVectors[0].name + "|| * ||" + selectedVectors[1].name + "|| * Sin(" + Vector3.Angle(selectedVectors[0].distance, selectedVectors[1].distance) + ")" +
                "<br> Vector's length = " + selectedVectors[0].magnitude + " * " + selectedVectors[1].magnitude + " * " + Mathf.Sin(Vector3.Angle(selectedVectors[0].distance, selectedVectors[1].distance) * Mathf.PI / 180);
            VectorGenerator vg = FindObjectOfType<VectorGenerator>();


            Vector3 tmpPos = selectedVectors[0].GetStartPos();
            MyVector calculatedVec = vg.CreateVector(false,
                tmpPos.x, tmpPos.y, tmpPos.z,
                tmpPos.x + crossedVec.x, tmpPos.y + crossedVec.y, tmpPos.z + crossedVec.z,
                "VectorCrossProductGuide"
                );
        }
    }
}
