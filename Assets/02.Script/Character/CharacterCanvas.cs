using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCanvas : MonoBehaviour
{
    [SerializeField] Text txt;
    [SerializeField] Image img;
    
    public void SetActiveInfoPanel(bool _active)
    {
        img.gameObject.SetActive(_active);
    }
    public void SetInfoTxt(string _txt)
    {
        txt.text = _txt;
    }
    public void SetInfoImg(Sprite _img)
    {
        img.sprite = _img;
    }
}
