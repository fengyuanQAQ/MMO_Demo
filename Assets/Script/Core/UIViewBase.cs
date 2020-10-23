
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 所有UI的基类
/// </summary>
public class UIViewBase : MonoBehaviour 
{
    void Awake()
    {
        OnAwake();
    }

    void Start()
    {
        Button[] btnArr = GetComponentsInChildren<Button>(true);
        for (int i = 0; i < btnArr.Length; i++)
        {
            EventTriggerListener.Get(btnArr[i].gameObject).onClick = BtnClick;
        }
        OnStart();
    }

    void OnDestroy()
    {
        BeforeOnDestroy();
    }

    /// <summary>
    /// 事件绑定
    /// </summary>
    /// <param name="按钮物体"></param>
    private void BtnClick(GameObject go)
    {
        OnBtnClick(go);
    }

    /// <summary>
    /// 虚方法 子类需要重写
    /// </summary>
    protected virtual void OnAwake() { }
    protected virtual void OnStart() { }
    protected virtual void BeforeOnDestroy() { }
    protected virtual void OnBtnClick(GameObject go) { }
}