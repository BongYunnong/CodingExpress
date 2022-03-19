using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
public class SortManager : MonoBehaviour
{
    #region Variables & Initializer
    [SerializeField] Character character;

    [SerializeField] Slider speedSlider;
    [SerializeField] Slider countSlider;
    [SerializeField] Slider spacingSlider;
    [SerializeField] Text sortModeTxt;

    [SerializeField] List<CustomSort> SortLists;
    [SerializeField] int sortIndex=-1;

    bool camMode = true;
    [SerializeField] GameObject canvas;
    void Start()
    {
        AddSortIndex(1+Random.Range(0, SortLists.Count));
    }
    #endregion

    #region SORT
    public void StartSort()
    {
        if (SortLists[sortIndex].sorting == false)
        {
            SortLists[sortIndex].StopSort();
            SortLists[sortIndex].Sort();
            SortLists[sortIndex].SetTarget(character);
        }
    }
    public void StopSort()
    {
        SortLists[sortIndex].StopSort();
    }

    void Update()
    {
        // Set Sort Speed
        SortLists[sortIndex].SetSpeed(speedSlider.value);

        // UI On/Off
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            canvas.SetActive(!canvas.activeInHierarchy);
        }
        if (Input.GetMouseButtonDown(1))
        {
            StartSort();
        }

        // Camera
        CinemachineVirtualCamera tmpVC = Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
        tmpVC.transform.position += new Vector3(input.x, 0f, input.y) * Time.deltaTime * 10f;
        if (Input.GetKey(KeyCode.Q))
            tmpVC.transform.position += Vector3.up * -Time.deltaTime * 10f;
        else if(Input.GetKey(KeyCode.E))
            tmpVC.transform.position += Vector3.up * Time.deltaTime * 10f;

        if (Input.GetKeyDown(KeyCode.Space)) {
            camMode = !camMode;
        }
        if (camMode)
        {
            Vector3 rot = tmpVC.transform.eulerAngles - new Vector3(Input.GetAxisRaw("Mouse ScrollWheel") * Time.deltaTime * 1200, 0, 0);
            rot.x = ClampAngle(rot.x, 0, 45f);
            tmpVC.transform.eulerAngles = rot;
        }
        else
        {
            tmpVC.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * 1200;
            tmpVC.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView = Mathf.Clamp(tmpVC.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView, 5, 80);
        }

    }
    float ClampAngle(float angle, float from, float to)
    {
        // accepts e.g. -80, 80
        if (angle < 0f) angle = 360 + angle;
        if (angle > 180f) return Mathf.Max(angle, 360 + from);
        return Mathf.Min(angle, to);
    }
    #endregion

    #region Settings
    public void AddSortIndex(int _add)
    {
        int pastIndex = sortIndex;
        sortIndex = sortIndex + _add;
        sortIndex = Mathf.Clamp(sortIndex ,0, SortLists.Count-1);
        sortModeTxt.text = SortLists[sortIndex].name;

        if (pastIndex != sortIndex)
        {
            if (pastIndex != -1)
            {
                SortLists[pastIndex].StopSort();
                SortLists[pastIndex].DiscardSort();
            }
            SortLists[sortIndex].InitializeSort((int)countSlider.value);
            SetSpacing(spacingSlider.value);
        }
    }

    public void SetSpacing(float _spacing)
    {
        SortLists[sortIndex].SetSpacing(_spacing);
    }
    public void SetCount(float _count)
    {
        if (SortLists[sortIndex].sorting == false)
        {
            SortLists[sortIndex].SetCount((int)_count);
            Randomize();
        }
    }

    public void Randomize()
    {
        if (SortLists[sortIndex].sorting == false)
        {
            SortLists[sortIndex].Randomize();
        }
    }
    #endregion
}
