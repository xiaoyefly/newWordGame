using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GraphQlClient.Core;
using Jint;
using Main.Logic.CyberConnect;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;
using Object = System.Object;

namespace Main.Logic.Graph.CyberConnect
{
    public partial class LCyberConnect
    {
        private Engine engine;
        
        
        public IEnumerator LoadJSFile(string fileName)
        {
            engine=new Engine();
            string filePath = Path.Combine(Application.streamingAssetsPath, fileName);
            filePath = "file://"+filePath; 
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
    
    }
}