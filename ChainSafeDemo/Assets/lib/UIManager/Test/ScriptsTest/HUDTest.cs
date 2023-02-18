using PartySystems.UIParty;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDTest : UIBase
{
    public void OpenView1()
    {
        var view1 = UIManager.Instance.RequestView<BaseTest1>(UIManager.EViewPriority.MediumRenderPriority);
        view1.Reference.ShowView();
    }

    public void OpenView2()
    {
        var view2 = UIManager.Instance.RequestView<BaseTest2>(UIManager.EViewPriority.HighRenderPriority);
        view2.Reference.ShowView();
    }
}
