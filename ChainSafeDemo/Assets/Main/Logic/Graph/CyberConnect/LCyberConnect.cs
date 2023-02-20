using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GraphQlClient.Core;
using Main.Logic.CyberConnect;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;
using Object = System.Object;

namespace Main.Logic.Graph.CyberConnect
{
    public partial class LCyberConnect:Main.Utility.Singleton<LCyberConnect>
    {
        public GraphApi cyberConnectReference;
        
        //根据地址获得订阅的钱包地址
        public void GetSubscribingByAddress(string address,Action<Wallet> action)
        {
            GetSubscribingByAddressEVM(address,action);
        }
        private async void GetSubscribingByAddressEVM(string address,Action<Wallet> action)
        {
            if (cyberConnectReference == null)
            {
                action.Invoke(null);
                return;
            }
            
            GraphApi.Query createUser = cyberConnectReference.GetQueryByName("getSubscribingByAddressEVM", GraphApi.Query.Type.Query);
            createUser.SetArgs(new{address = address});
            
            UnityWebRequest request = await cyberConnectReference.Post(createUser);
            
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
                request.Dispose();
                action.Invoke(null);
                return;
            }
            
            string introspection = request.downloadHandler.text;
            request.Dispose();
            JObject obj = JObject.Parse(introspection);

            if (obj == null)
            {
                action.Invoke(null);
                return;
            }
            
            var data = obj["data"]?["address"];
            if (data == null)
            {
                action.Invoke(null);
                return;
            }
            
            Wallet schemaClass = JsonConvert.DeserializeObject<Wallet>(data.ToString());
            action.Invoke(schemaClass);

        }
        
        //根据地址获得关注数据
        public void GetFollowingByAddress(string address,Action<Address> action)
        {
            GetFollowingsByAddressEVM(address,action);
        }
        private async void GetFollowingsByAddressEVM(string address,Action<Address> action)
        {
            if (cyberConnectReference == null)
            {
                action.Invoke(null);
                return;
            }
            
            GraphApi.Query createUser = cyberConnectReference.GetQueryByName("getFollowingsByAddressEVM", GraphApi.Query.Type.Query);
            createUser.SetArgs(new{address = address});
            
            UnityWebRequest request = await cyberConnectReference.Post(createUser);
            
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
                request.Dispose();
                action.Invoke(null);
                return;
            }
            
            string introspection = request.downloadHandler.text;
            request.Dispose();
            JObject obj = JObject.Parse(introspection);

            if (obj == null)
            {
                action.Invoke(null);
                return;
            }
            
            var data = obj["data"]?["address"];
            if (data == null)
            {
                action.Invoke(null);
                return;
            }
            
            Address schemaClass = JsonConvert.DeserializeObject<Address>(data.ToString());
            action.Invoke(schemaClass);

        }
        
        //根据地址获得关注数据
        public void GetProfileByHandle(string handle,Action<Profile> action)
        {
            SendprofileByHandle(handle,action);
        }
        public IEnumerator SendprofileByHandleIEnum(string handle,Action<Profile> action)
        {
            if (cyberConnectReference == null)
            {
                action.Invoke(null);
                yield break;
            }
            
            GraphApi.Query createUser = cyberConnectReference.GetQueryByName("profileByHandle", GraphApi.Query.Type.Query);
            createUser.SetArgs(new{handle = handle});
            
            UnityWebRequest request = UnityWebRequest.Post(cyberConnectReference.url, UnityWebRequest.kHttpVerbPOST);
            string jsonData = JsonConvert.SerializeObject(new{query = createUser.query});
            byte[] postData = Encoding.ASCII.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(postData);
            request.SetRequestHeader("Content-Type", "application/json");
            if (!String.IsNullOrEmpty(cyberConnectReference.GetAuthToken())) 
                request.SetRequestHeader("Authorization", "Bearer " + cyberConnectReference.GetAuthToken());
            
            
            try{
                request.SendWebRequest();
            }
            catch(Exception e){
                request.Dispose();
                Debug.Log("Testing exceptions");
            }
            
            // UnityWebRequest request = cyberConnectReference.Post(createUser);
            yield return request;
            
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
                request.Dispose();
                action.Invoke(null);
                yield break;
            }
            
            string introspection = request.downloadHandler.text;
            request.Dispose();
            if(string.IsNullOrEmpty(introspection))
            {
                action.Invoke(null);
                yield break;
            }
            JObject obj = JObject.Parse(introspection);

            if (obj == null)
            {
                action.Invoke(null);
                yield break;

            }
            
            var data = obj["data"]?["profileByHandle"];
            if (data == null)
            {
                action.Invoke(null);
                yield break;

            }
            
            Profile schemaClass = JsonConvert.DeserializeObject<Profile>(data.ToString());
            action.Invoke(schemaClass);
            yield break;


        }
        private async void SendprofileByHandle(string handle,Action<Profile> action)
        {
            if (cyberConnectReference == null)
            {
                action.Invoke(null);
                return;
            }
            
            GraphApi.Query createUser = cyberConnectReference.GetQueryByName("profileByHandle", GraphApi.Query.Type.Query);
            createUser.SetArgs(new{handle = handle});
            
            UnityWebRequest request = await cyberConnectReference.Post(createUser);
            
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
                request.Dispose();
                action.Invoke(null);
                return;
            }
            
            string introspection = request.downloadHandler.text;
            request.Dispose();
            JObject obj = JObject.Parse(introspection);

            if (obj == null)
            {
                action.Invoke(null);
                return;
            }
            
            var data = obj["data"]["profileByHandle"];
            if (data == null)
            {
                action.Invoke(null);
                return;
            }
            
            Profile schemaClass = JsonConvert.DeserializeObject<Profile>(data.ToString());
            action.Invoke(schemaClass);

        }
        
        
    }
}