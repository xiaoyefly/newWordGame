using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

///该文件为自动生成，请不要手动修改
///使用方式为双击打开UIPrefab，在Prefab编辑模式下右键菜单 点击 生成UIWrap文件 即可在Assets\S1\Scripts\UIWrap 下生成对应的UI文件

[DisallowMultipleComponent]
[ExecuteInEditMode]
public class UILoginWrap :  MonoBehaviour
{
    public UnityEngine.Transform  tr_main;
public UnityEngine.UI.Text  txt_title;
public UnityEngine.UI.Button  btn_info;
public UnityEngine.UI.Text  txt_info;
public UnityEngine.Transform  tr_content;
public UnityEngine.UI.Text  txt_login_title;
public UnityEngine.UI.Button  btn_login;
public UnityEngine.UI.Text  txt_login;


#if UNITY_EDITOR
    void Reset()
    {
	tr_main = transform.GetChild(0).GetChild(1).GetComponent<UnityEngine.Transform>();

txt_title = transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetComponent<UnityEngine.UI.Text>();

btn_info = transform.GetChild(0).GetChild(1).GetChild(0).GetChild(1).GetComponent<UnityEngine.UI.Button>();

txt_info = transform.GetChild(0).GetChild(1).GetChild(0).GetChild(1).GetChild(0).GetComponent<UnityEngine.UI.Text>();

tr_content = transform.GetChild(0).GetChild(1).GetChild(1).GetComponent<UnityEngine.Transform>();

txt_login_title = transform.GetChild(0).GetChild(1).GetChild(1).GetChild(0).GetComponent<UnityEngine.UI.Text>();

btn_login = transform.GetChild(0).GetChild(1).GetChild(1).GetChild(1).GetComponent<UnityEngine.UI.Button>();

txt_login = transform.GetChild(0).GetChild(1).GetChild(1).GetChild(1).GetChild(0).GetComponent<UnityEngine.UI.Text>();


    }
#endif

}
