using UnityEngine;
using System.Text;
using System.Collections;
using System;

/// <summary>
/// 1、场景资源管理，只需要传递资源的类型和名字就可以返回对应的资源对象
/// 2、缓存表存储加载过的资源
/// 3、当切换场景的时候 释放不需要的资源
/// </summary>
public class ResourceMgr : SingleTon<ResourceMgr>,IDisposable
{
    private StringBuilder m_stringBulier = new StringBuilder();//记录资源对应的路径
    private Hashtable m_ResourceHB = new Hashtable();

    public enum ResourceType
    {
        EffectPrefab,
        RolePrefab,
        UIScene,
        UIWindow,
    }

    #region 获取资源 加载资源
    public GameObject Load(ResourceType type, string name, bool cache = false)
    {

        GameObject gameObject = null;
        //判断哈希表中是否有这个物体
        if (m_ResourceHB.Contains(m_stringBulier.ToString()))
        {
            //如果有
            gameObject = m_ResourceHB[m_stringBulier.ToString()] as GameObject;
            // Debug.Log("from cache");
        }
        else
        {
            //如果没有
            m_stringBulier.Clear();
            switch (type)
            {
                case ResourceType.EffectPrefab:
                    m_stringBulier.Append("EffectPrefab/");
                    break;
                case ResourceType.RolePrefab:
                    m_stringBulier.Append("RolePrefab/");
                    break;
                case ResourceType.UIScene:
                    m_stringBulier.Append("UIPrefab/UIScene/");
                    break;
                case ResourceType.UIWindow:
                    m_stringBulier.Append("UIPrefab/UIWindow/");
                    break;
            }
            m_stringBulier.Append(name);
            //加载物体
            gameObject = Resources.Load(m_stringBulier.ToString()) as GameObject;
            // Debug.Log("from myself");
            //判断是否缓存
            if (cache)
            {
                m_ResourceHB.Add(m_stringBulier.ToString(), gameObject);
            }
        }
        //初始化对象
        return GameObject.Instantiate(gameObject);
    }
    #endregion

    /// <summary>
    /// 切换场景的时候 清空缓存，销毁不需要的物体 
    /// </summary>
    public void Dispose()
    {
        m_ResourceHB.Clear();

        //清空不需要缓存在Resouces池里面的物体
        Resources.UnloadUnusedAssets();
    }
    
}
