using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Boids : MonoBehaviour
{
    #region Variables & Initializer
    [Header("Boid Options")]
    [SerializeField] private BoidUnit boidUnitPrefab;
    [Range(5, 5000)]
    public int boidCount;
    [Range(10, 100)]
    public float spawnRange=30;
    public Vector2 speedRange;

    [Range(0, 10)]
    public float cohesionWeight=1;
    [Range(0, 10)]
    public float alignmentWeight=1;
    [Range(0, 10)]
    public float separationWeight = 1;

    [Range(0, 100)]
    public float boundsWeight = 1;
    [Range(0, 100)]
    public float obstacleWeight = 10;
    [Range(0, 10)]
    public float egoWeight = 1;


    public BoidUnit currUnit;
    private MeshRenderer boundMR;
    [SerializeField] LayerMask unitLayer;

    [Header("Camera")]
    [SerializeField] CinemachineVirtualCamera originCam;
    [SerializeField] CinemachineFreeLook unitCam;

    [Header("OPTIONAL")]
    public bool ShowBounds = false;
    public bool cameraFollowUnit = false;
    public bool randomColor = false;
    public bool blackAndWhite = false;
    public bool protectiveColor = false;
    public float enemyPercentage = 0.01f;
    public Color[] GizmoColors;

    void Start()
    {
        // Generate Boids
        boundMR = GetComponentInChildren<MeshRenderer>();
        for (int i = 0; i < boidCount; i++)
        {
            Vector3 randomVec = Random.insideUnitSphere;
            randomVec *= spawnRange;
            Quaternion randomRot = Quaternion.Euler(0, Random.Range(0, 360f), 0);
            BoidUnit currUnit = Instantiate(boidUnitPrefab, this.transform.position+ randomVec, randomRot);
            currUnit.transform.SetParent(this.transform);
            currUnit.InitializeUnit(this,Random.Range(speedRange.x, speedRange.y),i);
        }
    }
    #endregion

    void Update()
    {
        // Right Mouse button Down, Set the unit to currUnit
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * 1000, Color.red, 3f);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, unitLayer))
            {
                currUnit = hit.transform.GetComponent<BoidUnit>();
            }
            else
            {
                currUnit = null;
            }
        }

        // we can see curr Unit's neighbour & target Vector
        if (currUnit)
        {
            currUnit.DrawVectorGizmo(0);
        }

        // camera switch
        if (cameraFollowUnit && currUnit != null)
        {
            originCam.gameObject.SetActive(false);
            unitCam.gameObject.SetActive(true);
            unitCam.Follow = currUnit.transform;
            unitCam.LookAt = currUnit.transform;
        }
        else
        {
            originCam.gameObject.SetActive(true);
            unitCam.gameObject.SetActive(false);
        }

        // can see bounds
        boundMR.enabled = ShowBounds;
    }
}
