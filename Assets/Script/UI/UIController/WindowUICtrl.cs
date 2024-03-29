using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

/// <summary>
/// 窗口UI管理器
/// </summary>
public class WindowUICtrl : SingleTon<WindowUICtrl>
{
    private Dictionary<WindowUIType, UIWindowViewBase> m_DicWindow = new Dictionary<WindowUIType, UIWindowViewBase>();

    /// <summary>
    /// 已经打开的窗口数量
    /// </summary>
    public int OpenWindowCount
    {
        get
        {
            return m_DicWindow.Count;
        }
    }

    #region OpenWindow 打开窗口
    /// <summary>
    /// 打开窗口
    /// </summary>
    /// <param name="type">窗口类型</param>
    /// <returns></returns>
    public GameObject OpenWindow(WindowUIType type)
    {
        if (type == WindowUIType.None) return null;

        GameObject obj = null;
        //如果窗口不存在 则
        if (!m_DicWindow.ContainsKey(type))
        {
            // Debug.Log(string.Format("Panel_{0}", type.ToString()));
            //枚举的名称要和预设的名称对应
            obj = ResourceMgr.Instance.Load(ResourceMgr.ResourceType.UIWindow, string.Format("Panel_{0}", type.ToString()), cache: true);
            if (obj == null) return null;
            UIWindowViewBase windowBase = obj.GetComponent<UIWindowViewBase>();
            if (windowBase == null) return null;

            m_DicWindow.Add(type, windowBase);

            windowBase.CurrentUIType = type;
            RectTransform transParent = null;

            switch (windowBase.containerType)
            {
                case WindowUIContainerType.Center:
                    transParent = SceneUICtrl.Instance.CurrentUIScene.Container_Center;
                    break;
            }

            obj.transform.SetParent(transParent);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            obj.gameObject.SetActive(false);
            StartShowWindow(windowBase, true);
        }
        else
        {
            obj = m_DicWindow[type].gameObject;
        }

        return obj;
    }
    #endregion

    #region CloseWindow 关闭窗口
    /// <summary>
    /// 关闭窗口
    /// </summary>
    /// <param name="type"></param>
    public void CloseWindow(WindowUIType type)
    {
        if (m_DicWindow.ContainsKey(type))
        {
            StartShowWindow(m_DicWindow[type], false);
        }
    }
    #endregion

    #region StartShowWindow 开始打开窗口
    /// <summary>
    /// 开始打开窗口
    /// </summary>
    /// <param name="windowBase"></param>
    /// <param name="isOpen">是否打开</param>
    private void StartShowWindow(UIWindowViewBase windowBase, bool isOpen)
    {
        switch (windowBase.showStyle)
        {
            case WindowShowStyle.Normal:
                ShowNormal(windowBase, isOpen);
                break;
            case WindowShowStyle.CenterToBig:
                ShowCenterToBig(windowBase, isOpen);
                break;
            case WindowShowStyle.FromTop:
                ShowFromDir(windowBase, 0, isOpen);
                break;
            case WindowShowStyle.FromDown:
                ShowFromDir(windowBase, 1, isOpen);
                break;
            case WindowShowStyle.FromLeft:
                ShowFromDir(windowBase, 2, isOpen);
                break;
            case WindowShowStyle.FromRight:
                ShowFromDir(windowBase, 3, isOpen);
                break;
        }
    }
    #endregion

    #region 各种打开效果

    /// <summary>
    /// 正常打开
    /// </summary>
    /// <param name="windowBase"></param>
    /// <param name="isOpen"></param>
    private void ShowNormal(UIWindowViewBase windowBase, bool isOpen)
    {
        if (isOpen)
        {
            windowBase.gameObject.SetActive(false);
        }
        else
        {
            DestroyWindow(windowBase);
        }
    }

    /// <summary>
    /// 中间变大
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="isOpen"></param>
    private void ShowCenterToBig(UIWindowViewBase windowBase, bool isOpen)
    {
        windowBase.transform.localScale = Vector3.zero;//设置初始大小为0
        //SetAutoKill(false) 动画播放完毕后这个动画不会销毁，可以实现反向播放
        Tweener tweener = windowBase.transform.DOScale(1, windowBase.duration).Pause().SetAutoKill(false)
        .OnRewind(()=>
        {
            DestroyWindow(windowBase);
        });

        if (isOpen)
            //正向播放动画
            windowBase.transform.DOPlayForward();
            // tweener.PlayForward();
        else{
            //反向播放动画
            windowBase.transform.DOPlayBackwards();
            // tweener.PlayBackwards();
        }

        //组件激活
        windowBase.gameObject.SetActive(true);
    }

    /// <summary>
    /// 从不同的方向加载
    /// </summary>
    /// <param name="windowBase"></param>
    /// <param name="dirType">0=从上 1=从下 2=从左 3=从右</param>
    /// <param name="isOpen"></param>
    private void ShowFromDir(UIWindowViewBase windowBase, int dirType, bool isOpen)
    {
        windowBase.gameObject.SetActive(true);//激活组件
        windowBase.transform.localPosition=Vector3.zero;

        Vector3 dir=Vector3.zero;
        //判断方向
        if(dirType==0)
            dir=new Vector3(0,1000,0);
        else if(dirType==1)
            dir=new Vector3(0,-1000,0);
        else if(dirType==2)
            dir=new Vector3(-1000,0,0);
        else 
            dir=new Vector3(1000,0,0);
        
        windowBase.transform.localPosition+=dir;

        //播放动画  
        windowBase.transform.DOLocalMove(Vector3.zero,windowBase.duration).SetAutoKill(false).Pause()
        .OnRewind(()=>
        {
            DestroyWindow(windowBase);
        });

        //判断动画类型
        if(isOpen)
            windowBase.transform.DOPlayForward();
        else 
            windowBase.transform.DOPlayBackwards();
    }

    #endregion

    #region DestroyWindow 销毁窗口
    /// <summary>
    /// 销毁窗口
    /// </summary>
    /// <param name="obj"></param>
    private void DestroyWindow(UIWindowViewBase windowBase)
    {
        m_DicWindow.Remove(windowBase.CurrentUIType);//从表中移除
        Object.Destroy(windowBase.gameObject);//销毁表
    }
    #endregion
}