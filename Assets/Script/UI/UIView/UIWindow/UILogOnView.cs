using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILogOnView : UIWindowViewBase
{
    protected override void OnStart()
    {
        base.OnStart();
        StartCoroutine(OpenLogOnWindow());
    }

    private IEnumerator OpenLogOnWindow()
    {
        yield return new WaitForSeconds(.2f);

        //打开登陆窗口
        AccountCtrl.Instance.OpenLogOnView();
    }

}
