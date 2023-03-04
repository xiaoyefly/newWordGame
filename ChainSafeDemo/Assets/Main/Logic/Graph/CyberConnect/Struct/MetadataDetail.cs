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
    public class MetadataDetail
    {
        public string displayName;
        public string bio;
        public string avatar;
        public string coverImage;
        public string version;
    }
}