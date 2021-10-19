using MAMBrowser.Common.Foundation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAMBrowser.DTO
{
    //기본편성표 리스트
    public class DTO_BASEINFOLIST
    {
        [JsonProperty("day")]
        [SortName("ONAIRDAY")]
        public string Day { get; set; }    //편성표
        [SortName("ONAIRTIME")]
        public string OnAirTime { get; set; }    //방송일
        [JsonProperty("id")]
        [SortName("PRODUCTID")]
        public string Id { get; set; }   //프로그램ID
        [SortName("EVENTNAME")]
        public string Name { get; set; }  //이벤트네임
    }
}
