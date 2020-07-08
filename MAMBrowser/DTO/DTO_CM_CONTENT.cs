using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAMBrowser.DTO
{
    public class DTO_CM_CONTENT
    {
        public string Order { get; set; }   //순번
        public string Advertiser { get; set; }  //광고주명
        public string MaterialID { get; set; }  //소재ID
        public string MaterlalName { get; set; }    //소재명
        public string Length { get; set; }  //길이 (xx:xx)
        public string CodingUserName { get; set; }  //코딩인
        public string CodingDT { get; set; }   //코딩일
        public string Format { get; set; }  //text값
        public string FilePath { get; set; }    // 파일경로
    }
}
