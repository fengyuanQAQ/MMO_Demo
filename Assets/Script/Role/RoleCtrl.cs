using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleCtrl : MonoBehaviour
{
    //角色控制器
    private CharacterController characterController;

    //目标点
    private Vector3 mTargetPos = Vector3.zero;

    //目标转向
    private Quaternion mTargetQua = Quaternion.identity;

    private bool isMoveOver = false;//是否已经移动完成

    [Space]
    [SerializeField]
    //移动速度
    private float mMoveSpeed = 5.0f;
    [SerializeField]
    //旋转速度
    private float mRotateSpeed = 0.02f;



    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        
    }

    // Update is called once per frame
    void Update()
    {
        KeyDetection();//按键检测

        RoleMove();//人物移动
    }

    /// <summary>
    /// 按键检测
    /// </summary>
    void KeyDetection()
    {
        //鼠标右键点击判断
        if (Input.GetMouseButtonDown(1))
        {
            //获取摄像机发出的一条射线
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            //判断射线是否相交
            if (Physics.Raycast(ray, out hitInfo))
            {
                //判断该射线是否与地面相交
                if (hitInfo.collider.gameObject.name.Equals("Ground", System.StringComparison.CurrentCultureIgnoreCase))
                {
                    mTargetPos = hitInfo.point;
                    isMoveOver = false;
                    mRotateSpeed = 0.02f;
                }
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            //拾取单个物体
            #region 
            //鼠标左键拾取物体
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            //bitmask
            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, 1 << LayerMask.NameToLayer("Item")))
            {
                BoxCtrl boxCtrl = hitInfo.collider.gameObject.GetComponent<BoxCtrl>();
                if (boxCtrl != null)
                    boxCtrl.Hit();
            }
            #endregion

            //拾取多个物体
            #region 
            //当前角色发出一条射线
            // Collider[] colliders= Physics.OverlapSphere(transform.position,4f,1<<LayerMask.NameToLayer("Item"));
            // if(colliders.Length>0)
            // {
            //     foreach (var item in colliders)
            //     {
            //         Debug.Log("找到了"+item.gameObject.name);
            //     }
            // }
            #endregion
        }

    }

    /// <summary>
    /// 人物移动和转向
    /// </summary>
    void RoleMove()
    {
        if (characterController == null)
            return;

        // 如果角色不在地面
        if (!characterController.isGrounded)
        {
            characterController.Move((transform.position + new Vector3(0, -1000f, 0)) - transform.position);
        }

        if (mTargetPos != Vector3.zero)
        {
            //求得方向
            Vector3 dir = (mTargetPos - transform.position).normalized;
            dir.y = 0;//这个方向不能有y上的偏移

            if (!isMoveOver)
            {
                //移动
                if (Vector3.Distance(transform.position, mTargetPos) > 0.3f)
                {
                    // transform.LookAt(new Vector3(mTargetPos.x,transform.position.y,mTargetPos.z));
                    characterController.Move(dir * Time.deltaTime * mMoveSpeed);
                }
                else
                {
                    isMoveOver = true;
                    mRotateSpeed = 1f;//重置人物旋转速度
                }
            }

            // if (Quaternion.Angle(transform.rotation, mTargetQua) > 1f)
            if (mRotateSpeed < 1 && !isMoveOver)//旋转速度<1的时候才会才会
            {
                //转向
                mTargetQua = Quaternion.LookRotation(dir);    //求得目标转向
                // Debug.Log(mRotateSpeed);
                mRotateSpeed += 2 * Time.deltaTime;
                //缓慢移动
                transform.rotation = Quaternion.Slerp(transform.rotation, mTargetQua, mRotateSpeed);
            }

        }
    }

    /// <summary>
    /// Callback to draw gizmos that are pickable and always drawn.
    /// </summary>
    // void OnDrawGizmos()
    // {
    //     // 画一个范围
    //     Gizmos.color=Color.blue;
    //     Gizmos.DrawWireSphere(transform.position,4);
    // }



}
