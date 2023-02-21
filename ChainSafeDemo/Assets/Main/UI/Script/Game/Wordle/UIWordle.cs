using System;
using PartySystems.UIParty;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using GraphQlClient.Core;
using Jint;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Util;

public class UIWordle : UIBase
{
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

    private UIWordleWrap wrap;
    public override void OnInit()
    {
        wrap = transform.GetComponent<UIWordleWrap>();
        
        wrap.txt_wallet_address.text = PlayerPrefs.GetString("Account");
        girdDataDict.Clear();
        InputWordDict.Clear();
        myScore=0;
        wrap.txt_score_val.text = "score:"+myScore.ToString();
        UIUtil.UpdateWithTransfrom(wrap.node_item.transform, (trans1, index1) =>
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
        
        wrap.btn_restart.onClick.RemoveListener(OnClickRestart);
        wrap.btn_restart.onClick.AddListener(OnClickRestart);
        
        wrap.btn_my_follow.onClick.RemoveListener(OnClickQuerry);
        wrap.btn_my_follow.onClick.AddListener(OnClickQuerry);
    }
    
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        if (wrap.tr_restart.gameObject.activeSelf)
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
                    wrap.txt_restart_title.text = "You Win!!!";
                    wrap.tr_restart.gameObject.SetActive(true);
                    myScore++;
                    wrap.txt_score_val.text = "score:"+myScore.ToString();
                }
                else
                {
                    if (curRow >= RowCount-1 )
                    {
                        wrap.txt_restart_title.text = "Failed!Please restart game!";
                        wrap.tr_restart.gameObject.SetActive(true);
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
        wrap.tr_restart.gameObject.SetActive(false);
    }
    
    //点击查询
    void OnClickQuerry()
    {
        var hud = UIManager.Instance.GetHUD<UIRank>(UIManager.EViewPriority.HighRenderPriority);
        hud.Reference.ShowView();
        // GetPokemons();
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
        
        
        // //获取关注
        // GraphApi.Query createUser = pokemonReference.GetQueryByName("getFollowingsByAddressEVM", GraphApi.Query.Type.Query);
        //
        // createUser.SetArgs(new{address = "0x591e0850a4D19045388F37E5D1BA9be411b22a57"});
        //
        // //Performs Post request to server
        // UnityWebRequest request = await pokemonReference.Post(createUser);
        //
        // if (!request.isNetworkError)
        // {
        //     string introspection = request.downloadHandler.text;
        //     // var type = createUser.fields[0].GetType();
        //     
        //     JObject obj = JObject.Parse(introspection);
        //
        //     Array followings = (Array)obj["data"]["address"];
        //
        //     // Object schemaClass = JsonConvert.DeserializeObject<Object>(introspection);
        //     // var etrfd = schemaClass["followings"];
        // }
        
        
      //   Engine engine1 = new Engine();
      //   engine1.SetValue("log", new Action<object>(msg => Debug.Log(msg)));
      //
      //   engine1.Execute(@"
      //   var myVariable = 108;
      //   log('Hello from Javascript! myVariable = '+myVariable);
      // ");
        
        Engine engine = new Engine();
        // 在引擎中执行 JavaScript 代码
        // string followStr="cyberConnect.follow('0xD790D1711A9dCb3970F47fd775f2f9A2f0bCc348', handle);"
//         engine.Execute(File.ReadAllText($"Assets/lib/cyberConnect/node_modules/@cyberlab/cyberconnect-v2/lib/cyberConnect.js")); 
//          engine.Execute(@"
// const CyberConnect = require('@cyberlab/cyberconnect-v2').default;
//
// const { Env } = require('@cyberlab/cyberconnect-v2');
//
// const cyberConnect = new CyberConnect({
//     namespace: 'CyberConnect',
//     env: Env.Staging,
//     provider: '42863c51-54dc-470f-a74e-71cd240fdc56',
//     signingMessageEntity: 'CyberConnect' || your entity,
// });
// cyberConnect.follow('0xD790D1711A9dCb3970F47fd775f2f9A2f0bCc348', 'shiyu');
// ");
// engine.Execute(@"
// var modules = {};
//
// function require(moduleName) {
//   if (modules[moduleName]) {
//     return modules[moduleName];
//   }
//
//   var module = { exports: {} };
//
//   var moduleCode = '
//  import CyberConnect, {
//    Env
//  } from '@cyberlab/cyberconnect-v2';
//
//  const cyberConnect = new CyberConnect({
//    namespace: 'CyberConnect',
//    env: Env.Production,
//    provider: provider,
//    signingMessageEntity: 'CyberConnect' ,
//  });
//  cyberConnect.follow('0xD790D1711A9dCb3970F47fd775f2f9A2f0bCc348', 'shiyu')
//
//
// ';
//   var exports = module.exports;
//
//   // Evaluate the module code
//   (new Function('module', 'exports', moduleCode))(module, exports);
//
//   modules[moduleName] = module.exports;
//
//   return module.exports;
// }
// ");
// engine.Execute(File.ReadAllText($"Assets/lib/cyberConnect/node_modules/@cyberlab/cyberconnect-v2/lib/xiaoye.js"));
// engine.Execute(@"
//  var moduleCode = '
// const CyberConnect = require('@cyberlab/cyberconnect-v2').default;
//  const cyberConnect = new CyberConnect(
// { namespace: 'CyberConnect', env: Env.Production, 
// provider: 'dac1e6e1-78ae-457f-8ba2-79366b503666', signingMessageEntity: 'CyberConnect' });
//  cyberConnect.follow('0xD790D1711A9dCb3970F47fd775f2f9A2f0bCc348', 'shiyu');
//  module.exports = cyberConnect;'
// //log('Hello from Javascript! myVariable = 107');
// ");
// engine.Execute(@"
// import CyberConnect, {
//   Env
// } from '@cyberlab/cyberconnect-v2';
//
// const cyberConnect = new CyberConnect({
//   namespace: 'CyberConnect',
//   env: Env.Production,
//   provider: provider,
//   signingMessageEntity: 'CyberConnect' ,
// });
// cyberConnect.follow('0xD790D1711A9dCb3970F47fd775f2f9A2f0bCc348', 'shiyu');
// ");
// engine.Execute(@"
// cyberConnect.follow('0xD790D1711A9dCb3970F47fd775f2f9A2f0bCc348', 'shiyu');
// ");
// jsli
// engine.SetValue("log", new Action<object>(msg => Debug.Log(msg)));
// IntPtr cyberConnectInstance = CyberConnect("CyberConnect", (int)Env.Production, "dac1e6e1-78ae-457f-8ba2-79366b503666", "CyberConnect");
// Follow(cyberConnectInstance, "0xD790D1711A9dCb3970F47fd775f2f9A2f0bCc348", "shiyu");
// // DestroyCyberConnectInstance(cyberConnectInstance);

        querry1();
    }
    // [DllImport("cyberconnect.dll")]
    // private static extern IntPtr CyberConnect(string namespaceStr, int env, string provider, string signingMessageEntity);
    //
    //
    // [DllImport("cyberconnect.dll")]
    // private static extern void Follow(IntPtr instance, string address, string handle);
    // private static extern void DestroyCyberConnectInstance(IntPtr instance);

    public delegate object RequireDelegate(string moduleName);private void querry1()
    {
        // create a new Jint engine
         engine = new Engine();

        // add the require function to the engine's global object
        // engine.SetValue("require", new RequireDelegate((string moduleName) =>
        // {
        //     // load the module code from a file or other source
        //     var moduleCode = LoadModuleCode(moduleName);
        //
        //     // evaluate the module code in the engine
        //     var exports = engine.Execute(moduleCode).GetCompletionValue();
        //
        //     // check if the module has any nested require calls
        //     var requireDelegate = engine.GetValue("require").ToObject() as RequireDelegate;
        //     var requireFunc = exports.Get("require").As<Function>();
        //     if (requireFunc != null)
        //     {
        //         // replace the nested require call with a custom implementation
        //         exports.Set("require", new ClrFunctionInstance(engine, (thisObj, arguments) =>
        //         {
        //             // get the module name from the arguments
        //             var nestedModuleName = arguments[0].AsString();
        //
        //             // recursively call the require function with the nested module name
        //             return engine.Invoke(requireFunc, nestedModuleName).GetCompletionValue();
        //         }));
        //     }
        //
        //     // return the module's exports object
        //     return exports;
        // }));

        // load the top-level module using the require function
        // engine.Execute("console.warn('dfd')");

        StartCoroutine(LoadJSFile("lib/cyberConnect/node_modules/@cyberlab/cyberconnect-v2/lib/require.js"));
        // string filePath = "lib/cyberConnect/node_modules/@cyberlab/cyberconnect-v2/lib/require.js";
        //
        // TextAsset scriptAsset = Resources.Load<TextAsset>(filePath);
        //
        // if (scriptAsset != null) {
        //     engine.Execute(scriptAsset.text);
        // } else {
        //     Debug.LogError("File not found: " + filePath);
        // }
        // engine.Execute(File.ReadAllText(Application.dataPath +$"/lib/cyberConnect/node_modules/@cyberlab/cyberconnect-v2/lib/require.js"));
        // engine.Execute(File.ReadAllText($"Assets/lib/cyberConnect/node_modules/@cyberlab/cyberconnect-v2/lib/xiaoye.js"));
       
//         engine.Execute(@"
//  require(['cyberconnect'], function (CyberConnect) {
//     const cyberConnect = new CyberConnect({
//         namespace: 'CyberConnect',
//         provider: provider,
//         signingMessageEntity: 'CyberConnect' ,
//     });
//     cyberConnect.follow('0xD790D1711A9dCb3970F47fd775f2f9A2f0bCc348', 'shiyu');
// });
// ");

    }

    private Engine engine;
    IEnumerator LoadJSFile(string fileName)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);

        // Load the main JavaScript file
        UnityWebRequest www = UnityWebRequest.Get(filePath);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(www.error);
            yield break;
        }

        // Execute the main script
        engine.Execute(www.downloadHandler.text);

        // Find and load any additional modules
        var require = engine.GetValue("require");
        if (require != null)
        {
            // Check which modules are required by the script
            var dependencies = GetDependencies(www.downloadHandler.text);

            // Load each required module
            foreach (var dependency in dependencies)
            {
                www = UnityWebRequest.Get(Path.Combine(Application.streamingAssetsPath, dependency));
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError(www.error);
                    yield break;
                }

                // Execute the module
                Debug.Log(www.downloadHandler.text);
                engine.Execute(www.downloadHandler.text);
            }
        }
        string jsStr1 = @"
 require(['cyberconnect'], function (CyberConnect) {{
    const cyberConnect = new CyberConnect({{
        namespace: 'CyberConnect',
env: CyberConnect.Env.Staging,
        provider: '0x0eF49927a22630e22729Cd91892A75f1E15E01D2',
        signingMessageEntity: 'CyberConnect' ,
    }});
    cyberConnect.follow({0}, 'shiyu');
cyberConnect.console.log('e');
}});
";
        //     jsStr1 = @" ,
        // }}{0}";
        string jsSt = string.Format(jsStr1.ToString(),"'"+( PlayerPrefs.GetString("Account")==""?"0x591e0850a4D19045388F37E5D1BA9be411b22a57":PlayerPrefs.GetString("Account"))+"'");
        engine.Execute(jsSt);
    }

// Helper function to parse the required modules from a script
    List<string> GetDependencies(string script)
    {
        var dependencies = new List<string>();
        var regex = new Regex(@"require\s*\(\s*""(.+)""\s*\)");

        var matches = regex.Matches(script);
        foreach (Match match in matches)
        {
            dependencies.Add(match.Groups[1].Value);
        }

        return dependencies;
    }

    private string LoadModuleCode(string moduleName)
    {
        // load the module code from a file or other source
        return File.ReadAllText(moduleName);
    }
    
    public enum Env {
        Staging ,
        Production,
    }

    
    // public void OnHidden()
    // {
    // }
}
