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
    public class Address
    {
        public string address;
        public int chainID;
        public Wallet wallet;
        public PostConnection posts;
        public int postCount;
        public FollowConnection followings;
        public int followingCount;
    }
}