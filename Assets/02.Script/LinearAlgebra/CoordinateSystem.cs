using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoordinateSystem : MonoBehaviour
{
    [SerializeField] LineRenderer axis_X_Line;
    [SerializeField] LineRenderer axis_Y_Line;
    [SerializeField] LineRenderer axis_Z_Line;

    [SerializeField] float maxAxisLineLength=10000000000;

    [SerializeField] GameObject GridPlanePrefab;

    void Start()
    {
        axis_X_Line.SetPosition(0, -Vector3.right * maxAxisLineLength);
        axis_X_Line.SetPosition(1, Vector3.right * maxAxisLineLength);
        axis_Y_Line.SetPosition(0, -Vector3.up * maxAxisLineLength);
        axis_Y_Line.SetPosition(1, Vector3.up * maxAxisLineLength);
        axis_Z_Line.SetPosition(0, -Vector3.forward * maxAxisLineLength);
        axis_Z_Line.SetPosition(1, Vector3.forward * maxAxisLineLength);

        int[] dx = { 1, 1, -1, -1 };
        int[] dy = { 1, -1, -1, 1 };
        int[] dz = { 1, -1, -1, 1 };
        for (int i = 0; i < 4; i++)
        {
            Transform tmpGrid = Instantiate(GridPlanePrefab,this.transform).transform;
            tmpGrid.name = "GridPlane_" + "X(" + dx[i] + ")Y(" + dy[i] + ")";
            tmpGrid.position = new Vector3(
                tmpGrid.localScale.x * dx[i] * 5f,
                tmpGrid.localScale.y * dy[i] * 5f,
                0);
            tmpGrid.Rotate(new Vector3(90, 0, 0));
        }
        for (int i = 0; i < 4; i++)
        {
            Transform tmpGrid = Instantiate(GridPlanePrefab, this.transform).transform;
            tmpGrid.name = "GridPlane_" + "X(" + dx[i] + ")Z(" + dz[i] + ")";
            tmpGrid.position = new Vector3(
                tmpGrid.localScale.x * dx[i] * 5f,
                0,
                tmpGrid.localScale.y * dz[i] * 5f);
            tmpGrid.Rotate(new Vector3(0, 0, 0));
        }
        for (int i = 0; i < 4; i++)
        {
            Transform tmpGrid = Instantiate(GridPlanePrefab, this.transform).transform;
            tmpGrid.name = "GridPlane_" + "Y(" + dy[i] + ")Z(" + dz[(i+1) % 4] + ")";
            tmpGrid.position = new Vector3(
                0,
                tmpGrid.localScale.x * dy[i] * 5f,
                tmpGrid.localScale.y * dz[(i + 1 ) % 4] * 5f);
            tmpGrid.Rotate(new Vector3(0, 0, 90));
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            for (int i = -5; i < 5; i++)
            {
                for (int j = -5; j < 5; j++)
                {
                    for (int k = -5; k < 5; k++)
                    {
                        FindObjectOfType<VectorGenerator>().CreateVector(0, 0, 0, i, j, k,"newVec"+i+"-"+j+"-"+k);
                    }
                }
            }
        }
    }
}
