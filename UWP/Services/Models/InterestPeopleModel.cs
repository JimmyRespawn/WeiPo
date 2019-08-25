﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WeiPo.Services.Models
{
    
    public partial class InterestPropleDescModel
    {
        [JsonProperty("desc", NullValueHandling = NullValueHandling.Ignore)]
        public string Desc { get; set; }

        [JsonProperty("card_type", NullValueHandling = NullValueHandling.Ignore)]
        public long CardType { get; set; }

        [JsonProperty("scheme", NullValueHandling = NullValueHandling.Ignore)]
        public string Scheme { get; set; }

        [JsonProperty("actionlog", NullValueHandling = NullValueHandling.Ignore)]
        public Actionlog Actionlog { get; set; }

        [JsonProperty("display_arrow", NullValueHandling = NullValueHandling.Ignore)]
        public long DisplayArrow { get; set; }

        [JsonProperty("title_extra_text", NullValueHandling = NullValueHandling.Ignore)]
        public string TitleExtraText { get; set; }
    }

    public partial class InterestPeopleModel
    {
        [JsonProperty("card_type", NullValueHandling = NullValueHandling.Ignore)]
        public long? CardType { get; set; }

        [JsonProperty("itemid", NullValueHandling = NullValueHandling.Ignore)]
        public string Itemid { get; set; }

        [JsonProperty("scheme", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Scheme { get; set; }

        [JsonProperty("background_color", NullValueHandling = NullValueHandling.Ignore)]
        public long? BackgroundColor { get; set; }

        [JsonProperty("recom_remark", NullValueHandling = NullValueHandling.Ignore)]
        public string RecomRemark { get; set; }

        [JsonProperty("desc1", NullValueHandling = NullValueHandling.Ignore)]
        public string Desc1 { get; set; }

        [JsonProperty("user", NullValueHandling = NullValueHandling.Ignore)]
        public User User { get; set; }

        [JsonProperty("actionlog", NullValueHandling = NullValueHandling.Ignore)]
        public Actionlog Actionlog { get; set; }

        [JsonProperty("buttons", NullValueHandling = NullValueHandling.Ignore)]
        public List<ButtonModel> Buttons { get; set; }
    }
}