using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleCamera : MonoBehaviour
{
    private CameraCtrl cameraCtrl;//相机脚本

    private CameraState cameraState;
    private CameraDirection cameraDirection;

    private bool isCameraChange = false;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Start()
    {
        cameraCtrl = CameraCtrl.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        KeyDetection();
        SetCamera();
    }

    private void SetCamera()
    {
        if (cameraCtrl == null)
        {
            Debug.Log("不存在相机脚本");
            return;
        }

        //设置相机跟随主角
        cameraCtrl.transform.position=transform.position;
        //看着主角
        cameraCtrl.m_CameraZoomContaner.transform.LookAt(transform.position);

        if (isCameraChange)
            cameraCtrl.SetCameraState(cameraState, cameraDirection);

    }

    private void KeyDetection()
    {
        isCameraChange=false;
        if (Input.GetKey(KeyCode.A))
        {
            cameraState = CameraState.ROTATE;
            cameraDirection = CameraDirection.FORWARD;
            isCameraChange=true;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            cameraState = CameraState.ROTATE;
            cameraDirection = CameraDirection.OPPOSITE;
            isCameraChange=true;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            cameraState = CameraState.UPANDDOWN;
            cameraDirection = CameraDirection.FORWARD;
            isCameraChange=true;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            cameraState = CameraState.UPANDDOWN;
            cameraDirection = CameraDirection.OPPOSITE;
            isCameraChange=true;
        }
        else if (Input.GetKey(KeyCode.J))
        {
            cameraState = CameraState.ZOOM;
            cameraDirection = CameraDirection.FORWARD;
            isCameraChange=true;
        }
        else if (Input.GetKey(KeyCode.K))
        {
            cameraState = CameraState.ZOOM;
            cameraDirection = CameraDirection.OPPOSITE;
            isCameraChange=true;
        }
    }
}
