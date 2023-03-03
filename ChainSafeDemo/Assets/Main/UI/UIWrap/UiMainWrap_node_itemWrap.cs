using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

///该文件为自动生成，请不要手动修改
///使用方式为双击打开UIPrefab，在Prefab编辑模式下右键菜单 点击 生成UIWrap文件 即可在Assets\S1\Scripts\UIWrap 下生成对应的UI文件

[DisallowMultipleComponent]
[ExecuteInEditMode]
public class UIMainWrap_node_itemWrap :  MonoBehaviour
{
    public UnityEngine.UI.Image  img_item_bg;
public UnityEngine.UI.Text  txt_word;
public UnityEngine.UI.Button  btn_enter;
public UnityEngine.UI.Text  txt_info;


#if UNITY_EDITOR
    void Reset()
    {
	img_item_bg = transform.GetChild(0).GetComponent<UnityEngine.UI.Image>();

txt_word = transform.GetChild(0).GetChild(0).GetComponent<UnityEngine.UI.Text>();

btn_enter = transform.GetChild(1).GetComponent<UnityEngine.UI.Button>();

txt_info = transform.GetChild(1).GetChild(0).GetComponent<UnityEngine.UI.Text>();


    }
#endif

}
