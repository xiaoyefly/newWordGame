using System;
using System.Collections;
using System.Collections.Generic;
using GraphQlClient.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEditor.ShaderGraph.Serialization;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Util;

public class LMainManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Text txt_wallet_address;
    public Transform node_item;
    public Transform tr_restart;
    public Button btn_restart;
    public Text txt_restart_title;
    // public Button btn_restart;
    public Text txt_score_val;
    public Button btn_query;
    private const int RowCount=5;//行数
    private const int ColCount=5;//列数

    private string AnswerStr = "APPLE";
    //格子的transform列表
    private Dictionary<int, Dictionary<int, Text>> girdDataDict = new Dictionary<int, Dictionary<int, Text>>();
    //每一行的文字数据
    private Dictionary<int, string> InputWordDict = new Dictionary<int, string>();
    //当前输入的行数
    private int curRow = 0;
    private int myScore = 0;
    void Awake()
    {
        txt_wallet_address.text = PlayerPrefs.GetString("Account");
        girdDataDict.Clear();
        InputWordDict.Clear();
        myScore=0;
        txt_score_val.text = "score:"+myScore.ToString();
        UIUtil.UpdateWithTransfrom(node_item, (trans1, index1) =>
        {
            Transform tr_grid = trans1.Find("tr_grid");
            girdDataDict[index1] = new Dictionary<int, Text>();
            UIUtil.UpdateWithTransfrom(tr_grid, (trans2, index2) =>
            {
                Text txt_word = trans2.Find("bg/txt_word").GetComponent<Text>();
                txt_word.text = "";
                girdDataDict[index1][index2] = txt_word;
            },ColCount);
        },RowCount);
        
        btn_restart.onClick.RemoveListener(OnClickRestart);
        btn_restart.onClick.AddListener(OnClickRestart);
        
        btn_query.onClick.RemoveListener(OnClickQuerry);
        btn_query.onClick.AddListener(OnClickQuerry);
    }
    
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        if (tr_restart.gameObject.activeSelf)
        {
            return;
        }
        if (Input.anyKeyDown) {
            foreach (char c in Input.inputString) {
                if (Char.IsLetter(c)) {
                    if (!InputWordDict.ContainsKey(curRow))
                    {
                        InputWordDict[curRow] = "";
                    }
                    if (InputWordDict[curRow].Length < ColCount)
                    {
                        InputWordDict[curRow] += c.ToString().ToUpper();
                        RefreshGrid(curRow);
                    }
                    // Debug.Log("You pressed the letter " + c);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (InputWordDict[curRow].Length >= ColCount)
            {
                //检查是否输入正确
                var isEnd=CheckGrid(curRow);

                if (isEnd)
                {
                    txt_restart_title.text = "You Win!!!";
                    tr_restart.gameObject.SetActive(true);
                    myScore++;
                    txt_score_val.text = "score:"+myScore.ToString();
                }
                else
                {
                    if (curRow >= RowCount-1 )
                    {
                        txt_restart_title.text = "Failed!Please restart game!";
                        tr_restart.gameObject.SetActive(true);
                    }
                    else
                    {
                        curRow += 1;  
                    }
                }
            }
        }
    }

    //刷新格子的word
    void RefreshGrid(int row)
    {
        string wordtempStr = "";
        if (InputWordDict.ContainsKey(row))
        {
            wordtempStr = InputWordDict[row];
        }
        if (girdDataDict.TryGetValue(row, out var rowGridTempDic))
        {
            foreach (var UPPER in rowGridTempDic)
            {
                if (wordtempStr.Length > UPPER.Key)
                {
                    UPPER.Value.text = wordtempStr[UPPER.Key].ToString();
                }
            }
        }
    }
    
    //检查某一行的格子
    bool CheckGrid(int row)
    {
        string wordtempStr = "";
        int currentNum = 0;
        if (InputWordDict.ContainsKey(row))
        {
            wordtempStr = InputWordDict[row];
        }
        if (girdDataDict.TryGetValue(row, out var rowGridTempDic))
        {
            foreach (var UPPER in rowGridTempDic)
            {
                if (wordtempStr.Length > UPPER.Key)
                {
                    if (wordtempStr[UPPER.Key] == AnswerStr[UPPER.Key])
                    { //位置文字都正确显示绿色
                        UPPER.Value.text = "<color=#11CB00>"+wordtempStr[UPPER.Key].ToString()+"</color>";
                        currentNum++;
                    }
                    else
                    {
                        bool haveWord = false;
                        foreach (var VARIABLE in AnswerStr)
                        {
                            if (VARIABLE == wordtempStr[UPPER.Key])
                            {
                                haveWord = true;
                                break;
                            }
                        }
                        if (haveWord)
                        { //文字正确位置不正确显示蓝色
                            UPPER.Value.text = "<color=#170EC6>"+wordtempStr[UPPER.Key].ToString()+"</color>";
                        }
                        else
                        { //文字位置都不正确显示红色
                            UPPER.Value.text = "<color=#B70011>"+wordtempStr[UPPER.Key].ToString()+"</color>";
                        }
                    }
                }
            }
        }
        return currentNum >= ColCount;
    }
    
    //点击重新开始
    void OnClickRestart()
    {
        foreach (var VARIABLE in girdDataDict)
        {
            foreach (var UPPER in VARIABLE.Value)
            {
                UPPER.Value.text = "";
                InputWordDict.Clear();
                curRow = 0;
            }
        }
        tr_restart.gameObject.SetActive(false);
    }
    
    //点击查询
    void OnClickQuerry()
    {
        GetPokemons();
    }
    
    public GraphApi pokemonReference;
    public async void GetPokemons(){
        //获得签证
        // GraphApi.Query createUser = pokemonReference.GetQueryByName("loginGetMessage", GraphApi.Query.Type.Mutation);
	       //
        // createUser.SetArgs(new{input = new{domain = "cyberconnect.me", address = "0x803F69aE5f5D839071fcD712e25BF3c8c35B2664"}});
	       //
        // //Performs Post request to server
        // UnityWebRequest request = await pokemonReference.Post(createUser);
        // \
        //获取关注
        GraphApi.Query createUser = pokemonReference.GetQueryByName("getFollowingsByAddressEVM", GraphApi.Query.Type.Query);
        
        createUser.SetArgs(new{address = "0x591e0850a4D19045388F37E5D1BA9be411b22a57"});
        
        //Performs Post request to server
        UnityWebRequest request = await pokemonReference.Post(createUser);

        if (!request.isNetworkError)
        {
            string introspection = request.downloadHandler.text;
            // var type = createUser.fields[0].GetType();
            
            JObject obj = JObject.Parse(introspection);

            Array followings = (Array)obj["data"]["address"];

            // Object schemaClass = JsonConvert.DeserializeObject<Object>(introspection);
            // var etrfd = schemaClass["followings"];
        }
        
    }
    
}
