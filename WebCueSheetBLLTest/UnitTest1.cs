using DAP3.CueSheetCommon.DTO.Builder;
using DAP3.CueSheetCommon.DTO.Param;
using DAP3.CueSheetDAL.Factories.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace WebCueSheetBLLTest
{
    [TestClass]
    public class UnitTest1
    {
        private const string ConnectionString =
          "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.1.202)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=orcl)));User Id=MIROS;Password=MIROS;";

        public UnitTest1()
        {

            WebCueSheetFactory.Instance.SetConnectionString(ConnectionString);
        }
        [TestMethod]
        public void TestMethod1()
        {
         

            DAP3.CueSheetCommon.DTO.Param.CueSheetParamDTO dto = new CueSheetParamBuilder()
            .SetCueID(-1)
            .SetProductID("PM1505NA")
            .SetMedia("A")
            .SetPersonID("080035")
            .Build();

            DayCueSheetParamDTO daydto = new DayCueSheetParamBuilder()
                .SetBrdDate("20210928")
                .SetBrdTime(DateTime.Now)
                .Build();

            DAP3.CueSheetCommon.DTO.Param.CueSheetConParamDTO conDto1 = new CueSheetConParamBuilder()
               .SetChannelType("I")
               .SetSeqNum(1)
               
               .SetCartID("AC00239471")
               .SetCartCode("S01G01C014")
               .SetUseFlag("N")
               .SetMainTitle("효과_위풍당당BG123123")
               .SetSubTitle("2시 만세555555")
               .SetTranstype("L")
               .Build();
            DAP3.CueSheetCommon.DTO.Param.CueSheetConParamDTO conDto2 = new CueSheetConParamBuilder()
                .SetChannelType("I")
                .SetSeqNum(2)
                
                .SetCartCode("S01G01C021")
                .SetUseFlag("N")
                .SetCartID("FC00000166")
                .SetMainTitle("10초,친구처럼 다정한 연인처럼")
                .SetSubTitle("로고송")
                .SetTranstype("N")
                .Build();
            DAP3.CueSheetCommon.DTO.Param.CueSheetConParamDTO conDto3 = new CueSheetConParamBuilder()
                .SetChannelType("I")
                .SetSeqNum(3)
                
                .SetCartCode("S01G01C012")
                .SetUseFlag("N")
                .SetCartID("RC00229647")
                .SetMainTitle("41-1")
                .SetSubTitle("20180616")
                .SetTranstype("N")
                .Build();
            DAP3.CueSheetCommon.DTO.Param.CueSheetConParamDTO conDto4 = new CueSheetConParamBuilder()
            .SetChannelType("I")
            .SetSeqNum(4)
            
            .SetCartID("TS00007417")
            .SetCartCode("S01G01C024")
            .SetUseFlag("N")
            .SetMainTitle("12시 시보")
            .SetSubTitle("20180409")
            .SetTranstype("N")
            .Build();
            DAP3.CueSheetCommon.DTO.Param.CueSheetConParamDTO conDto5 = new CueSheetConParamBuilder()
            .SetChannelType("I")
            .SetSeqNum(5)
            
            .SetCartID("EC00044595")
            .SetCartCode("S01G01C015")
            .SetUseFlag("N")
            .SetMainTitle("뉴스포커스  브릿지3")
            .SetTranstype("N")
            .Build();
            DAP3.CueSheetCommon.DTO.Param.CueSheetConParamDTO conDto6 = new CueSheetConParamBuilder()
            .SetChannelType("I")
            .SetSeqNum(6)
            
            .SetCartID("221PI1BB00")
            .SetCartCode("S01G01C019")
            .SetUseFlag("Y")
            .SetOnairDate("20200622")
            .SetMainTitle("세계는 우리는 1부-전CM")
            .SetSubTitle("20200622")
            .SetTranstype("N")
            .Build();
            DAP3.CueSheetCommon.DTO.Param.CueSheetConParamDTO conDto7 = new CueSheetConParamBuilder()
            .SetChannelType("I")
            .SetSeqNum(7)
            
            .SetCartID("221PI2FF00")
            .SetCartCode("S01G01C018")
            .SetUseFlag("Y")
            .SetOnairDate("20200622")
            .SetMainTitle("세계는 우리는 2부-후CM")
            .SetSubTitle("20200622")
            .SetTranstype("N")
            .Build();
            DAP3.CueSheetCommon.DTO.Param.CueSheetConParamDTO conDto8 = new CueSheetConParamBuilder()
            .SetChannelType("I")
            .SetSeqNum(8)
            
            .SetCartID("ASG230929")
            .SetCartCode("S01G01C016")
            .SetUseFlag("Y")
            .SetOnairDate("20200623")
            .SetMainTitle("SB1534")
            .SetSubTitle("2020-03-04")
            .SetTranstype("N")
            .Build();

            DAP3.CueSheetCommon.DTO.Param.CueSheetConParamDTO conDto9 = new CueSheetConParamBuilder()
           .SetChannelType("N")
           .SetSeqNum(1)
           
           .SetCartID("AC00239471")
           .SetCartCode("S01G01C014")
           .SetUseFlag("N")
           .SetMainTitle("효과_위풍당당BG")
           .SetSubTitle("2시 만세")
           .SetTranstype("N")
           .Build();
            DAP3.CueSheetCommon.DTO.Param.CueSheetConParamDTO conDto10 = new CueSheetConParamBuilder()
                .SetChannelType("N")
                .SetSeqNum(2)
           
                .SetCartCode("S01G01C021")
                .SetUseFlag("N")
                .SetCartID("FC00000166")
                .SetMainTitle("10초,친구처럼 다정한 연인처럼")
                .SetSubTitle("로고송")
                .SetTranstype("N")
                .Build();
            DAP3.CueSheetCommon.DTO.Param.CueSheetConParamDTO conDto11 = new CueSheetConParamBuilder()
                .SetChannelType("N")
                .SetSeqNum(3)
           
                .SetCartCode("S01G01C012")
                .SetUseFlag("N")
                .SetCartID("RC00229647")
                .SetMainTitle("41-1")
                .SetSubTitle("20180616")
                .SetTranstype("N")
                .Build();
            DAP3.CueSheetCommon.DTO.Param.CueSheetConParamDTO conDto12 = new CueSheetConParamBuilder()
            .SetChannelType("N")
            .SetSeqNum(4)
           
            .SetCartID("TS00007417")
            .SetCartCode("S01G01C024")
            .SetUseFlag("N")
            .SetMainTitle("12시 시보")
            .SetSubTitle("20180409")
            .SetTranstype("N")
            .Build();
            DAP3.CueSheetCommon.DTO.Param.CueSheetConParamDTO conDto13 = new CueSheetConParamBuilder()
            .SetChannelType("N")
            .SetSeqNum(20)
           
            .SetCartID("EC00044595")
            .SetCartCode("S01G01C015")
            .SetUseFlag("N")
            .SetMainTitle("뉴스포커스  브릿지3")
            .SetTranstype("N")
            .Build();
            DAP3.CueSheetCommon.DTO.Param.CueSheetConParamDTO conDto14 = new CueSheetConParamBuilder()
            .SetChannelType("N")
            .SetSeqNum(21)
           
            .SetCartID("221PI1BB00")
            .SetCartCode("S01G01C019")
            .SetUseFlag("Y")
            .SetOnairDate("20200622")
            .SetMainTitle("세계는 우리는 1부-전CM")
            .SetSubTitle("20200622")
            .SetTranstype("N")
            .Build();
            DAP3.CueSheetCommon.DTO.Param.CueSheetConParamDTO conDto15 = new CueSheetConParamBuilder()
            .SetChannelType("N")
            .SetSeqNum(22)
           
            .SetCartID("221PI2FF00")
            .SetCartCode("S01G01C018")
            .SetUseFlag("Y")
            .SetOnairDate("20200622")
            .SetMainTitle("세계는 우리는 2부-후CM")
            .SetSubTitle("20200622")
            .SetTranstype("N")
            .Build();
            DAP3.CueSheetCommon.DTO.Param.CueSheetConParamDTO conDto16 = new CueSheetConParamBuilder()
            .SetChannelType("N")
            .SetSeqNum(23)
           
            .SetCartID("ASG231629")
            .SetCartCode("S01G01C016")
            .SetUseFlag("Y")
            .SetOnairDate("20200623")
            .SetMainTitle("SB1534")
            .SetSubTitle("2020-03-04")
            .SetTranstype("N")
            .Build();

            List<DAP3.CueSheetCommon.DTO.Param.CueSheetConParamDTO> lists = new List<DAP3.CueSheetCommon.DTO.Param.CueSheetConParamDTO>();
            lists.Add(conDto1);
            lists.Add(conDto2);
            lists.Add(conDto3);
            lists.Add(conDto4);
            lists.Add(conDto5);
            lists.Add(conDto6);
            lists.Add(conDto7);
            lists.Add(conDto8);

            lists.Add(conDto9);
            lists.Add(conDto10);
            lists.Add(conDto11);
            lists.Add(conDto12);
            lists.Add(conDto13);
            lists.Add(conDto14);
            lists.Add(conDto15);
            lists.Add(conDto16);
            //IEnumerable<PrintParamDTO> printParams

            List<PrintParamDTO> prints = new List<PrintParamDTO>();
            prints.Add(new PrintParamDTO() { CODE = "CSGP10", CONTENTS = "내용1", ETC = "비고1", SEQNUM = 1, STARTTIME = "051113" });
            var result = WebCueSheetFactory.Instance.CueSheetRepository.SaveDayCueSheet(dto, daydto, lists, null, prints, null);
        }

        [TestMethod]
        public void TestDefultSaveCueSheet()
        {
            //WebCueSheetFactory.Instance.CueSheetRepository.SaveDefaultCueSheet(~~~)
        }

        //광고 (프로그램, 날짜)
        [TestMethod]
        public void Test1()
        {
            //var result = WebCueSheetFactory.Instance.CueSheetRepository.GetCMGroup("PM1415NA", "A", "20190603");
            var result = WebCueSheetFactory.Instance.CueSheetRepository.GetSponsor("2MS", "20190603");

        }
        

        [TestMethod]
        public void Test3()
        {
            var result = WebCueSheetFactory.Instance.CueSheetRepository.GetDayCueSheet("PM0500NF", 677);
        }

        [TestMethod]
        public void Test4()
        {
            int[] test = { 673, 674, 675, 676 };
            var result = WebCueSheetFactory.Instance.CueSheetRepository.GetDefultCueSheet("PM0500NF", test);
        }
    }
}
