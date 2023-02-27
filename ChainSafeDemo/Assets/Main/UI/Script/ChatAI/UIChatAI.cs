using System;
using PartySystems.UIParty;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using GraphQlClient.Core;
using Jint;
using Main.Logic.ChatAI;
using Main.Logic.Player;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Util;

public class UIChatAI : UIBase
{

    private UIChatAIWrap wrap;

    public override void OnInit()
    {
        wrap = transform.GetComponent<UIChatAIWrap>();

        wrap.btn_enter.onClick.RemoveListener(OnClickSend);
        wrap.btn_enter.onClick.AddListener(OnClickSend);
    }

    void Start()
    {

    }

    void OnClickSend()
    {
        ChatAIPostData _postData = new ChatAIPostData
        {
            model = "text-davinci-003",
            prompt = wrap.txt_input_text.text,
            temperature = 0.7f,
            max_tokens = 512,
            top_p = 1,
            frequency_penalty = 0,
            presence_penalty = 0
        };
        // GetPostData(_postData,"", (answer) =>
        // {
        //     string result = answer;
        // });
        StartCoroutine(LChatAI.I.GetPostData(_postData, "", (answer) =>
        {
            string result = answer;
            StartCoroutine(RefreshTxt(result));

        }));

    }

    private IEnumerator RefreshTxt(string text)
    {
        wrap.txt_word.text = text;
        yield return null;
    }


}
