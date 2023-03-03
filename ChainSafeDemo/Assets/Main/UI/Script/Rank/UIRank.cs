using System;
using PartySystems.UIParty;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GraphQlClient.Core;
using Jint;
using Main.Logic.CyberConnect;
using Main.Logic.Graph.CyberConnect;
using Main.Logic.Player;
using Main.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Util;
using WebP.Experiment.Animation;

// using WebP.Experiment.Animation;
// using Object = System.Object;

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
        //获取关注列表
        LCyberConnect.I.GetFollowingByAddress(
            LPlayer.I.Address == "" ? "0x591e0850a4D19045388F37E5D1BA9be411b22a57" : LPlayer.I.Address,
            (followedData) =>
            {
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    UIUtil.UpdateWithTransfrom(wrap.node_item.transform, (trans1, index1) =>
                    {
                        var itemData = followedData.followings.edges[index1];
                        UIRankWrap_node_itemWrap nodeWrap = trans1.GetComponent<UIRankWrap_node_itemWrap>();
                        nodeWrap.txt_address.text =
                            itemData.node.profile.handle;
                        string imgUrl = itemData.node.profile.avatar;
                        if (!string.IsNullOrEmpty(imgUrl))
                        {
                            if (IsWebP(imgUrl))
                            {
                                LoadWebP(nodeWrap.img_avata, imgUrl);
                                // StartCoroutine(LoadImageConvertToPNG(nodeWrap.img_avata, imgUrl));
                            }
                            else
                            {
                                StartCoroutine(LoadImageConvertToPNGFromWeb(nodeWrap.img_avata, imgUrl));
                            }
                         
                        }
                        // LCyberConnect.I.GetProfileByHandle(itemData.node.handle, (profileDaata) =>
                        // {
                        //     if (profileDaata != null)
                        //     {
                        //         if (profileDaata.owner != null)
                        //         {
                        //             nodeWrap.txt_address.text =
                        //                 profileDaata.handle;
                        //         }
                        //         else
                        //         {
                        //             nodeWrap.txt_address.text = "空地址";
                        //         }
                        //
                        //         // string imgUrl = profileDaata.avatar;
                        //         // if (!string.IsNullOrEmpty(imgUrl))
                        //         // {
                        //         //     if (IsWebP(imgUrl))
                        //         //     {
                        //         //         StartCoroutine(LoadImageConvertToPNG(nodeWrap.img_avata, imgUrl));
                        //         //     }
                        //         //     else
                        //         //     {
                        //         //         StartCoroutine(LoadImageConvertToPNGFromWeb(nodeWrap.img_avata, imgUrl));
                        //         //     }
                        //         //  
                        //         // }
                        //     }
                        // });

                    },followedData.followingCount);
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
                // do something with the sprite, e.g. assign it to a GameObject's SpriteRenderer component
            }
        }
    }
    
    private IEnumerator LoadImageConvertToPNG(Image image,String imageUrl)
    {
        // var lWebStream = new WWW(imageUrl);
        //
        // yield return lWebStream;
        //
        // Error lError;
        //
        // Texture2D lTexture2D = Texture2DExt.CreateTexture2DFromWebP(lWebStream.bytes, true, true, out lError);
        //
        // if (lError == Error.Success)
        // {
        //     // m_Material.mainTexture = lTexture2D;
        //     image.sprite = Sprite.Create(lTexture2D, new Rect(0, 0, lTexture2D.width, lTexture2D.height), Vector2.zero);
        // }
        // else
        // {
        //     Debug.LogError("Webp Load Error : " + lError.ToString());
        // }
        // var renderer=  WebP.Experiment.Animation.WebP.LoadTexturesAsync(imageUrl);
        // if (renderer != null)
        // {
        //     renderer.Result.OnRender= (tex)=>
        //     {
        //         image.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
        //     };
        //     renderer.Result.Start();
        // }

        yield return null;
        // using (WWW www = new WWW(imageUrl))
        // {
        //     yield return www;
        //     // WebP.Experiment.Animation.WebP.LoadTexturesAsync()
        //     
        //     Texture2D webTexture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
        //     webTexture.LoadImage(www.bytes);
        //     byte[] pngData = webTexture.EncodeToJPG(100);
        //     Texture2D tex = new Texture2D(2, 2);
        //     tex.LoadImage(pngData);
        //     Texture2D spriteTex = new Texture2D(webTexture.width, webTexture.height, TextureFormat.RGBA32, false);
        //     spriteTex.SetPixels(tex.GetPixels());
        //     spriteTex.Apply();
        //     image.sprite = Sprite.Create(spriteTex, new Rect(0, 0, webTexture.width, webTexture.height), Vector2.zero);
        //     // if (www.result != UnityWebRequest.Result.ConnectionError && www.result != UnityWebRequest.Result.ProtocolError )
        //     // {
        //     // Texture2D webpTexture = DownloadHandlerTexture.GetContent(www);
        //     // byte[] pngData = webpTexture.EncodeToPNG();
        //     // string pngFilePath = Path.Combine(Application.persistentDataPath, "temp.png");
        //     // File.WriteAllBytes(pngFilePath, pngData);
        //     // //
        //     // //
        //     // StartCoroutine(LoadSpriteFromPNG(image,pngFilePath));
        //     // }
        // }
        // WWW www = new WWW(imageUrl);
        // yield return www;
        // image.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), Vector2.zero);
    }
    
    private async Task LoadWebP(Image image,string url)
    {
        WebPRendererWrapper<Texture2D> renderer = await WebP.Experiment.Animation.WebP.LoadTexturesAsync(url,
            (texture) =>
            {
                OnWebPRender(image,texture, url);
            });
        // if (renderer != null)
        // {
        //     renderer.OnRender += texture => OnWebPRender(image,texture, url);
        //     renderer.Start();
        // }
        // mRenderer = renderer;
    }
    
    private void OnWebPRender(Image image,Texture2D texture, string url)
    {
        image.sprite=Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        // image = texture;
    }
    // private async Task LoadWebP(Image image,string url)
    // {
    //     WebPRendererWrapper<Texture2D> renderer = await WebP.Experiment.Animation.WebP.LoadTexturesAsync(url);
    //     if (renderer != null)
    //     {
    //         renderer.OnRender += texture =>
    //         {
    //             Sprite sprite = Sprite.Create(texture, new Rect(0,0,texture.width,texture.height),Vector2.zero);
    //             image.sprite = sprite;
    //         };
    //     }
    // }
    
    // private void OnWebPRender(Texture texture, string url)
    // {
    //     RawImage.texture = texture;
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
            return DownloadHandlerTexture.GetContent(www);
        }
    }
    
    public bool IsWebP(string imgurl)
    {
        bool isWebP = false;
        WebClient client = new WebClient();
        byte[] header = client.DownloadData(imgurl).Take(12).ToArray();
        if (header[0] == 'R' && header[1] == 'I' && header[2] == 'F' && header[3] == 'F' &&
            header[8] == 'W' && header[9] == 'E' && header[10] == 'B' && header[11] == 'P')
        {
            isWebP = true;
        }
        return isWebP;
    }
}
