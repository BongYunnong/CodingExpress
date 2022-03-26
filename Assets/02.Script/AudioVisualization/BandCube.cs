using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BandCube : MonoBehaviour
{
    public int band;
    public float startScaleY=1, scaleMultiplier=10;
    [SerializeField] bool useBuffer;
    private void Start()
    {
        startScaleY = transform.localScale.y;
    }
    private void Update()
    {
        if(useBuffer)
            transform.localScale = new Vector3(transform.localScale.x, (AudioPeer.audioBand[band] * scaleMultiplier) + startScaleY, transform.localScale.z);
        else
            transform.localScale = new Vector3(transform.localScale.x, (AudioPeer.audioBandBuffer[band] * scaleMultiplier) + startScaleY, transform.localScale.z);
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localScale.y/2f+ AudioPeer.amplitudeBuffer * scaleMultiplier, transform.localPosition.z);

        
    }
}
