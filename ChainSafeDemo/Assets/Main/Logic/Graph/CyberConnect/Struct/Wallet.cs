using System;
using System.Collections.Generic;
using GraphQlClient.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;
using Object = System.Object;

namespace Main.Logic.CyberConnect
{
    public class Wallet
    {
        public long id;
        public string address;
        public long chainID;
        public Profile primaryProfile;
        public ProfileConnection profiles;
        public SubscribeConnection subscribings;
        public int subscribingCount;
    }
}