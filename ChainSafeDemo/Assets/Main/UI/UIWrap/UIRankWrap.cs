using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

///该文件为自动生成，请不要手动修改
///使用方式为双击打开UIPrefab，在Prefab编辑模式下右键菜单 点击 生成UIWrap文件 即可在Assets\S1\Scripts\UIWrap 下生成对应的UI文件

[DisallowMultipleComponent]
[ExecuteInEditMode]
public class UIRankWrap :  MonoBehaviour
{
    public UnityEngine.UI.Text  txt_title;
public UnityEngine.UI.Button  btn_close;
public UnityEngine.Transform  tr_title;
public UnityEngine.Transform  tr_followed_list;
public UIRankWrap_node_itemWrap  node_item;


#if UNITY_EDITOR
    void Reset()
    {
	txt_title = transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<UnityEngine.UI.Text>();

btn_close = transform.GetChild(0).GetChild(1).GetChild(1).GetComponent<UnityEngine.UI.Button>();

tr_title = transform.GetChild(0).GetChild(2).GetComponent<UnityEngine.Transform>();

tr_followed_list = transform.GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetComponent<UnityEngine.Transform>();

node_item = transform.GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetOrAddComponent<UIRankWrap_node_itemWrap>();

    }
#endif

}
