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
    public class SubscribeConnection
    {
        public int totalCount;
        public PageInfo pageInfo;
        public List<SubscribeEdge> edges;
    }
}