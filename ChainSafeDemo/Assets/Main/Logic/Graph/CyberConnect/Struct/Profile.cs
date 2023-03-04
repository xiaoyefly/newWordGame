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
    public class Profile
    {
        public long id;
        public int profileID;
        public string handle;
        public Wallet owner;
        public string metadata;
        public string avatar="";
        public bool isPrimary;
        public SubscribeConnection subscribers;
        public int subscriberCount;
        public int followerCount;
        public FollowConnection followers;
        public bool isSubscribedByMe;
        public bool isFollowedByMe;
        public MetadataDetail metadataInfo;
    }
}