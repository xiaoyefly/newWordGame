using PartySystems.UIParty;
using System.Collections;
using System.Collections.Generic;
using GraphQlClient.Core;
using Main.Logic.Graph.CyberConnect;
using UnityEngine;

public class InitMainScene : MonoBehaviour
{
    // Start is called before the first frame update
    public GraphApi cyberConnectReference;
    void Start()
    {
        var hud = UIManager.Instance.GetHUD<UIMain>(UIManager.EViewPriority.HighRenderPriority);
        hud.Reference.ShowView();
        LCyberConnect.I.cyberConnectReference = cyberConnectReference;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
