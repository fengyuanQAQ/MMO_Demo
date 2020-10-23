using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountCtrl : SingleTon<AccountCtrl>
{
   public void OpenLogOnView()
   {
       WindowUICtrl.Instance.OpenWindow(WindowUIType.LogOn);
   }
}
