using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelReader : MonoBehaviour
{
    struct pixelInfo {
        public pixelInfo(int _x,int _y,Color _c)
        {
            x = _x;
            y = _y;
            c = _c;
        }
        public int x { get; set; }
        public int y { get; set; }
        public Color c { get; set; }
    }

    [SerializeField] string willLoadImageName;
    List<pixelInfo> imagePixelInfos=new List<pixelInfo>();

    [SerializeField] GameObject circlePrefab;
    private void Start()
    {
        ReadPixel();

        for(int i = 0; i < imagePixelInfos.Count; i+=100)
        {
            Instantiate(circlePrefab, new Vector3(imagePixelInfos[i].x, imagePixelInfos[i].y, 0f), Quaternion.identity);
        }
    }
    private void ReadPixel()
    {
        Texture2D currTexture = (Texture2D)Resources.Load(willLoadImageName);
        if (currTexture == null)
            return;

        for(int i = 0; i < currTexture.width; i++)
        {
            for(int j = 0; j < currTexture.height; j++)
            {
                Color c = currTexture.GetPixel(i, j);
                pixelInfo pi = new pixelInfo(i, j, c);
                imagePixelInfos.Add(pi);
            }
        }
    }
}
