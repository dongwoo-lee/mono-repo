using MAMBrowser.Common.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAMBrowser.DTO
{
    public class DTO_CM_CONTENT : DTO_FILEBASE
    {

        //public int Order { get; set; }   //순번
        public string Advertiser { get; set; }  //광고주명
        public string ID { get; set; }  //소재ID
        public string Name { get; set; }    //소재명
        public string Length { get; set; }  //길이 (xx:xx)
        public string CodingUserID { get; set; }  //코딩인ID
        public string CodingUserName { get; set; }  //코딩인
        public string CodingDT { get; set; }   //코딩일
        public string Format { get; set; }  //text값
    }
}
