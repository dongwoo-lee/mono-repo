const date = new Date();

function get_date_str(date) {
  var sYear = date.getFullYear();
  var sMonth = date.getMonth() + 1;
  var sDate = date.getDate();

  sMonth = sMonth > 9 ? sMonth : "0" + sMonth;
  sDate = sDate > 9 ? sDate : "0" + sDate;

  return sYear + "-" + sMonth + "-" + sDate;
}

var toDay = get_date_str(date);
date.setDate(date.getDate() - 7);
var wDay = get_date_str(date);
date.setDate(date.getDate() - 83);
var mDay = get_date_str(date);

let searchMenuList = {
  data() {
    return {
      menuList_items: [
        { id: "1", name: "음반 기록실" },
        //{ id: "2", name: "공유소재" },
        { id: "2", name: "Song" },
        { id: "3", name: "프로소재" },
        { id: "4", name: "MY디스크" },
        {
          id: "5",
          name: "광고협찬",
          items: [
            //2022-04-01 부조SB 운영방침에 의해 폐기됨.
            // {
            //   id: "3_1",
            //   name: "부조SB",
            // },
            {
              id: "5_2",
              name: "부조SPOT(협찬)",
            },
            {
              id: "5_3",
              name: "프로그램CM",
            },
            {
              id: "5_4",
              name: "CM",
            },
          ],
        },
        {
          id: "6",
          name: "제작",
          items: [
            {
              id: "6_1",
              name: "취재물",
            },
            {
              id: "6_2",
              name: "효과음",
            },
            {
              id: "6_3",
              name: "Filler(PR)",
            },
            {
              id: "6_3",
              name: "Filler(소재)",
            },
            {
              id: "6_3",
              name: "Filler(시간)(변동/고정소재)",
            },
            {
              id: "6_3",
              name: "Filler(기타)",
            },
          ],
        },
        {
          id: "7",
          name: "기타",
          items: [
            // {
            //   id: "5_1",
            //   name: "MY디스크",
            // },
            {
              id: "7_1",
              name: "프로그램",
            },
            {
              id: "7_2",
              name: "DL3",
            },
            {
              id: "7_3",
              name: "주조SB",
            },
            {
              id: "7_4",
              name: "주조SPOT",
            },
          ],
        },
      ],
      searchData: [
        {
          num: 0,
          id: "MUSIC",
          cartcode: "S01G01C014",
          name: "음반 기록실",
          options: [
            {
              id: "searchtype1",
              text: "대분류",
              value: [
                { text: "국내", value: 1 },
                { text: "국외", value: 2 },
                { text: "클래식", value: 4 },
              ],
              type: "C",
              selectVal: [],
            },
            {
              id: "searchtype2",
              text: "소분류",
              value: [
                { text: "전체", value: "song_idx" },
                { text: "곡명", value: "song_name_idx" },
                { text: "곡명/아티스트", value: "songname_artist_idx" },
                { text: "아티스트", value: "song_artist_idx" },
                { text: "배열번호", value: "song_disc_arr_num_idx" },
                { text: "국가명", value: "song_country_name_idx" },
              ],
              type: "S",
              selectVal: "song_idx",
            },
            {
              id: "gradetype",
              text: "검색옵션",
              value: [
                { text: "히트", value: 1 },
                { text: "금지", value: 2 },
                { text: "주의", value: 4 },
                { text: "청소년 유해", value: 8 },
              ],
              type: "C",
            },
            {
              id: "searchtext",
              text: "검색어",
              value: "",
              type: "T",
            },
          ],
          columns: [
            {
              dataField: "rowNO",
              minWidth: "50",
              caption: "순서",
              alignment: "center",
              allowSorting: false,
            },
            {
              dataField: "name",
              caption: "곡명",
              alignment: "center",
              allowSorting: false,
            },
            {
              dataField: "artistName",
              caption: "아티스트",
              alignment: "center",
              allowSorting: false,
            },
            {
              dataField: "duration",
              caption: "재생시간",
              alignment: "center",
              allowSorting: false,
            },
            {
              dataField: "albumName",
              caption: "음반명",
              alignment: "center",
              allowSorting: false,
            },
            {
              dataField: "releaseDate",
              caption: "발매년도",
              alignment: "center",
              allowSorting: false,
            },
            {
              dataField: "composer",
              caption: "작곡가",
              alignment: "center",
              allowSorting: false,
            },
            {
              dataField: "writer",
              caption: "작사가",
              alignment: "center",
              allowSorting: false,
            },
            {
              dataField: "sequenceNO",
              caption: "배열번호",
              alignment: "center",
              allowSorting: false,
            },
            {
              cellTemplate: "play_Template",
              fixed: true,
              fixedPosition: "right",
              minWidth: "50",
              caption: "작업",
              alignment: "center",
              allowSorting: false,
            },
          ],
        },
        {
          num: 2,
          id: "OLD_PRO",
          cartcode: "S01G01C013",
          name: "프로소재",
          options: [
            {
              startText: "시작일(방송의뢰)",
              endText: "종료일(방송의뢰)",
              type: "SED",
              st_selectVal: mDay,
              end_selectVal: toDay,
              requiredVal: false
            },
            {
              id: "type",
              text: "구분",
              value: [
                { value: "", text: "선택해주세요." },
                { value: "Y", text: "방송중" },
                { value: "N", text: "폐지" },
              ],
              type: "S",
              selectVal: "Y",
            },
            {
              id: "cate",
              text: "분류",
              value: [],
              type: "S",
              name: "pro",
            },
            {
              id: "editor",
              text: "제작자",
              value: [],
              type: "S",
              name: "userList",
            },
            {
              id: "name",
              text: "소재명",
              value: "",
              type: "T",
            },
          ],
          columns: [
            {
              dataField: "rowNO",
              minWidth: "50",
              caption: "순서",
              alignment: "center",
              allowSorting: false,
            },
            {
              dataField: "name",
              caption: "소재명",
              alignment: "center",

            },
            {
              dataField: "categoryName",
              caption: "분류",
              alignment: "center",

            },
            {
              dataField: "duration",
              caption: "길이(초)",
              alignment: "center",

            },
            {
              dataField: "editorName",
              caption: "제작자",
              alignment: "center",

            },
            {
              dataField: "editDtm",
              caption: "최종편집일시",
              alignment: "center",

            },
            {
              dataField: "masteringDtm",
              caption: "방송의뢰 일시",
              alignment: "center",

            },
            {
              dataField: "proType",
              caption: "타입",
              alignment: "center",

            },
            {
              cellTemplate: "play_Template",
              fixed: true,
              fixedPosition: "right",
              minWidth: "50",
              caption: "작업",
              alignment: "center",

            },
          ],
        },
        {
          num: 4,
          id: "SCR_SPOT",
          cartcode: "S01G01C010",
          name: "부조SPOT(협찬)",
          options: [
            {
              startText: "시작일",
              endText: "종료일",
              type: "SED",
              st_selectVal: wDay,
              end_selectVal: toDay,
              maxMonth: 3,
              requiredVal: true
            },
            {
              id: "media",
              text: "매체",
              value: [],
              type: "S",
              name: "medias",
            },
            {
              id: "pgmname",
              text: "사용처명",
              value: "",
              type: "T",
            },
            {
              id: "editor",
              text: "제작자",
              value: [],
              type: "S",
              name: "userList",
            },
            {
              id: "name",
              text: "소재명",
              value: "",
              type: "T",
            },
          ],
          columns: [
            {
              dataField: "rowNO",
              minWidth: "50",
              caption: "순서",
              alignment: "center",
              allowSorting: false,
            },
            {
              dataField: "name",
              caption: "소재명",
              alignment: "center",

            },
            {
              dataField: "categoryName",
              caption: "분류",
              alignment: "center",

            },
            {
              dataField: "duration",
              cellTemplate: "duration_Template",
              caption: "길이(초)",
              alignment: "center",

            },
            {
              dataField: "brdDT",
              caption: "방송일",
              alignment: "center",

            },
            {
              dataField: "editorName",
              caption: "제작자",
              alignment: "center",

            },
            {
              dataField: "pgmName",
              caption: "사용처명",
              alignment: "center",

            },
            {
              dataField: "masteringDtm",
              caption: "방송의뢰 일시",
              alignment: "center",

            },
            {
              cellTemplate: "play_Template",
              fixed: true,
              fixedPosition: "right",
              minWidth: "50",
              caption: "작업",
              alignment: "center",

            },
          ],
        },
        {
          num: 5,
          id: "PGM_CM",
          cartcode: "S01G01C018",
          name: "프로그램CM",
          options: [
            {
              id: "brddate",
              text: "방송일",
              value: "",
              type: "D",
              selectVal: toDay,
              requiredVal: true
            },
            {
              id: "media",
              text: "매체",
              value: [],
              type: "S",
              name: "medias",
            },
            {
              id: "pgmname",
              text: "사용처",
              value: "",
              type: "T",
            },
          ],
          columns: [
            //{ cellTemplate: "row_Template", caption: "순서", width: "7%", alignment: "center", allowSorting: false },
            {
              dataField: "rowNO",
              caption: "순서",
              alignment: "center",
              allowSorting: false,
            },
            {
              dataField: "name",
              caption: "CM명",
              alignment: "center",
              allowSorting: false,
            },
            {
              dataField: "length",
              caption: "길이(초)",
              alignment: "center",
              allowSorting: false,
            },
            {
              dataField: "capacity",
              caption: "용량(초)",
              alignment: "center",
              allowSorting: false,
            },
            {
              dataField: "status",
              caption: "상태",
              alignment: "center",
              allowSorting: false,
            },
            {
              dataField: "editorName",
              caption: "담당자",
              alignment: "center",
              allowSorting: false,
            },
          ],
        },
        {
          num: 6,
          id: "CM",
          cartcode: "S01G01C019",
          name: "CM",
          options: [
            {
              id: "brddate",
              text: "방송일",
              value: "",
              type: "D",
              selectVal: toDay,
              requiredVal: true
            },
            {
              id: "media",
              text: "매체",
              value: [],
              type: "S",
              name: "medias",
            },
            {
              id: "cate",
              text: "분류",
              value: [],
              type: "S",
              name: "cm",
            },
            {
              id: "pgmname",
              text: "사용처",
              value: "",
              type: "T",
            },
          ],
          columns: [
            //{ cellTemplate: "row_Template", caption: "순서", width: "7%", alignment: "center", allowSorting: false },
            {
              dataField: "rowNO",
              caption: "순서",
              alignment: "center",
              allowSorting: false,
            },
            {
              dataField: "name",
              caption: "CM명",
              alignment: "center",
              allowSorting: false,
            },
            {
              dataField: "length",
              caption: "길이(초)",
              alignment: "center",
              allowSorting: false,
            },
            {
              dataField: "capacity",
              caption: "용량(초)",
              alignment: "center",
              allowSorting: false,
            },
            {
              dataField: "status",
              caption: "상태",
              alignment: "center",
              allowSorting: false,
            },
            {
              dataField: "editorName",
              caption: "담당자",
              alignment: "center",
              allowSorting: false,
            },
          ],
        },
        {
          num: 7,
          id: "REPOTE",
          cartcode: "S01G01C012",
          name: "취재물",
          options: [
            {
              id: "brddate",
              text: "방송일",
              value: "",
              type: "D",
              selectVal: toDay,
              requiredVal: true
            },
            {
              id: "cate",
              text: "분류",
              value: [],
              type: "S",
              name: "report",
            },
            {
              id: "pgmname",
              text: "사용처명",
              value: "",
              type: "T",
            },
            {
              id: "reporterName",
              text: "취재인",
              value: "",
              type: "T",
            },
            {
              id: "editor",
              text: "제작자",
              value: [],
              type: "S",
              name: "reportUserList",
            },
            {
              id: "name",
              text: "소재명",
              value: "",
              type: "T",
            },
            {
              id: "ismastering",
              text: "검색옵션",
              value: [{ value: "Y", text: "방송의뢰 완료한 소재만 보기" }],
              type: "C",
              selectVal: ["Y"],
            },
          ],
          columns: [
            {
              dataField: "rowNO",
              minWidth: "50",
              caption: "순서",
              alignment: "center",
              allowSorting: false,
            },
            {
              dataField: "name",
              caption: "소재명",
              alignment: "center",

            },
            {
              dataField: "reporter",
              caption: "취재인",
              alignment: "center",

            },
            {
              dataField: "pgmName",
              caption: "사용처명",
              alignment: "center",

            },
            {
              dataField: "brdDT",
              caption: "방송일",
              alignment: "center",

            },
            {
              dataField: "duration",
              cellTemplate: "duration_Template",
              caption: "길이(초)",
              alignment: "center",
              allowSorting: true,
            },
            {
              dataField: "editorName",
              caption: "제작자",
              alignment: "center",

            },
            {
              dataField: "editDtm",
              caption: "최종편집일시",
              alignment: "center",

            },
            {
              dataField: "masteringDtm",
              caption: "방송의뢰 일시",
              alignment: "center",

            },
            {
              cellTemplate: "play_Template",
              fixed: true,
              fixedPosition: "right",
              minWidth: "50",
              caption: "작업",
              alignment: "center",

            },
          ],
        },
        {
          num: 8,
          id: "EFFECT",
          name: "효과음",
          cartcode: "S01G01C015",
          options: [
            {
              id: "searchText",
              text: "검색어",
              value: "",
              type: "T",
            },
          ],
          columns: [
            {
              dataField: "rowNO",
              minWidth: "50",
              caption: "순서",
              alignment: "center",
              allowSorting: false,
            },
            {
              dataField: "name",
              caption: "효과음명",
              alignment: "center",
              allowSorting: false,
            },
            {
              dataField: "description",
              caption: "설명",
              alignment: "center",
              allowSorting: false,
            },
            {
              dataField: "duration",
              caption: "길이(초)",
              alignment: "center",
              allowSorting: false,
            },
            {
              dataField: "audioFormat",
              caption: "오디오 포맷",
              alignment: "center",
              allowSorting: false,
            },
            {
              cellTemplate: "play_Template",
              fixed: true,
              fixedPosition: "right",
              minWidth: "50",
              caption: "작업",
              alignment: "center",
              allowSorting: false,
            },
          ],
        },
        {
          num: 9,
          id: "FILLER_PR",
          cartcode: "S01G01C021",
          name: "Filler(PR)",
          options: [
            {
              id: "brddate",
              text: "방송일",
              value: "",
              type: "D",
              selectVal: toDay,
              requiredVal: true
            },
            {
              id: "cate",
              text: "분류",
              value: [],
              type: "S",
              name: "fillerPr",
            },
            {
              id: "editor",
              text: "제작자",
              value: [],
              type: "S",
              name: "mdUserList",
            },
            {
              id: "name",
              text: "소재명",
              value: "",
              type: "T",
            },
          ],
          columns: [
            {
              dataField: "rowNO",
              minWidth: "50",
              caption: "순서",
              alignment: "center",
              allowSorting: false,
            },
            {
              dataField: "categoryName",
              caption: "분류",
              alignment: "center",

            },
            {
              dataField: "name",
              caption: "소재명",
              alignment: "center",

            },
            {
              dataField: "brdDT",
              caption: "방송유효일",
              alignment: "center",

            },
            {
              dataField: "duration",
              cellTemplate: "duration_Template",
              caption: "길이(초)",
              alignment: "center",
              allowSorting: true,
            },
            {
              dataField: "editorName",
              caption: "편집자",
              alignment: "center",

            },
            {
              dataField: "editDtm",
              caption: "편집일자",
              alignment: "center",

            },
            {
              dataField: "masteringDtm",
              caption: "방송의뢰일자",
              alignment: "center",

            },
            {
              cellTemplate: "play_Template",
              fixed: true,
              fixedPosition: "right",
              minWidth: "50",
              caption: "작업",
              alignment: "center",

            },
          ],
        },
        {
          num: 10,
          id: "FILLER_MT",
          cartcode: "S01G01C022",
          name: "Filler(소재)",
          options: [
            {
              id: "brddate",
              text: "방송일",
              value: "",
              type: "D",
              selectVal: toDay,
              requiredVal: true
            },
            {
              id: "cate",
              text: "분류",
              value: [],
              type: "S",
              name: "fillerGeneral",
            },
            {
              id: "editor",
              text: "제작자",
              value: [],
              type: "S",
              name: "mdUserList",
            },
            {
              id: "name",
              text: "소재명",
              value: "",
              type: "T",
            },
          ],
          columns: [
            {
              dataField: "rowNO",
              minWidth: "50",
              caption: "순서",
              alignment: "center",
              allowSorting: false,
            },
            {
              dataField: "categoryName",
              caption: "분류",
              alignment: "center",

            },
            {
              dataField: "name",
              caption: "소재명",
              alignment: "center",

            },
            {
              dataField: "brdDT",
              caption: "방송유효일",
              alignment: "center",

            },
            {
              dataField: "duration",
              cellTemplate: "duration_Template",
              caption: "길이(초)",
              alignment: "center",

            },
            {
              dataField: "editorName",
              caption: "편집자",
              alignment: "center",

            },
            {
              dataField: "editDtm",
              caption: "편집일자",
              alignment: "center",

            },
            {
              dataField: "masteringDtm",
              caption: "방송의뢰일자",
              alignment: "center",

            },
            {
              cellTemplate: "play_Template",
              fixed: true,
              fixedPosition: "right",
              minWidth: "50",
              caption: "작업",
              alignment: "center",

            },
          ],
        },
        {
          num: 11,
          id: "FILLER_TIME",
          cartcode: "S01G01C023",
          name: "Filler(시간)(변동/고정소재)",
          options: [
            // {
            //   id: "startDate",
            //   text: "시작일",
            //   value: "",
            //   type: "D",
            //   selectVal: toDay,

            // },
            // {
            //   id: "endDate",
            //   text: "종료일",
            //   value: "",
            //   type: "D",
            //   selectVal: toDay,
            // },
            {
              startText: "시작일",
              endText: "종료일",
              type: "SED",
              st_selectVal: wDay,
              end_selectVal: toDay,
              maxMonth: 3,
              requiredVal: true
            },
            {
              id: "media",
              text: "매체",
              value: [],
              type: "S",
              name: "medias",
            },
            {
              id: "cate",
              text: "분류",
              value: [],
              type: "S",
              name: "fillerTimetone",
            },
            {
              id: "status",
              text: "상태",
              value: [],
              type: "S",
              name: "reqStatus",
            },
            {
              id: "editor",
              text: "제작자",
              value: [],
              type: "S",
              name: "mdUserList",
            },
          ],
          columns: [
            {
              dataField: "rowNO",
              minWidth: "50",
              caption: "순서",
              alignment: "center",
              allowSorting: false,
            },
            {
              dataField: "name",
              caption: "소재명",
              alignment: "center",

            },
            {
              dataField: "brdDate",
              caption: "방송개시일",
              alignment: "center",

            },
            {
              dataField: "endDT",
              caption: "방송종료일",
              alignment: "center",

            },
            {
              dataField: "status",
              caption: "상태",
              alignment: "center",

            },
            {
              dataField: "duration",
              cellTemplate: "duration_Template",
              caption: "길이(초)",
              alignment: "center",
              allowSorting: true,
            },
            {
              dataField: "editorName",
              caption: "편집자",
              alignment: "center",

            },
            {
              dataField: "editDtm",
              caption: "최종편집일시",
              alignment: "center",

            },
            {
              dataField: "fileName",
              caption: "파일명",
              alignment: "center",

            },
            {
              dataField: "masteringDtm",
              caption: "방송의뢰일자",
              alignment: "center",

            },
            {
              cellTemplate: "play_Template",
              fixed: true,
              fixedPosition: "right",
              minWidth: "50",
              caption: "작업",
              alignment: "center",

            },
          ],
        },
        {
          num: 12,
          id: "FILLER_ETC",
          cartcode: "S01G01C024",
          name: "Filler(기타)",
          options: [
            {
              id: "brddate",
              text: "방송일",
              value: "",
              type: "D",
              selectVal: toDay,
              requiredVal: true
            },
            {
              id: "cate",
              text: "분류",
              value: [],
              type: "S",
              name: "fillerETC",
            },
            {
              id: "editor",
              text: "제작자",
              value: [],
              type: "S",
              name: "mdUserList",
            },
            {
              id: "name",
              text: "소재명",
              value: "",
              type: "T",
            },
          ],
          columns: [
            {
              dataField: "rowNO",
              minWidth: "50",
              caption: "순서",
              alignment: "center",
              allowSorting: false,
            },
            {
              dataField: "categoryName",
              caption: "분류",
              alignment: "center",

            },
            {
              dataField: "name",
              caption: "소재명",
              alignment: "center",

            },
            {
              dataField: "brdDT",
              caption: "방송유효일",
              alignment: "center",

            },
            {
              cellTemplate: "duration_Template",
              caption: "길이(초)",
              alignment: "center",
              allowSorting: true,
            },
            {
              dataField: "editorName",
              caption: "편집자",
              alignment: "center",

            },
            {
              dataField: "editDtm",
              caption: "편집일자",
              alignment: "center",

            },
            {
              dataField: "masteringDtm",
              caption: "방송의뢰일자",
              alignment: "center",

            },
            {
              cellTemplate: "play_Template",
              fixed: true,
              fixedPosition: "right",
              minWidth: "50",
              caption: "작업",
              alignment: "center",

            },
          ],
        },
        {
          num: 13,
          id: "MyDisk",
          cartcode: "S01G01C007",
          name: "MY디스크",
          options: [
            {
              startText: "시작일",
              endText: "종료일",
              type: "SED",
              st_selectVal: "",
              end_selectVal: "",
              requiredVal: false
            },
            {
              id: "title",
              text: "제목",
              value: "",
              type: "T",
            },
            {
              id: "memo",
              text: "메모",
              value: "",
              type: "T",
            },
          ],
          columns: [
            {
              dataField: "rowNO",
              caption: "순서",
              alignment: "center",
              allowSorting: false,
            },
            {
              cellTemplate: "maxWidth_ellipsis_title",
              dataField: "title",
              caption: "제목",
              alignment: "center",
              width: "500",
              allowSorting: true,
            },
            {
              cellTemplate: "maxWidth_ellipsis_memo",
              dataField: "memo",
              caption: "메모",
              width: "250",
              alignment: "center",
              allowSorting: true,
            },
            {
              dataField: "fileSize",
              cellTemplate: "calculate_KB_Template",
              caption: "파일사이즈",
              alignment: "center",
              allowSorting: true,
            },
            {
              dataField: "editedDtm",
              caption: "등록일시",
              alignment: "center",

            },
            {
              cellTemplate: "play_Template",
              fixed: true,
              fixedPosition: "right",
              minWidth: "50",
              caption: "작업",
              alignment: "center",

            },
          ],
        },
        {
          num: 14,
          id: "PGM",
          cartcode: "S01G01C009",
          name: "프로그램",
          options: [
            {
              id: "brddate",
              text: "방송일",
              value: "",
              type: "D",
              selectVal: toDay,
              requiredVal: false
            },
            {
              id: "media",
              text: "매체",
              value: [],
              type: "S",
              name: "medias",
            },
            {
              id: "pgmname",
              text: "프로그램",
              value: [],
              type: "S",
              name: "pgmCodes",
            },
            {
              id: "editorname",
              text: "제작자",
              value: [],
              type: "S",
              name: "pdUserList",
            },
          ],
          columns: [
            {
              dataField: "rowNO",
              minWidth: "50",
              caption: "순서",
              alignment: "center",
              allowSorting: false,
            },
            {
              dataField: "mediaName",
              caption: "매체",
              alignment: "center",

            },
            {
              dataField: "name",
              caption: "프로그램명",
              alignment: "center",

            },
            {
              dataField: "brdDT",
              caption: "방송일",
              alignment: "center",

            },
            {
              dataField: "brdTime",
              caption: "방송시간",
              alignment: "center",

            },
            {
              dataField: "status",
              caption: "상태",
              alignment: "center",

            },
            {
              dataField: "duration",
              cellTemplate: "duration_Template",
              caption: "길이(초)",
              alignment: "center",
              allowSorting: true,
            },
            {
              dataField: "editorName",
              caption: "제작자",
              alignment: "center",

            },
            {
              dataField: "editDtm",
              caption: "최종편집일시",
              alignment: "center",

            },
            {
              dataField: "reqCompleteDtm",
              caption: "방송의뢰일시",
              alignment: "center",

            },
            {
              cellTemplate: "play_Template",
              fixed: true,
              fixedPosition: "right",
              minWidth: "50",
              caption: "작업",
              alignment: "center",

            },
          ],
        },
        {
          num: 15,
          id: "DL30",
          cartcode: "S01G01C006",
          name: "DL3",
          options: [
            {
              id: "brddate",
              text: "방송일",
              value: "",
              type: "D",
              selectVal: toDay,
              requiredVal: true
            },
            {
              id: "deviceSeq",
              text: "단말",
              value: [],
              type: "S",
              name: "dlDevice",
            },
            {
              id: "media",
              text: "매체",
              value: [],
              type: "S",
              name: "medias",
            },
            {
              id: "name",
              text: "소재명",
              value: "",
              type: "T",
            },
          ],
          columns: [
            {
              dataField: "rowNO",
              minWidth: "50",
              caption: "순서",
              alignment: "center",
              allowSorting: false,
            },
            {
              dataField: "brdDate",
              caption: "송출일시",
              alignment: "center",

            },
            {
              dataField: "recName",
              caption: "녹음소재명",
              alignment: "center",

            },
            {
              dataField: "sourceID",
              caption: "Source ID",
              alignment: "center",

            },
            {
              dataField: "duration",
              cellTemplate: "duration_Template",
              caption: "녹음분량",
              alignment: "center",
              allowSorting: true,
            },
            {
              dataField: "fileSize",
              cellTemplate: "calculate_MB_Template",
              caption: "파일사이즈",
              alignment: "center",
              allowSorting: true,
            }, //변환해야함
            {
              dataField: "regDtm",
              caption: "등록일시",
              alignment: "center",

            },
            {
              cellTemplate: "play_Template",
              fixed: true,
              fixedPosition: "right",
              minWidth: "50",
              caption: "작업",
              alignment: "center",

            },
          ],
        },
        {
          num: 16,
          id: "MCR_SB",
          cartcode: "S01G01C016",
          name: "주조SB",
          options: [
            {
              id: "brddate",
              text: "방송일",
              value: "",
              type: "D",
              selectVal: toDay,
              requiredVal: true
            },
            {
              id: "media",
              text: "매체",
              value: [],
              type: "S",
              name: "medias",
            },
          ],
          columns: [
            //{ cellTemplate: "row_Template", caption: "순서", width: "7%", alignment: "center", allowSorting: false },
            {
              dataField: "rowNO",
              caption: "순서",
              alignment: "center",
              allowSorting: false,
            },
            {
              dataField: "id",
              caption: "SB ID",
              alignment: "center",
              allowSorting: false,
            },
            {
              dataField: "name",
              caption: "SB명",
              alignment: "center",
              allowSorting: false,
            },
            {
              dataField: "length",
              caption: "길이",
              alignment: "center",
              allowSorting: false,
            },
            {
              dataField: "capacity",
              caption: "용량",
              alignment: "center",
              allowSorting: false,
            },
            {
              dataField: "status",
              caption: "상태",
              alignment: "center",
              allowSorting: false,
            },
            {
              dataField: "editorName",
              caption: "담당자",
              alignment: "center",
              allowSorting: false,
            },
          ],
        },
        {
          num: 17,
          id: "MCR_SPOT",
          cartcode: "S01G01C020",
          name: "주조SPOT",
          options: [
            {
              startText: "시작일",
              endText: "종료일",
              type: "SED",
              st_selectVal: wDay,
              end_selectVal: toDay,
              maxMonth: 3,
              requiredVal: true
            },
            {
              id: "media",
              text: "매체",
              value: [],
              type: "S",
              name: "medias",
            },
            {
              id: "spotid",
              text: "사용처",
              value: [],
              type: "S",
              name: "mcrSpot",
            },
            {
              id: "editor",
              text: "제작자",
              value: [],
              type: "S",
              name: "mdUserList",
            },
          ],
          columns: [
            {
              dataField: "rowNO",
              minWidth: "50",
              caption: "순서",
              alignment: "center",
              allowSorting: false,
            },
            {
              dataField: "name",
              caption: "소재명",
              alignment: "center",

            },
            {
              dataField: "brdDT",
              caption: "방송일",
              alignment: "center",

            },
            {
              dataField: "status",
              caption: "상태",
              alignment: "center",

            },
            {
              cellTemplate: "duration_Template",
              caption: "길이(초)",
              alignment: "center",
              allowSorting: true,
            },
            {
              dataField: "editorName",
              caption: "제작자",
              alignment: "center",

            },
            {
              dataField: "editDtm",
              caption: "최종편집일시",
              alignment: "center",

            },
            {
              dataField: "reqCompleteDtm",
              caption: "방송의뢰일시",
              alignment: "center",

            },
            {
              cellTemplate: "play_Template",
              fixed: true,
              fixedPosition: "right",
              minWidth: "50",
              caption: "작업",
              alignment: "center",

            },
          ],
        },
        {
          // Song
          num: 18,
          id: "SONG",
          cartcode: "S01G01C032",
          name: "Song",
          options: [
            {
              id: "albumName",
              text: "음반명",
              value: "",
              type: "T",
            },
            {
              id: "artistName",
              text: "아티스트",
              value: "",
              type: "T",
            },
            {
              id: "title",
              text: "곡명",
              value: "",
              type: "T",
            },
          ],
          columns: [
            {
              dataField: "rowNO",
              minWidth: "50",
              caption: "순서",
              alignment: "center",
              allowSorting: false,
            },
            {
              cellTemplate: "maxWidth_ellipsis_name",
              dataField: "name",
              caption: "곡명",
              alignment: "center",
              width: "300",
              allowSorting: true,
            },
            {
              dataField: "artistName",
              caption: "아티스트",
              alignment: "center",
              width: "300",
            },
            {
              dataField: "albumName",
              caption: "음반명",
              alignment: "center",
              width: "300",
            },
            {
              dataField: "duration",
              cellTemplate: "duration_Template",
              caption: "길이(초)",
              alignment: "center",
              allowSorting: true,
            },
            {
              dataField: "musicID",
              caption: "Music ID",
              alignment: "center",
            },
            {
              dataField: "encodeDate",
              caption: "인코딩일시",
              alignment: "center",

            },
            {
              cellTemplate: "play_Template",
              fixed: true,
              fixedPosition: "right",
              minWidth: "50",
              caption: "작업",
              alignment: "center",

            },
          ],
        },
      ],
    };
  },
};
export default searchMenuList;
