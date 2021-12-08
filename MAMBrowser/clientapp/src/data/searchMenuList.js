let searchMenuList = {
  data() {
    return {
      menuList_items: [
        { id: "1", name: "음반 기록실" },
        { id: "2", name: "공유소재" },
        { id: "3", name: "(구)프로소재" },
        {
          id: "4",
          name: "광고협찬",
          items: [
            {
              id: "4_1",
              name: "부조SB",
            },
            {
              id: "4_2",
              name: "부조SPOT",
            },
            {
              id: "4_3",
              name: "프로그램CM",
            },
            {
              id: "4_4",
              name: "CM",
            },
          ],
        },
        {
          id: "5",
          name: "제작",
          items: [
            {
              id: "5_1",
              name: "취재물",
            },
            {
              id: "5_2",
              name: "효과음",
            },
            {
              id: "5_3",
              name: "Filler(PR)",
            },
            {
              id: "5_3",
              name: "Filler(소재)",
            },
            {
              id: "5_3",
              name: "Filler(시간)",
            },
            {
              id: "5_3",
              name: "Filler(기타)",
            },
          ],
        },
        {
          id: "6",
          name: "기타",
          items: [
            {
              id: "6_1",
              name: "MY디스크",
            },
            {
              id: "6_2",
              name: "프로그램",
            },
            {
              id: "6_3",
              name: "DL3",
            },
            {
              id: "6_4",
              name: "주조SB",
            },
            {
              id: "6_5",
              name: "주조SPOT",
            },
          ],
        },
      ],
      searchData:
        [
          {
            num: 0,
            id: "",
            cartcode: "S01G01C020",
            name: "음반 기록실",
            options:
              [
                {
                  text: "대분류",
                  value:
                    [
                      { text: '국내', value: 1 },
                      { text: '국외', value: 2 },
                      { text: '클래식', value: 4 }
                    ],
                  type: "C"
                },
                {
                  text: "검색옵션",
                  value:
                    [
                      { text: '히트', value: 1 },
                      { text: '금지', value: 2 },
                      { text: '주의', value: 4 },
                      { text: '청소년 유해', value: 8 },
                    ],
                  type: "C"
                },
                {
                  text: "소분류",
                  value:
                    [
                      { text: '전체', value: 'song_idx' },
                      { text: '곡명', value: 'song_name_idx' },
                      { text: '곡명/아티스트', value: 'songname_artist_idx' },
                      { text: '아티스트', value: 'song_artist_idx' },
                      { text: '배열번호', value: 'song_disc_arr_num_idx' },
                      { text: '국가명', value: 'song_country_name_idx' }
                    ],
                  type: "S"
                },
                {
                  text: "검색어",
                  value: "",
                  type: "T"
                }
              ],
            columns:
              [
                { dataField: "rowNO", caption: "순서" },
                { dataField: "name", caption: "곡명" },
                { dataField: "artistName", caption: "아티스트" },
                { dataField: "duration", caption: "재생시간" },
                { dataField: "albumName", caption: "음반명" },
                { dataField: "releaseDate", caption: "발매년도" },
                { dataField: "composer", caption: "작곡가" },
                { dataField: "writer", caption: "작사가" },
                { dataField: "sequenceNO", caption: "배열번호" },
                { dataField: "__slot:actions", caption: "추가작업" }
              ]
          },
          {
            num: 1,
            id: "PUBLIC_FILE",
            cartcode: "S01G01C011",
            name: "공유소재",
            options: [
              {
                id: "startDate",
                text: "시작일",
                value: "",
                type: "D",
              },
              {
                id: "endDate",
                text: "종료일",
                value: "",
                type: "D",
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
                name: "publicSecond",
              },
              {
                id: "editor",
                text: "제작자",
                value: [],
                type: "S",
                name: "userList",
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
            columns:
              [
                { dataField: "rowNO", caption: "순서" },
                { dataField: "categoryName", caption: "분류" },
                { dataField: "title", caption: "제목" },
                { dataField: "memo", caption: "메모" },
                { dataField: "fileExt", caption: "파일형식" },
                { dataField: "fileSize", caption: "파일사이즈" },
                { dataField: "audioFormat", caption: "오디오포맷" },
                { dataField: "userName", caption: "제작자" },
                { dataField: "editedDtm", caption: "등록일시" },
                { dataField: "__slot:actions", caption: "추가작업" }
              ],
          },
          {
            num: 2,
            id: "OLD_PRO",
            cartcode: "S01G01C013",
            name: "(구)프로소재",
            options: [
              {
                id: "startDate",
                text: "시작일(마스터링)",
                value: "",
                type: "D",
              },
              {
                id: "endDate",
                text: "종료일(마스터링)",
                value: "",
                type: "D",
              },
              {
                id: "type",
                text: "구분",
                value: [
                  { value: '', text: '선택해주세요.' },
                  { value: 'Y', text: '방송중' },
                  { value: 'N', text: '폐지' },
                ],
                type: "S",
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
              { dataField: "rowNO", caption: "순서" },
              { dataField: "name", caption: "소재명" },
              { dataField: "categoryName", caption: "분류" },
              { dataField: "duration", caption: "길이(초)" },
              { dataField: "editorName", caption: "제작자" },
              { dataField: "editDtm", caption: "최종편집일시" },
              { dataField: "masteringDtm", caption: "마스터링 일시" },
              { dataField: "proType", caption: "타입" },
              { cellTemplate: "play_Template", caption: "작업", width: "65", alignment: "center" }
            ],
          },
          {
            num: 3,
            id: "SCR_SB",
            cartcode: "S01G01C017",
            name: "부조SB",
            options: [
              {
                id: "brddate",
                text: "방송일",
                value: "",
                type: "D",
              },
              {
                id: "media",
                text: "매체",
                value: [],
                type: "S",
                name: "medias",
              },
              {
                id: "pgm",
                text: "사용처",
                value: [],
                type: "S",
                name: "pgmCodes",
              },
            ],
            columns: [
              { cellTemplate: "row_Template", caption: "순서", whdth: "40px", alignment: "center" },
              { dataField: "id", caption: "SB ID" },
              { dataField: "name", caption: "SB명" },
              { dataField: "length", caption: "길이(초)", format: "00:hh:mm.0", dataType: "date" },
              { dataField: "pgmName", caption: "사용처명" },
              { dataField: "capacity", caption: "용량(초)" },
              { dataField: "status", caption: "상태" },
              { dataField: "editorName", caption: "담당자" },

            ],
          },
          {
            num: 4,
            id: "SCR_SPOT",
            cartcode: "S01G01C010",
            name: "부조SPOT",
            options: [
              {
                id: "startDate",
                text: "시작일",
                value: "",
                type: "D",
              },
              {
                id: "endDate",
                text: "종료일",
                value: "",
                type: "D",
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
              { dataField: "rowNO", caption: "순서" },
              { dataField: "name", caption: "소재명" },
              { dataField: "categoryName", caption: "분류" },
              { dataField: "duration", caption: "길이(초)" },
              { dataField: "brdDT", caption: "방송일" },
              { dataField: "editorName", caption: "제작자" },
              { dataField: "pgmName", caption: "사용처명" },
              { dataField: "masteringDtm", caption: "마스터링 일시" },
              { dataField: "__slot:actions", caption: "추가작업" }
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
              }
            ],
            columns: [
              { cellTemplate: "row_Template", caption: "순서", width: "50px", alignment: "center" },
              { dataField: "name", caption: "CM명" },
              { dataField: "length", caption: "길이(초)", format: "00:hh:mm.0", dataType: "date", width: "95px", },
              { dataField: "capacity", caption: "용량(초)", width: "70px" },
              { dataField: "status", caption: "상태", width: "80px" },
              { dataField: "editorName", caption: "담당자", width: "60px" },
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

              }
            ],
            columns: [
              { cellTemplate: "row_Template", caption: "순서", whdth: "40px", alignment: "center" },
              { dataField: "name", caption: "CM명" },
              { dataField: "length", caption: "길이(초)", format: "00:hh:mm.0", dataType: "date" },
              { dataField: "capacity", caption: "용량(초)" },
              { dataField: "status", caption: "상태" },
              { dataField: "editorName", caption: "담당자" },
            ],
          },
          {
            num: 7,
            id: "REPOTE",
            cartcode: "S01G01C012",
            name: "취재물",
            options: [
              {
                id: "startDate",
                text: "시작일",
                value: "",
                type: "D",

              },
              {
                id: "endDate",
                text: "종료일",
                value: "",
                type: "D",

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
                value:
                  [
                    { value: 'Y', text: '마스터링 완료한 소재만 보기' },
                  ],
                type: "C",
                selectVal: [],
              },
            ],
            columns: [
              { dataField: "rowNO", caption: "순서" },
              { dataField: "name", caption: "소재명" },
              { dataField: "reporter", caption: "취재인" },
              { dataField: "pgmName", caption: "사용처명" },
              { dataField: "brdDT", caption: "방송일" },
              { dataField: "duration", caption: "길이(초)" },
              { dataField: "editorName", caption: "제작자" },
              { dataField: "editDtm", caption: "최종편집일시" },
              { dataField: "masteringDtm", caption: "마스터링 일시" },
              { dataField: "__slot:actions", caption: "추가작업" }
            ],
          },
          {
            num: 8,
            id: "",
            name: "효과음",
            options: [
              {
                text: "검색어",
                value: "",
                type: "T",

              },
            ],
            columns: [
              { dataField: "rowNO", caption: "순서" },
              { dataField: "name", caption: "효과음명" },
              { dataField: "description", caption: "설명" },
              { dataField: "duration", caption: "길이(초)" },
              { dataField: "audioFormat", caption: "오디오 포맷" },
              { dataField: "__slot:actions", caption: "추가작업" }
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
              }
            ],
            columns: [
              { dataField: "rowNO", caption: "순서" },
              { dataField: "categoryName", caption: "분류" },
              { dataField: "name", caption: "소재명" },
              { dataField: "brdDT", caption: "방송유효일" },
              { dataField: "duration", caption: "길이(초)" },
              { dataField: "editorName", caption: "편집자" },
              { dataField: "editDtm", caption: "편집일자" },
              { dataField: "masteringDtm", caption: "마스터링일자" },
              { dataField: "__slot:actions", caption: "추가작업" }
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

              }
            ],
            columns: [
              { dataField: "rowNO", caption: "순서" },
              { dataField: "categoryName", caption: "분류" },
              { dataField: "name", caption: "소재명" },
              { dataField: "brdDT", caption: "방송유효일" },
              { dataField: "duration", caption: "길이(초)" },
              { dataField: "editorName", caption: "편집자" },
              { dataField: "editDtm", caption: "편집일자" },
              { dataField: "masteringDtm", caption: "마스터링일자" },
              { dataField: "__slot:actions", caption: "추가작업" }
            ],
          },
          {
            num: 11,
            id: "FILLER_TIME",
            cartcode: "S01G01C023",
            name: "Filler(시간)",
            options: [
              {
                id: "startDate",
                text: "시작일",
                value: "",
                type: "D",

              },
              {
                id: "endDate",
                text: "종료일",
                value: "",
                type: "D",

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
              { dataField: "rowNO", caption: "순서" },
              { dataField: "name", caption: "소재명" },
              { dataField: "brdDate", caption: "방송개시일" },
              { dataField: "endDT", caption: "방송종료일" },
              { dataField: "status", caption: "상태" },
              { dataField: "duration", caption: "길이(초)" },
              { dataField: "editorName", caption: "편집자" },
              { dataField: "editDtm", caption: "최종편집일시" },
              { dataField: "fileName", caption: "파일명" },
              { dataField: "masteringDtm", caption: "마스터링일자" },
              { dataField: "__slot:actions", caption: "추가작업" }
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

              }
            ],
            columns: [
              { dataField: "rowNO", caption: "순서" },
              { dataField: "categoryName", caption: "분류" },
              { dataField: "name", caption: "소재명" },
              { dataField: "brdDT", caption: "방송유효일" },
              { dataField: "duration", caption: "길이(초)" },
              { dataField: "editorName", caption: "편집자" },
              { dataField: "editDtm", caption: "편집일자" },
              { dataField: "masteringDtm", caption: "마스터링일자" },
              { dataField: "__slot:actions", caption: "추가작업" }
            ],
          },
          {
            num: 13,
            id: "MyDisk",
            cartcode: "S01G01C007",
            name: "MY디스크",
            options: [
              {
                id: "startDate",
                text: "시작일",
                value: "",
                type: "D",

              },
              {
                id: "endDate",
                text: "종료일",
                value: "",
                type: "D",

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
              { dataField: "rowNO", caption: "순서" },
              { dataField: "title", caption: "제목" },
              { dataField: "memo", caption: "메모" },
              { dataField: "fileExt", caption: "파일형식" },
              { dataField: "fileSize", caption: "파일사이즈" },
              { dataField: "audioFormat", caption: "오디오포맷" },
              { dataField: "editedDtm", caption: "등록일시" },
              { dataField: "__slot:actions", caption: "추가작업" }
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
              { dataField: "rowNO", caption: "순서" },
              { dataField: "mediaName", caption: "매체" },
              { dataField: "name", caption: "프로그램명" },
              { dataField: "brdDT", caption: "방송일" },
              { dataField: "brdTime", caption: "방송시간" },
              { dataField: "status", caption: "상태" },
              { dataField: "duration", caption: "길이(초)" },
              { dataField: "editorName", caption: "제작자" },
              { dataField: "editDtm", caption: "최종편집일시" },
              { dataField: "reqCompleteDtm", caption: "방송의뢰일시" },
              { dataField: "__slot:actions", caption: "추가작업" }
            ],
          },
          {
            num: 15,
            id: "DL30",
            name: "DL3",
            options: [
              {
                id: "brddate",
                text: "방송일",
                value: "",
                type: "D",
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
              { dataField: "rowNO", caption: "순서" },
              { dataField: "brdDate", caption: "송출일시" },
              { dataField: "recName", caption: "녹음소재명" },
              { dataField: "sourceID", caption: "Source ID" },
              { dataField: "duration", caption: "녹음분량" },
              //{ dataField: "fileSize", caption: "파일사이즈" ,cellTemplate: "play_Template"},//변환해야함
              { caption: "파일사이즈", cellTemplate: "calculate_Template" },//변환해야함
              { dataField: "regDtm", caption: "등록일시" },
              { dataField: "__slot:actions", caption: "추가작업" }
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
              { cellTemplate: "row_Template", caption: "순서", whdth: "10px" },
              { dataField: "id", caption: "SB ID" },
              { dataField: "name", caption: "SB명" },
              { dataField: "length", caption: "길이(초)", format: "00:hh:mm.0", dataType: "date", width: "100px" },
              { dataField: "capacity", caption: "용량(초)" },
              { dataField: "status", caption: "상태" },
              { dataField: "editorName", caption: "담당자" },
            ],
          },
          {
            num: 17,
            id: "MCR_SPOT",
            cartcode: "S01G01C020",
            name: "주조SPOT",
            options: [
              {
                id: "startDate",
                text: "시작일",
                value: "",
                type: "D",

              },
              {
                id: "endDate",
                text: "종료일",
                value: "",
                type: "D",

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
              { dataField: "rowNO", caption: "순서" },
              { dataField: "name", caption: "소재명" },
              { dataField: "brdDT", caption: "방송일" },
              { dataField: "status", caption: "상태" },
              { dataField: "duration", caption: "길이(초)" },
              { dataField: "editorName", caption: "제작자" },
              { dataField: "editDtm", caption: "최종편집일시" },
              { dataField: "reqCompleteDtm", caption: "방송의뢰일시" },
              { dataField: "__slot:actions", caption: "추가작업" },
            ],
          }
        ]
    }
  },
  watch: {
  },
  computed: {
  },
  mounted() {
  },
  methods: {
  }
}
export default searchMenuList;