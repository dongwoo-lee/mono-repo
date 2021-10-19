using MAMBrowser.Common.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAMBrowser.DTO
{
    //템플릿PQSCON
    public class DTO_TEMPLATE_PQSCON
    {
        [SortName("PRODUCTID")]
        public string Id { get; set; }    //제작ID
        [SortName("ONAIRDATE")]
        public string Date { get; set; }    //방송날짜
        [SortName("PQSTYPE")]
        public string CartType { get; set; }    //카트타입
        [SortName("SEQNUM")]
        public string Num { get; set; }   //순번
        [SortName("CARTID")]
        public string FileId { get; set; }  //소재파일ID
        [SortName("CARTTYPE")]
        public string ItemType { get; set; }    //소재분류(유형)
        [SortName("EOM")]
        public string Duration { get; set; } //소재길이(시간)
        [SortName("TRANSTYPE")]
        public string TransType { get; set; }    //트랜스타입
        [SortName("INTROTIME")]
        public string InTime { get; set; }    //시작점
        [SortName("EXTROTIME")]
        public string OutTime { get; set; }    //끝점
        [SortName("FADEOUTTIME")]
        public string FadeOutTime { get; set; }    //페이드아웃시간
        [SortName("FADEINTIME")]
        public string FadeInTime { get; set; }    //페이드인시간
        [SortName("DESCRIPTION")]
        public string Memo { get; set; }   //메모
        [SortName("MAINTITLE")]
        public string MainTitle { get; set; }  //메인소재명
        [SortName("SUBTITLE")]
        public string SubTitle { get; set; }    //서브소재명
        
    }
}
