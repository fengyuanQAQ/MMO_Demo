using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 控制物体生成
/// </summary>
public class textCtrl : MonoBehaviour
{
    [Space]
    private int m_currentBoxes = 0;//当前箱子数量
    private int m_maxBoxes = 10;//最大箱子数量
    private float m_intervalTime = 1f;//箱子生成时间
    private float m_genTimer = 0f;//箱子生成计时器

    [Space]
    //需要实例化的箱子
    private GameObject boxGameObj;

    private int aquiredBox;

    // Start is called before the first frame update
    void Start()
    {
        //加载物体
        boxGameObj = Resources.Load<GameObject>("Role/Items/Box");
        aquiredBox = PlayerPrefs.GetInt("boxNum", 0);
    }

    // Update is called once per frame
    void Update()
    {
        GenerateBox();
        // Debug.Log("当前已获得箱子数:"+aquiredBox);
    }

    void GenerateBox()
    {
        m_genTimer += Time.deltaTime;
        if (m_genTimer >= m_intervalTime && m_currentBoxes < m_maxBoxes)
        {
            GameObject boxGo = Instantiate(boxGameObj) as GameObject;
            boxGo.transform.SetParent(transform);
            //这里TransformPoint里面填的坐标为相对坐标，相对这个transform所变换的位置，这个坐标要受到scale的影响，
            //如果相对偏移为0.5那么最后相对偏移为0.5*20为10，已知y轴方向的缩放倍数为100，如果想要相对其向上偏移0.5
            //那么需要+50才能保证相对偏移了0.5
            boxGo.transform.position = transform.TransformPoint(new Vector3(Random.Range(-0.475f, 0.475f),
                transform.position.y + 50, Random.Range(-0.475f, 0.475f)));

            //设置委托
            if (boxGo.GetComponent<BoxCtrl>() != null)
            {
                //1 BoxCtrl事件的拥有者
                //2 事件 onHit
                //3 事件响应者 this
                //4 事件处理器:BoxHit
                //5 订阅事件 =
                boxGo.GetComponent<BoxCtrl>().onHit = (go) =>
                {
                    m_currentBoxes--;
                    aquiredBox++;
                    Destroy(go);
                    //存储当前获得箱子数
                    PlayerPrefs.SetInt("boxNum", aquiredBox);
                };
            }

            m_genTimer = 0;
            m_currentBoxes++;
        }
    }

    //点击事件
    void BoxHit(GameObject go)
    {
        m_currentBoxes--;
        aquiredBox++;
        Destroy(go);
        //存储当前获得箱子数
        PlayerPrefs.SetInt("boxNum", aquiredBox);
    }

}
