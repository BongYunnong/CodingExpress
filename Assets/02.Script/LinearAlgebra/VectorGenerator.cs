using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VectorGenerator : MonoBehaviour
{
    [SerializeField] GameObject VectorPrefab;
    [SerializeField] GameObject GuideVectorPrefab;

    [SerializeField] TMPro.TMP_InputField vectorNameInputField;

    [SerializeField] InputField[] Start_Val_InputField;
    [SerializeField] InputField[] End_Val_InputField;

    [SerializeField] ColorPicker vectorColorPicker;


    public void CreateVectorByUI(bool _isRealVector)
    {
        float[] tmpStartValue = new float[3];
        for (int i = 0; i < 3; i++)
        {
            float.TryParse(Start_Val_InputField[i].text, out tmpStartValue[i]);
        }
        float[] tmpEndValue = new float[3];
        for (int i = 0; i < 3; i++)
        {
            float.TryParse(End_Val_InputField[i].text, out tmpEndValue[i]);
        }
        CreateVector(_isRealVector,
            tmpStartValue[0], tmpStartValue[1], tmpStartValue[2],
            tmpEndValue[0], tmpEndValue[1], tmpEndValue[2], vectorNameInputField.text);

        
    }
    public MyVector CreateVector(bool _isRealVector,
        float _startX, float _startY, float _startZ,
        float _endX, float _endY, float _endZ, string _vectorName="Vec_")
    {
        MyVector tmpVector = Instantiate(_isRealVector? VectorPrefab : GuideVectorPrefab, LinearAlgebraManager.GetInstance().CurrentCoordinateSystem.transform).GetComponent<MyVector>();
        tmpVector.InitializeVector(
            new Vector3(_startX, _startY, _startZ),
            new Vector3(_endX, _endY, _endZ));
        string tmpName = LinearAlgebraManager.GetInstance().GetUniqueName(_vectorName);
        tmpVector.SetVectorName(tmpName);
        tmpVector.SetVectorColor(vectorColorPicker.currentColor);
        LinearAlgebraManager.GetInstance().RegisterVector(tmpName, tmpVector);

        return tmpVector;
    }
}
