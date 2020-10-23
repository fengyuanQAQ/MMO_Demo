
using UnityEngine;
using System.Collections;

/// <summary>
/// 场景UI管理器
/// </summary>
public class SceneUICtrl: SingleTon<SceneUICtrl>
{

    /// <summary>
    /// 场景UI类型
    /// </summary>
    public enum SceneUIType
    {
        /// <summary>
        /// 登录
        /// </summary>
        LogOn,
        /// <summary>
        /// 加载
        /// </summary>
        Loading,
        /// <summary>
        /// 主城
        /// </summary>
        MainCity
    }

    /// <summary>
    /// 当前场景UI
    /// </summary>
    public UISceneViewBase CurrentUIScene;

    #region LoadSceneUI 加载场景UI
    /// <summary>
    /// 加载场景UI
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public GameObject LoadSceneUI(SceneUIType type)
    {
        GameObject obj = null;
        switch (type)
        {
            case SceneUIType.LogOn:
                obj = ResourceMgr.Instance.Load(ResourceMgr.ResourceType.UIScene, "UI_Root_LogOn");
                CurrentUIScene = obj.GetComponent<UISceneLogonCtrl>();
                break;
            case SceneUIType.Loading:
                break;
            case SceneUIType.MainCity:
                obj = ResourceMgr.Instance.Load(ResourceMgr.ResourceType.UIScene, "UI Root_City");
                CurrentUIScene = obj.GetComponent<UISceneCityCtrl>();
                break;
        }
        return obj;
    }
    #endregion
}