using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

///该文件为自动生成，请不要手动修改
///使用方式为双击打开UIPrefab，在Prefab编辑模式下右键菜单 点击 生成UIWrap文件 即可在Assets\S1\Scripts\UIWrap 下生成对应的UI文件

[DisallowMultipleComponent]
[ExecuteInEditMode]
public class UIMainWrap :  MonoBehaviour
{
    public UnityEngine.Transform  tr_title_top;
public UnityEngine.UI.Text  txt_title;
public UnityEngine.UI.Button  btn_info;
public UnityEngine.UI.Text  txt_info;
public UnityEngine.Transform  tr_main;
public UnityEngine.Transform  tr_game_panel;
public UnityEngine.Transform  tr_game;
public UnityEngine.Transform  tr_restart;
public UnityEngine.UI.Text  txt_restart_title;
public UnityEngine.UI.Button  btn_restart;
public UnityEngine.UI.Text  txt_restart_btn_title;
public UnityEngine.UI.Button  btn_rank;
public UnityEngine.UI.Text  txt_rank_title;
public UnityEngine.Transform  tr_button_panel;
public UnityEngine.Transform  tr_button;
public UnityEngine.UI.Button  btn_my_follow;
public UnityEngine.UI.Text  txt_follow_title;
public UnityEngine.UI.Button  btn_chat;
public UnityEngine.UI.Text  txt_chat_title;
public UIMainWrap_node_itemWrap  node_item;


#if UNITY_EDITOR
    void Reset()
    {
	tr_title_top = transform.GetChild(0).GetChild(1).GetComponent<UnityEngine.Transform>();

txt_title = transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetComponent<UnityEngine.UI.Text>();

btn_info = transform.GetChild(0).GetChild(1).GetChild(0).GetChild(1).GetComponent<UnityEngine.UI.Button>();

txt_info = transform.GetChild(0).GetChild(1).GetChild(0).GetChild(1).GetChild(0).GetComponent<UnityEngine.UI.Text>();

tr_main = transform.GetChild(0).GetChild(2).GetComponent<UnityEngine.Transform>();

tr_game_panel = transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<UnityEngine.Transform>();

tr_game = transform.GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetComponent<UnityEngine.Transform>();

tr_restart = transform.GetChild(0).GetChild(2).GetChild(1).GetComponent<UnityEngine.Transform>();

txt_restart_title = transform.GetChild(0).GetChild(2).GetChild(1).GetChild(0).GetComponent<UnityEngine.UI.Text>();

btn_restart = transform.GetChild(0).GetChild(2).GetChild(1).GetChild(1).GetComponent<UnityEngine.UI.Button>();

txt_restart_btn_title = transform.GetChild(0).GetChild(2).GetChild(1).GetChild(1).GetChild(0).GetComponent<UnityEngine.UI.Text>();

btn_rank = transform.GetChild(0).GetChild(2).GetChild(2).GetComponent<UnityEngine.UI.Button>();

txt_rank_title = transform.GetChild(0).GetChild(2).GetChild(2).GetChild(0).GetComponent<UnityEngine.UI.Text>();

tr_button_panel = transform.GetChild(0).GetChild(2).GetChild(3).GetComponent<UnityEngine.Transform>();

tr_button = transform.GetChild(0).GetChild(2).GetChild(3).GetChild(1).GetComponent<UnityEngine.Transform>();

btn_my_follow = transform.GetChild(0).GetChild(2).GetChild(3).GetChild(1).GetChild(0).GetComponent<UnityEngine.UI.Button>();

txt_follow_title = transform.GetChild(0).GetChild(2).GetChild(3).GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetComponent<UnityEngine.UI.Text>();

btn_chat = transform.GetChild(0).GetChild(2).GetChild(3).GetChild(1).GetChild(1).GetComponent<UnityEngine.UI.Button>();

txt_chat_title = transform.GetChild(0).GetChild(2).GetChild(3).GetChild(1).GetChild(1).GetChild(0).GetChild(0).GetComponent<UnityEngine.UI.Text>();

node_item = transform.GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetOrAddComponent<UIMainWrap_node_itemWrap>();

    }
#endif

}
