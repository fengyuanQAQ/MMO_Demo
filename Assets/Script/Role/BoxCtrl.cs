using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BoxCtrl : MonoBehaviour
{
    
    public System.Action<GameObject> onHit;//一个参数为GameObject的委托
    
    //点击事件
    public void Hit()
    {
        //如果绑定了事件
        if(onHit!=null)
            onHit(gameObject);
    }
}
