using System;
using PartySystems.UIParty;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GraphQlClient.Core;
using Jint;
using Main.Logic.CyberConnect;
using Main.Logic.Graph.CyberConnect;
using Main.Logic.Player;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Util;
using Object = System.Object;

public class UIRank : UIBase
{
    private UIRankWrap wrap;
    
    public GraphApi pokemonReference;
    public override void OnInit()
    {
        wrap = transform.GetComponent<UIRankWrap>();
        wrap.btn_close.onClick.RemoveListener(HideView);
        wrap.btn_close.onClick.AddListener(HideView);
    }

    void HideView()
    {
        // var hud = UIManager.Instance.GetHUD<UIRank>(UIManager.EViewPriority.HighRenderPriority);
        // hud.Reference.CloseView();
        CloseView();
    }
    protected override void OnShown()
    {
        // UIUtil.UpdateWithTransfrom(wrap.node_item.transform, (trans1, index1) =>
        // {
        //     Transform tr_grid = trans1.Find("tr_grid");
        //     girdDataDict[index1] = new Dictionary<int, Text>();
        //     UIUtil.UpdateWithTransfrom(tr_grid, (trans2, index2) =>
        //     {
        //         Text txt_word = trans2.Find("bg/txt_word").GetComponent<Text>();
        //         txt_word.text = "";
        //         girdDataDict[index1][index2] = txt_word;
        //     },ColCount);
        // },RowCount);
        RequestFollowInfo();
    }

    private Dictionary<string, object> getsdfa(Dictionary<string,object> schemaDict,string introspection)
    {
        Dictionary<string, object> schemaClass = JsonConvert.DeserializeObject<Dictionary<string, object>>(introspection);
        // if()
        return null;
    }
    async void RequestFollowInfo()
    {
        LCyberConnect.I.GetProfileByHandle("suv",(profileDaata)=>
        {
            // if (profileDaata != null)
            // {
            //     nodeWrap.txt_address.text =
            //         profileDaata.owner.address;
            //     string imgUrl=profileDaata.avatar;
            //     StartCoroutine(LoadImage(nodeWrap.img_avata,imgUrl));
            // }
                       
        });
        List<string> AddressList = new List<string>();
         LCyberConnect.I.GetFollowingByAddress(
            LPlayer.I.Address == "" ? "0x591e0850a4D19045388F37E5D1BA9be411b22a57" : LPlayer.I.Address,
            (followedData) =>
            {
                foreach (var VARIABLE in followedData.followings.edges)
                {
                    AddressList.Add(VARIABLE.node.address);
                }
            });
        //获取关注列表
        LCyberConnect.I.GetFollowingByAddress(
            LPlayer.I.Address == "" ? "0x591e0850a4D19045388F37E5D1BA9be411b22a57" : LPlayer.I.Address,
            (followedData) =>
            {
                UIUtil.UpdateWithTransfrom(wrap.node_item.transform, (trans1, index1) =>
                {
                    var itemData = followedData.followings.edges[index1];
                    UIRankWrap_node_itemWrap nodeWrap = trans1.GetComponent<UIRankWrap_node_itemWrap>();
                    // var handleStr = itemData["node"]?["handle"];
                    //  RequestProfileInfo(nodeWrap.txt_address,handleStr.ToString());
                    // var addressStr = itemData["node"]?["address"];
                    LCyberConnect.I.GetProfileByHandle(itemData.node.handle,(profileDaata)=>
                    {
                        if (profileDaata != null)
                        {
                            nodeWrap.txt_address.text =
                                profileDaata.owner.address;
                            string imgUrl=profileDaata.avatar;
                            StartCoroutine(LoadImage(nodeWrap.img_avata,imgUrl));
                        }
                       
                    });
                    // nodeWrap.txt_address.text = itemData["node"]?["address"]?.ToString() ?? "";
                    // string imgUrl=itemData["node"]?["profile"]["owner"]?["avatar"]?.ToString() ?? "";
                    // StartCoroutine(LoadImage(nodeWrap.img_avata,"imgUrl"));
                    // Transform tr_grid = trans1.Find("tr_grid");
                    // girdDataDict[index1] = new Dictionary<int, Text>();
                    // UIUtil.UpdateWithTransfrom(tr_grid, (trans2, index2) =>
                    // {
                    //     Text txt_word = trans2.Find("bg/txt_word").GetComponent<Text>();
                    //     txt_word.text = "";
                    //     girdDataDict[index1][index2] = txt_word;
                    // },ColCount);
                },followedData.followingCount);
            });
        // //获取关注
        // GraphApi.Query createUser = pokemonReference.GetQueryByName("getFollowingsByAddressEVM", GraphApi.Query.Type.Query);
        //
        // createUser.SetArgs(new{address = LPlayer.I.Address==""?"0x591e0850a4D19045388F37E5D1BA9be411b22a57":LPlayer.I.Address});
        //
        //
        // //Performs Post request to server
        // UnityWebRequest request = await pokemonReference.Post(createUser);
        //
        // if (!request.isNetworkError)
        // {
        //     string introspection = request.downloadHandler.text;
        //     // var type = createUser.fields[0].GetType();
        //     
        //     Dictionary<string, object> schemaClass = JsonConvert.DeserializeObject<Dictionary<string, object>>(introspection);
        //     
        //     // Dictionary<string, object> schemaClass1= JsonConvert.DeserializeObject<Dictionary<string, object>>(schemaClass["data"]);
        //     JObject obj = JObject.Parse(introspection);
        //
        //     JArray followings = (JArray)obj["data"]?["address"]?["followings"]?["edges"];
        //     if (followings!= null)
        //     {
        //         UIUtil.UpdateWithTransfrom(wrap.node_item.transform, (trans1, index1) =>
        //         {
        //             JObject itemData = followings[index1] as JObject;
        //             UIRankWrap_node_itemWrap nodeWrap = trans1.GetComponent<UIRankWrap_node_itemWrap>();
        //            // var handleStr = itemData["node"]?["handle"];
        //            //  RequestProfileInfo(nodeWrap.txt_address,handleStr.ToString());
        //            var addressStr = itemData["node"]?["address"];
        //             LCyberConnect.I.GetProfilesByAddress(addressStr.ToString(),(profileDaata)=>
        //             {
        //                 if (profileDaata != null)
        //                 {
        //                     nodeWrap.txt_address.text =
        //                         profileDaata.profiles.edges[0];
        //                     string imgUrl=profileDaata.wallet?.subscribings?.edges?[0]?.node?.profile?.avatar ?? "";
        //                     StartCoroutine(LoadImage(nodeWrap.img_avata,imgUrl));
        //                 }
        //                
        //             });
        //             // nodeWrap.txt_address.text = itemData["node"]?["address"]?.ToString() ?? "";
        //             // string imgUrl=itemData["node"]?["profile"]["owner"]?["avatar"]?.ToString() ?? "";
        //             // StartCoroutine(LoadImage(nodeWrap.img_avata,"imgUrl"));
        //             // Transform tr_grid = trans1.Find("tr_grid");
        //             // girdDataDict[index1] = new Dictionary<int, Text>();
        //             // UIUtil.UpdateWithTransfrom(tr_grid, (trans2, index2) =>
        //             // {
        //             //     Text txt_word = trans2.Find("bg/txt_word").GetComponent<Text>();
        //             //     txt_word.text = "";
        //             //     girdDataDict[index1][index2] = txt_word;
        //             // },ColCount);
        //         },followings.Count);
        //     }
        //     
        //     // Object schemaClass = JsonConvert.DeserializeObject<Object>(introspection);
        //     // var etrfd = schemaClass["followings"];
        // }

    }
    
    public async void RequestProfileInfo(Text txt_address,string handleStr)
    {
        //获取关注
        GraphApi.Query createUser = pokemonReference.GetQueryByName("getProfileByHandle", GraphApi.Query.Type.Query);
        
        createUser.SetArgs(new{handle = handleStr});
        
        //Performs Post request to server
        UnityWebRequest request = await pokemonReference.Post(createUser);

        if (!request.isNetworkError)
        {
            string introspection = request.downloadHandler.text;
            // var type = createUser.fields[0].GetType();

            JObject obj = JObject.Parse(introspection);

            var profileByHandle =  obj["data"]?["profileByHandle"];
            if (profileByHandle != null && profileByHandle is JObject)
            {
                txt_address.text = profileByHandle["owner"]?["address"].ToString();
            }
            // return profileByHandle;
        }

        // return null;

    }

    
    private IEnumerator LoadImage(Image image,String imageUrl)
    {
        WWW www = new WWW(imageUrl);
        yield return www;
        image.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), Vector2.zero);
    }
}
