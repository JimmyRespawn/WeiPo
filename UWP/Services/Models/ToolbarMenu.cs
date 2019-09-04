﻿using Newtonsoft.Json;

namespace WeiPo.Services.Models
{
    public class ToolbarMenu
    {
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("sub_type", NullValueHandling = NullValueHandling.Ignore)]
        public long? SubType { get; set; }

        [JsonProperty("params", NullValueHandling = NullValueHandling.Ignore)]
        public ToolbarMenuParams Params { get; set; }

        [JsonProperty("actionlog", NullValueHandling = NullValueHandling.Ignore)]
        public Actionlog Actionlog { get; set; }

        [JsonProperty("scheme", NullValueHandling = NullValueHandling.Ignore)]
        public string Scheme { get; set; }
    }
}