using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float cameraMoveSpeed = 5f;
    [SerializeField] Material gridMat;
    private void Update()
    {
        CameraMoveFunc();

        gridMat.SetVector("_PlayerPosition", this.transform.position);
    }

    private void CameraMoveFunc()
    {
        //  Forward / Right Input
        Vector3 input = (transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal")).normalized * cameraMoveSpeed * Time.deltaTime;

        float wheelInput = Input.GetAxis("Mouse ScrollWheel");
        float height = 0;

        // Up / Down Input
        if (Input.GetKey(KeyCode.Q))
            height = -cameraMoveSpeed * Time.deltaTime;
        else if (Input.GetKey(KeyCode.E))
            height = cameraMoveSpeed * Time.deltaTime;

        if (input.magnitude > 0 || height != 0)
            transform.position += (new Vector3(input.x, height, input.z));

        if (wheelInput != 0)
            transform.Rotate(new Vector3(-wheelInput * 20f, 0, 0));

        // If Mouse Wheel is pressed, player can Rotate Camera
        if (Input.GetMouseButton(1))
        {
            float sensitivity = 10f;
            transform.localEulerAngles = transform.rotation.eulerAngles + new Vector3(-Input.GetAxis("Mouse Y") * sensitivity, Input.GetAxis("Mouse X") * sensitivity, 0f);
        }
    }
}
