using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioVisualizationManager : MonoBehaviour
{
    [SerializeField] GameObject VisualizationMeshPrefab;
    GameObject[] VisualizationMeshs = new GameObject[512];
    [SerializeField] float multipleScale=1f;
    private void Start()
    {
        for(int i = 0; i < 512; i++)
        {
            GameObject tmpMesh = Instantiate(VisualizationMeshPrefab, transform);
            tmpMesh.name = "SampleCube" + i;
            this.transform.eulerAngles = new Vector3(0, (360 / 512f) * i, 0);
            tmpMesh.transform.position = Vector3.forward * 10;
            VisualizationMeshs[i] = tmpMesh;
        }
    }
    private void Update()
    {
        for(int i = 0; i < 512; i++)
        {
            if (VisualizationMeshs != null)
            {
                VisualizationMeshs[i].transform.localScale = new Vector3(0.1f, AudioPeer.samplesLeft[i] * multipleScale + 0.05f, 0.1f);
                VisualizationMeshs[i].transform.localPosition = new Vector3(VisualizationMeshs[i].transform.localPosition.x, VisualizationMeshs[i].transform.localScale.y/2f, VisualizationMeshs[i].transform.localPosition.z);
            }
        }
    }

}
