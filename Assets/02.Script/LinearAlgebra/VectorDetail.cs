using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VectorDetail : MonoBehaviour
{
    private MyVector myVector;

    [SerializeField] TMPro.TMP_InputField vectorNameInputField;

    [SerializeField] InputField[] Start_Val_InputField;
    [SerializeField] InputField[] End_Val_InputField;

    public void SelectVector(MyVector _selectedVector)
    {
        myVector = _selectedVector;

        vectorNameInputField.SetTextWithoutNotify(myVector.name);

        Vector3 tmpPos = myVector.GetStartPos();
        Start_Val_InputField[0].SetTextWithoutNotify(tmpPos.x.ToString());
        Start_Val_InputField[1].SetTextWithoutNotify(tmpPos.y.ToString());
        Start_Val_InputField[2].SetTextWithoutNotify(tmpPos.z.ToString());

        tmpPos = myVector.GetEndPos();
        End_Val_InputField[0].SetTextWithoutNotify(tmpPos.x.ToString());
        End_Val_InputField[1].SetTextWithoutNotify(tmpPos.y.ToString());
        End_Val_InputField[2].SetTextWithoutNotify(tmpPos.z.ToString());
    }

    public void SetStartPos()
    {
        float[] tmpStartValue = new float[3];
        for (int i = 0; i < 3; i++)
        {
            float.TryParse(Start_Val_InputField[i].text, out tmpStartValue[i]);
        }
        myVector.InitializeVector(
            new Vector3(tmpStartValue[0], tmpStartValue[1], tmpStartValue[2]),
            myVector.GetEndPos());

       LinearAlgebraManager.GetInstance().UpdateAxisBasedPresentationVectors();
    }
    public void SetEndPos()
    {
        float[] tmpEndValue = new float[3];
        for (int i = 0; i < 3; i++)
        {
            float.TryParse(End_Val_InputField[i].text, out tmpEndValue[i]);
        }
        myVector.InitializeVector(
            myVector.GetStartPos(), 
            new Vector3(tmpEndValue[0], tmpEndValue[1], tmpEndValue[2]));
        LinearAlgebraManager.GetInstance().UpdateAxisBasedPresentationVectors();
    }

    public void SetVectorName(string _newName)
    {
        string tmpName = LinearAlgebraManager.GetInstance().GetUniqueName(_newName);
        myVector.SetVectorName(tmpName);
    }
    public void SetVectorColor(Color _newColor)
    {
        myVector.SetVectorColor(_newColor);
    }


    float scaling = 0;
    public void ChangeScale(float _value)
    {
        scaling = _value;
    }

    private void Update()
    {
        if (scaling != 0)
        {
            myVector.ChangeScale(scaling);
            LinearAlgebraManager.GetInstance().UpdateAxisBasedPresentationVectors();
        }
    }
}
