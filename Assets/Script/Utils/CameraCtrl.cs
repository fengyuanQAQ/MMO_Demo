using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraCtrl : MonoBehaviour
{
    [SerializeField]
    //控制上下视野的摄像机
    public Transform m_CameraUpAndDown;
    [SerializeField]
    //控制缩放的摄像机
    public Transform m_CameraZoomContaner;
    [SerializeField]
    //摄像机容器
    public Transform m_CameraContainer;

    public static CameraCtrl Instance;

    [Space]
    [SerializeField]
    private float m_UpAndDownSpeed = 10f;
    [SerializeField]
    private float m_RotateSpeed = 10f;
    [SerializeField]
    private float m_ZoomSpeed = 10f;


    void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        m_CameraUpAndDown.transform.localEulerAngles = new Vector3(
       0, 0, (Mathf.Clamp(m_CameraUpAndDown.transform.localEulerAngles.z, 10, 40)));
       //初始化摄像机的初始位置
    }

    /// <summary>
    /// Callback to draw gizmos that are pickable and always drawn.
    /// </summary>
    void OnDrawGizmos()
    {
        // Gizmos.color = Color.red;
        // Gizmos.DrawWireSphere(transform.position, 15);

        // Gizmos.color = Color.green;
        // Gizmos.DrawWireSphere(transform.position, 12f);

    }

    public void SetCameraState(CameraState state, CameraDirection direction)
    {
        switch (state)
        {
            case CameraState.ROTATE:
                CameraRotate(direction);
                break;
            case CameraState.UPANDDOWN:
                CameraUD(direction);
                break;
            case CameraState.ZOOM:
                CameraZoom(direction);
                break;
        }
    }

    private void CameraRotate(CameraDirection direction)
    {
        transform.Rotate(new Vector3(0, m_RotateSpeed * Time.deltaTime * (direction == CameraDirection.FORWARD ?
        1 : -1), 0));
    }

    private void CameraUD(CameraDirection direction)
    {
        m_CameraUpAndDown.transform.Rotate(0, 0, m_UpAndDownSpeed * Time.deltaTime * (direction == CameraDirection.FORWARD ?
        1 : -1));
        //限制俯仰高度
        m_CameraUpAndDown.transform.localEulerAngles = new Vector3(
        0, 0, (Mathf.Clamp(m_CameraUpAndDown.transform.localEulerAngles.z, 10, 40)));
    }

    private void CameraZoom(CameraDirection direction)
    {
        //z轴缩放 z越大越近
        m_CameraContainer.transform.Translate(0,0,m_ZoomSpeed*Time.deltaTime*
        (direction == CameraDirection.FORWARD ?-1 : 1));
        //限制最大的缩放距离
        m_CameraContainer.transform.localPosition=new Vector3(m_CameraContainer.transform.localPosition.x,
        m_CameraContainer.transform.localPosition.y,Mathf.Clamp(m_CameraContainer.transform.localPosition.z,-5,5f));
    }

}
