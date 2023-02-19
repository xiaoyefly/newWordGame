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
    public class ProfileConnection
    {
        public int totalCount;
        public PageInfo pageInfo;
        public List<ProfileEdge> edges;
    }
}