using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingTextController : MonoBehaviour
{
    private static FloatingText popupText;
    private static GameObject canvas;

    public static void InitializeFloatingTextController()
    {
        canvas = GameObject.Find("MainGameCanvas");
        popupText = Resources.Load<FloatingText>("FloatTextParent");
    }
    public static void CreateFloatingText(string text, Transform location, Color color, Vector3 offset)
    {
        InitializeFloatingTextController();
        FloatingText instance = Instantiate(popupText);
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(new Vector3(location.position.x,location.position.y, -0.1f)+ offset);
        instance.transform.SetParent(canvas.transform, false);

        instance.transform.position = screenPosition;
        instance.gameObject.GetComponentInChildren<Text>().color = color;
        instance.SetText(text);
    }
}
