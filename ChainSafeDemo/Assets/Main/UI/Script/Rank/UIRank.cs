using System;
using PartySystems.UIParty;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
// using System.Net;
// using System.Text.RegularExpressions;
// using System.Threading;
// using System.Threading.Tasks;
using GraphQlClient.Core;
// using Jint;
// using Main.Logic.CyberConnect;
using Main.Logic.Graph.CyberConnect;
using Main.Logic.Player;
using Main.Utility;
// using Newtonsoft.Json;
// using Newtonsoft.Json.Linq;
// using OpenCover.Framework.Model;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Util;
// using Web3Unity.Scripts.Library.IPFS; 
// using WebP.Experiment.Animation;
using File = System.IO.File;

// using WebP.Experiment.Animation;
// using Object = System.Object;

public class UIRank : UIBase
{
    private UIRankWrap wrap;
    
    public GraphApi pokemonReference;

    private string RequestAddress = "";
    public override void OnInit()
    {
        wrap = transform.GetComponent<UIRankWrap>();
        wrap.btn_close.onClick.RemoveListener(HideView);
        wrap.btn_close.onClick.AddListener(HideView);
    }

    public void SetInitData(string address)
    {
        RequestAddress = address;
        if (RequestAddress == LPlayer.I.Address)
        {
            wrap.txt_title.text = "Following";
        }
        else
        {
            // wrap.txt_title.text = "Profile";
        }
    }
    void HideView()
    {
        CloseView();
    }
    protected override void OnShown()
    {
        RequestFollowInfo();
    }

    private List<string> myFolloringList = new List<string>();
    async void RequestFollowInfo()
    {
        //获取关注列表
        LCyberConnect.I.GetFollowingByAddress(
            RequestAddress == "" ? "0x591e0850a4D19045388F37E5D1BA9be411b22a57" : RequestAddress,
            (followedData) =>
            {
                int Count = 0;
                if (followedData!=null && followedData.followingCount > 0 && followedData.followings.edges.Count > 0)
                {
                    Count = followedData.followings.edges.Count;
                }

                if (RequestAddress == LPlayer.I.Address)
                {
                    myFolloringList.Clear();
                    if (followedData!=null && followedData.followingCount > 0 && followedData.followings.edges.Count>0)
                    {
                        foreach (var VARIABLE in followedData.followings.edges)
                        {
                            myFolloringList.Add(VARIABLE.node.profile.handle);
                        }
                    }
                   
                }
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    UIUtil.UpdateWithTransfrom(wrap.node_item.transform, (trans1, index1) =>
                    {
                        var itemData = followedData.followings.edges[index1];
                        UIRankWrap_node_itemWrap nodeWrap = trans1.GetComponent<UIRankWrap_node_itemWrap>();
                        nodeWrap.btn_item.onClick.RemoveAllListeners();
                        nodeWrap.btn_item.onClick.AddListener(() =>
                        {
                            SetInitData(itemData.node.profile.owner.address);
                            wrap.txt_title.text = itemData.node.profile.handle;
                            RequestFollowInfo();
                            // StartCoroutine(LCyberConnect.I.LoadJSFile("lib/cyberConnect/node_modules/@cyberlab/cyberconnect-v2/lib/require.js"));
                        });
                        if (myFolloringList.Contains(itemData.node.profile.handle))
                        {
                            nodeWrap.txt_word.text = "Following";
                        }
                        else
                        {
                            nodeWrap.txt_word.text = "Follow";
                        }
                        nodeWrap.txt_address.text =
                            itemData.node.profile.handle;
                        string imgUrl = itemData.node.profile.avatar;
                        nodeWrap.img_avata.gameObject.SetActive(false);
                        if (string.IsNullOrEmpty(imgUrl))
                        {
                            if (itemData.node.profile.metadataInfo != null)
                            {
                                imgUrl=itemData.node.profile.metadataInfo.avatar;
                            }
                        }
                        if (imgUrl.StartsWith("ipfs://"))
                        {
                            imgUrl = imgUrl.Replace("ipfs://", "https://ipfs.io/ipfs/");
                        } 
                        if (!string.IsNullOrEmpty(imgUrl))
                        {
                            StartCoroutine(IsWebPIEnum(imgUrl, (isWebp) =>
                            {
                                if (isWebp)
                                {
                                    // LoadWebP(nodeWrap.img_avata, imgUrl);
                                    StartCoroutine(LoadImageConvertToPNGFromWeb1(nodeWrap.img_avata, imgUrl));
                                }
                                else
                                {
                                    StartCoroutine(LoadImageConvertToPNGFromWeb(nodeWrap.img_avata, imgUrl));
                                    // StartCoroutine(LoadImageConvertToPNGFromWeb(nodeWrap.img_avata, imgUrl));
                                    // StartCoroutine(LoadImageConvertToPNGFromWeb(nodeWrap.img_avata, imgUrl));
                                    // StartCoroutine(LoadImageConvertToPNGFromWeb(nodeWrap.img_avata, imgUrl));
                                    // StartCoroutine(LoadImageConvertToPNGFromWeb(nodeWrap.img_avata, imgUrl));
                                    // StartCoroutine(LoadImageConvertToPNGFromWeb(nodeWrap.img_avata, imgUrl));
                                    // StartCoroutine(LoadImageConvertToPNGFromWeb(nodeWrap.img_avata, imgUrl));
                                    // StartCoroutine(LoadImageConvertToPNGFromWeb(nodeWrap.img_avata, imgUrl));
                                    // StartCoroutine(LoadImageConvertToPNGFromWeb(nodeWrap.img_avata, imgUrl));
                                }
                            }));
                            // if (IsWebP(imgUrl))
                            // {
                            //     // LoadWebP(nodeWrap.img_avata, imgUrl);
                            //     StartCoroutine(LoadImageConvertToPNGFromWeb1(nodeWrap.img_avata, imgUrl));
                            // }
                            // else
                            // {
                            //     StartCoroutine(LoadImageConvertToPNGFromWeb(nodeWrap.img_avata, imgUrl));
                            //     StartCoroutine(LoadImageConvertToPNGFromWeb(nodeWrap.img_avata, imgUrl));
                            //     StartCoroutine(LoadImageConvertToPNGFromWeb(nodeWrap.img_avata, imgUrl));
                            //     StartCoroutine(LoadImageConvertToPNGFromWeb(nodeWrap.img_avata, imgUrl));
                            //     StartCoroutine(LoadImageConvertToPNGFromWeb(nodeWrap.img_avata, imgUrl));
                            //     StartCoroutine(LoadImageConvertToPNGFromWeb(nodeWrap.img_avata, imgUrl));
                            //     StartCoroutine(LoadImageConvertToPNGFromWeb(nodeWrap.img_avata, imgUrl));
                            //     StartCoroutine(LoadImageConvertToPNGFromWeb(nodeWrap.img_avata, imgUrl));
                            //     StartCoroutine(LoadImageConvertToPNGFromWeb(nodeWrap.img_avata, imgUrl));
                            // }

                        }
                       

                    },Count);
                });
            });
    }

    
    private IEnumerator LoadImageConvertToPNGFromWeb(Image image,String imageUrl)
    {
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageUrl))
        {
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(www);
                image.sprite= Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                image.gameObject.SetActive(true);
                // do something with the sprite, e.g. assign it to a GameObject's SpriteRenderer component
            }
            www.Dispose();
        }
    }
    private IEnumerator LoadImageConvertToPNGFromWeb1(Image image,String imageUrl)
    {
        yield return WebP.Experiment.Animation.WebP.LoadIEnumerator(imageUrl,(texture) =>
        {
            image.sprite=Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            image.gameObject.SetActive(true);
        });
    }
    
    // private async Task LoadWebP(Image image,string url)
    // {
    //     // Thread thread = new Thread(() =>
    //     // {
    //         ThreadLoadWebP(image,url);
    //     // });
    //     //
    //     // // threads.Add(thread);
    //     // thread.Start();
    //   
    // }

    // private async Task ThreadLoadWebP(Image image, string url)
    // {
    //     WebPRendererWrapper<Texture2D> renderer = await WebP.Experiment.Animation.WebP.LoadTexturesAsync(url,
    //         (texture) =>
    //         {
    //             image.sprite=Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
    //             image.gameObject.SetActive(true);
    //         });
    // }
    public IEnumerator LoadSpriteFromPNG(Image image,string imgPath)
    {
        Texture2D texture;
        imgPath = imgPath.Substring(7);
        texture = LoadTextureFromFile(imgPath);
        if (texture == null)
        {
            Debug.LogError("Failed to load texture from " + imgPath);
            yield break;
        }
        image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        File.Delete(imgPath);
        // do something with the sprite, e.g. assign it to a GameObject's SpriteRenderer component
    }
        
    private Texture2D LoadTextureFromFile(string filePath)
    {
        byte[] fileData = File.ReadAllBytes(filePath);
        Texture2D texture = new Texture2D(2, 2);
        if (texture.LoadImage(fileData))
        {
            return texture;
        }
        return null;
    }
    
    private Texture2D LoadTextureFromURL(string url)
    {
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
        {
            www.SendWebRequest();
            while (!www.isDone) { }
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Failed to load texture from " + url + ": " + www.error);
                return null;
            }

            Texture2D result = DownloadHandlerTexture.GetContent(www);
            www.Dispose();
            return result;
        }
        
    }

    public async void IsWebP111(string imgurl,Action<bool> handle)
    {
      // IPFS  
        // ipfsClient ipfsClient = new IpfsClient();
        //
        // // Retrieve the file from IPFS
        // byte[] fileBytes = await ipfsClient.FileSystem.ReadAllBytesAsync(ipfsHash);
        //
        // // Convert the file bytes to a string
        // string fileString = System.Text.Encoding.UTF8.GetString(fileBytes);
        //
        // // Deserialize the JSON response
        // IpfsResponse ipfsResponse = JsonConvert.DeserializeObject<IpfsResponse>(fileString);
        //
        // // Display the response
        // Debug.Log(ipfsResponse.Hash);
    }
    // public async void IsWebP(string imgurl,Action<bool> handle)
    // {
    //     bool isWebP = false;
    //     
    //     WebClient client = new WebClient();
    //     client.DownloadDataCompleted += (sender, e) => {
    //         if (e.Error != null || e.Cancelled)
    //         {
    //             handle(false);
    //         }
    //         else
    //         {
    //             byte[] header = e.Result.Take(12)?.ToArray();
    //             if (header!=null && header.Length>11 && header[0] == 'R' && header[1] == 'I' && header[2] == 'F' && header[3] == 'F' &&
    //                 header[8] == 'W' && header[9] == 'E' && header[10] == 'B' && header[11] == 'P')
    //             {
    //                 handle(true);
    //             }
    //             else
    //             {
    //                 handle(false);
    //             }
    //         }
    //     };
    //
    //     client.DownloadDataAsync(new Uri(imgurl));
    //
    //     await Task.Delay(TimeSpan.FromSeconds(3));
    //
    //     if (client.IsBusy)
    //     {
    //         client.CancelAsync();
    //         // handle(false);
    //         // Handle cancellation
    //     }
    //     
    //     client.Dispose();
    // }
    
    public IEnumerator IsWebPIEnum(string imgurl, Action<bool> handle)
    {
        bool isWebP = false;

        UnityWebRequest webRequest = UnityWebRequest.Get(imgurl);
        AsyncOperation asyncOp = webRequest.SendWebRequest();

        while (!asyncOp.isDone)
        {
            // Wait for async operation to complete
            yield return null;
        }

        if (webRequest.result == UnityWebRequest.Result.Success)
        {
            byte[] header = webRequest.downloadHandler.data.Take(12)?.ToArray();
            if (header != null && header.Length > 11 && header[0] == 'R' && header[1] == 'I' && header[2] == 'F' && header[3] == 'F' &&
                header[8] == 'W' && header[9] == 'E' && header[10] == 'B' && header[11] == 'P')
            {
                isWebP = true;
            }
        }

        webRequest.Dispose();
        handle(isWebP);
    }
}
