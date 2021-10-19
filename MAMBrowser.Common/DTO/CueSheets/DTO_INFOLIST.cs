using MAMBrowser.Common.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAMBrowser.DTO
{
    //일일큐시트 리스트
    public class DTO_INFOLIST
    {
        [SortName("ONAIRDAY")]
        public string Day { get; set; }    //편성표 (기본, 일일)
        [SortName("STARTDATE")]
        public string Date { get; set; }    //시작날짜
        [SortName("ONAIRTIME")]
        public string OnAirTime { get; set; }    //방송일
        [SortName("PRODUCTID")]
        public string Id { get; set; }   //프로그램ID
        [SortName("EVENTNAME")]
        public string Name { get; set; }  //이벤트네임
        [SortName("MEDIA")]
        public string Media { get; set; }    //매체
        
    }
}
