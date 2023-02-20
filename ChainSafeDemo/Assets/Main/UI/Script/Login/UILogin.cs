using System;
using PartySystems.UIParty;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using GraphQlClient.Core;
using Jint;
using Main.Logic.Player;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Util;

public class UILogin : UIBase
{
    private UILoginWrap wrap;
    public WebLogin webLogin;
   

    
    public override void OnInit()
    {
        wrap = transform.GetComponent<UILoginWrap>();
    }

    public void SetWebLogin(WebLogin _webLogin)
    {
        webLogin = _webLogin;
        if (LPlayer.I.Address != "")
        {
            webLogin.OnLogin();
        }
        wrap.btn_login.onClick.RemoveListener(webLogin.OnLogin);
        wrap.btn_login.onClick.AddListener(webLogin.OnLogin);
       
    }
    // public void OnLogin()
    // {
    //     // Web3Connect();
    //     // OnConnected();
    // }
    //
    // async private void OnConnected()
    // {
    //     account = ConnectAccount();
    //     while (account == "") {
    //         await new WaitForSeconds(1f);
    //         account = ConnectAccount();
    //     };
    //     // save account for next scene
    //     PlayerPrefs.SetString("Account", account);
    //     // reset login message
    //     SetConnectAccount("");
    //     // load next scene
    //     SceneManager.LoadScene("Main");
    //     // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    // }
    //
    // public void OnSkip()
    // {
    //     // burner account for skipped sign in screen
    //     PlayerPrefs.SetString("Account", "");
    //     // move to next scene
    //     SceneManager.LoadScene("Main");
    //     // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    // }
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
      
    }

}
