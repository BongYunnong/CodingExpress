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

        Start_Val_InputField[0].SetTextWithoutNotify(myVector.startPos.x.ToString());
        Start_Val_InputField[1].SetTextWithoutNotify(myVector.startPos.y.ToString());
        Start_Val_InputField[2].SetTextWithoutNotify(myVector.startPos.z.ToString());

        End_Val_InputField[0].SetTextWithoutNotify(myVector.endPos.x.ToString());
        End_Val_InputField[1].SetTextWithoutNotify(myVector.endPos.y.ToString());
        End_Val_InputField[2].SetTextWithoutNotify(myVector.endPos.z.ToString());
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
            myVector.endPos);

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
            myVector.startPos, 
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
