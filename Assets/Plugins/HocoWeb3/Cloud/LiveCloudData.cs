using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Hoco.Cloud
{
    [System.Serializable]
    public abstract class LiveCloudData 
    {
        [JsonProperty("_id")]
        public string Id { get; set; } = string.Empty;
    }

}