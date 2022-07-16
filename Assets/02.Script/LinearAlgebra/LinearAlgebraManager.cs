using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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

    public List<MyVector> selectedVectors=new List<MyVector>();

    [SerializeField] GameObject DetailPanel;

    [SerializeField] MyVector[] axisBasedPresentationVectors= new MyVector[3];
    bool initialized = false;
    private void Start()
    {
        axisBasedPresentationVectors[0].SetVectorName("baseAxisX");
        axisBasedPresentationVectors[0].SetVectorColor(new Color(1,0,0,0.5f));
        axisBasedPresentationVectors[1].SetVectorName("baseAxisY");
        axisBasedPresentationVectors[1].SetVectorColor(new Color(0, 1, 0, 0.5f));
        axisBasedPresentationVectors[2].SetVectorName("baseAxisZ");
        axisBasedPresentationVectors[2].SetVectorColor(new Color(0, 0, 1, 0.5f));
        for (int i=0;i< axisBasedPresentationVectors.Length; i++)
        {
            axisBasedPresentationVectors[i].gameObject.SetActive(false);
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
                    UpdateSelectedVectors(2, null);
                }
            }
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

            if (initialized)
            {
                axisBasedPresentationVectors[0].InitializeVector(
                    selectedVectors[0].startPos,
                    new Vector3(selectedVectors[0].endPos[0], selectedVectors[0].startPos[1], selectedVectors[0].startPos[2]));
                axisBasedPresentationVectors[1].InitializeVector(
                    axisBasedPresentationVectors[0].endPos,
                    new Vector3(axisBasedPresentationVectors[0].endPos[0], selectedVectors[0].endPos[1], axisBasedPresentationVectors[0].endPos[2]));
                axisBasedPresentationVectors[2].InitializeVector(
                    axisBasedPresentationVectors[1].endPos,
                    new Vector3(axisBasedPresentationVectors[1].endPos[0], axisBasedPresentationVectors[1].endPos[1], selectedVectors[0].endPos[2]));
            }
        }
    }
}
