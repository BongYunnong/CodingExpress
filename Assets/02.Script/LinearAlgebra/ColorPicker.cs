using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

[System.Serializable]
public class ColorEvent : UnityEvent<Color> { }
public class ColorPicker : MonoBehaviour
{
    public TMP_Text debugTxt;
    RectTransform Rect;
    Texture2D ColorTexture;

    [SerializeField] Image ColorPreviewImg;
     
    public ColorEvent OnColorPreview;
    public ColorEvent OnColorSelect;

    public Color currentColor;
    void Start()
    {
        Rect = GetComponent<RectTransform>();
        ColorTexture = GetComponent<Image>().mainTexture as Texture2D;

        OnColorSelect.AddListener(SetColor);
    }
    private void SetColor(Color _color)
    {
        currentColor = _color;
        ColorPreviewImg.color = currentColor;
    }
    void Update()
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(Rect, Input.mousePosition))
        {
            Vector2 delta;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(Rect, Input.mousePosition, null, out delta);

            string debug = "mousePosition =" + Input.mousePosition;
            debug += "<br>delta =" + delta;

            float width = Rect.rect.width;
            float height = Rect.rect.height;
            /*
            delta += new Vector2(width * 0.5f, height * 0.5f);
            debug += "<br>offset delta=" + delta;
            */

            float x = Mathf.Clamp(delta.x / width, 0, 1f);
            float y = Mathf.Clamp(delta.y / height, 0, 1f);
            debug += "<br>x =" + x + " y = " + y;

            int texX = Mathf.RoundToInt(x * ColorTexture.width);
            int texY = Mathf.RoundToInt(y * ColorTexture.height);
            debug += "<br>texX =" + texX + " texY = " + texY;

            Color color = ColorTexture.GetPixel(texX, texY);

            debugTxt.color = currentColor;
            debugTxt.text = debug;

            OnColorPreview?.Invoke(color);

            if (Input.GetMouseButtonDown(0))
            {
                OnColorSelect?.Invoke(color);
            }
        }
    }
}
