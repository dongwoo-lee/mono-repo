const url = [
    {
        // My 공간
        id: "S01G01C001",
        icon: "iconsminds-shop",
    },

    {
        // My 디스크
        id: "S01G01C007",
        to: "/app/my/private",
        icon: "simple-icon-paper-plane",
    },
    {
        // 휴지통
        id: "S01G01C008",
        to: "/app/my/waste-basket",
        icon: "iconsminds-chemical",
    },
    {
        // My 선곡 순위
        id: "S01G01C037",
        to: "/app/my/music-ranking",
        icon: "iconsminds-big-data",
    },
    {
        // 제작
        id: "S01G01C002",
        icon: "iconsminds-film",
    },
    {
        // 프로그램
        id: "S01G01C009",
        to: "/app/products/program",
        icon: "iconsminds-blackboard",
    },
    {
        // 부조 SPOT
        id: "S01G01C010",
        to: "/app/products/scr-spot",
        icon: "",
    },
    {
        // 공유소재
        id: "S01G01C011",
        to: "/app/products/public",
        icon: "iconsminds-blackboard",
    },
    {
        // 취재물
        id: "S01G01C012",
        to: "/app/products/coverage",
        icon: "iconsminds-blackboard",
    },
    {
        // 프로소재
        id: "S01G01C013",
        to: "/app/products/pro-mt",
        icon: "iconsminds-blackboard",
    },
    {
        // 음원
        id: "S01G01C003",
        icon: "iconsminds-cd-2",
    },
    {
        // 음반기록실
        id: "S01G01C014",
        to: "/app/music/song",
        icon: "iconsminds-cd-2",
    },
    {
        // 효과음
        id: "S01G01C015",
        to: "/app/music/effect",
        icon: "iconsminds-clef",
    },
    {
        // SONG
        id: "S01G01C032",
        to: "/app/music/songcache",
        icon: "iconsminds-clef",
    },
    {
        // 전체 선곡 순위 
        id: "S01G01C038",
        to: "/app/music/statistics",
        icon: "iconsminds-big-data",
    },
    {
        // 광고
        id: "S01G01C004",
        icon: "iconsminds-coins",
    },
    {
        // 주조SB
        id: "S01G01C016",
        to: "/app/advertising/mcr-sb",
        icon: "iconsminds-optimization",
    },
    {
        // 부조SB
        id: "S01G01C017",
        to: "/app/advertising/scr-sb",
        icon: "iconsminds-optimization",
    },
    {
        // 프로그램CM
        id: "S01G01C018",
        to: "/app/advertising/pgm-cm",
        icon: "iconsminds-optimization",
    },
    {
        // CM
        id: "S01G01C019",
        to: "/app/advertising/cm",
        icon: "iconsminds-optimization",
    },
    {
        // 편성 MD
        id: "S01G01C005",
        icon: "iconsminds-engineering",
    },
    {
        // 주조SPOT
        id: "S01G01C020",
        to: "/app/combinationmd/mcr-spot",
        icon: "iconsminds-notepad",
    },
    {
        // Filler(PR)
        id: "S01G01C021",
        to: "/app/combinationmd/filler",
        icon: "iconsminds-notepad",
    },
    {
        // Filler(소재)
        id: "S01G01C022",
        to: "/app/combinationmd/filler-mt",
        icon: "iconsminds-notepad",
    },
    {
        // Filler(시간)
        id: "S01G01C023",
        to: "/app/combinationmd/filler-time",
        icon: "iconsminds-notepad",
    },
    {
        // Filler(기타)
        id: "S01G01C024",
        to: "/app/combinationmd/filler-etc",
        icon: "iconsminds-notepad",
    },
    {
        // 아카이브 (DL)
        id: "S01G01C006",
        // icon: "iconsminds-big-data",
        icon: "iconsminds-optimization",
        // icon: "iconsminds-posterous",
    },
    {
        // DL3
        id: "S01G01C033",
        to: "/app/monitoring/dl30",
        icon: "iconsminds-big-data",
    },
    {
        // 편성표
        id: "S01G01C034",
        to: "/app/monitoring/broadcastList",
        icon: "iconsminds-big-data",
    },
    {
        // 스튜디오 정보
        id: "S01G01C035",
        to: "/app/monitoring/studio",
        icon: "iconsminds-big-data",
    },
    {
        // 프로그램 정보
        id: "S01G01C036",
        to: "/app/monitoring/programInfo",
        icon: "iconsminds-big-data",
    },
    {
        // 큐시트
        id: "S01G01C025",
        icon: "iconsminds-testimonal",
    },
    {
        // 일일 큐시트
        id: "S01G01C026",
        to: "/app/cuesheet/day/list",
        icon: "iconsminds-testimonal",
    },
    {
        // 일일 큐시트 작성
        id: "S01G01C025",
        to: "/app/cuesheet/day/detail",
        icon: "iconsminds-testimonal",
    },
    {
        // 기본 큐시트
        id: "S01G01C027",
        to: "/app/cuesheet/week/list",
        icon: "iconsminds-testimonal",
    },
    {
        // 기본 큐시트 작성
        id: "S01G01C025",
        to: "/app/cuesheet/week/detail",
        icon: "iconsminds-testimonal",
    },
    {
        // 템플릿
        id: "S01G01C028",
        to: "/app/cuesheet/template/list",
        icon: "iconsminds-testimonal",
    },
    {
        // 템플릿 작성
        id: "S01G01C025",
        to: "/app/cuesheet/template/detail",
        icon: "iconsminds-testimonal",
    },
    {
        // 이전 큐시트 조회
        id: "S01G01C029",
        to: "/app/cuesheet/previous/list",
        icon: "iconsminds-testimonal",
    },
    {
        // 이전 큐시트
        id: "S01G01C025",
        to: "/app/cuesheet/previous/detail",
        icon: "iconsminds-testimonal",
    },
    {
        // 즐겨찾기
        id: "S01G01C030",
        to: "/app/cuesheet/favorite",
        icon: "iconsminds-testimonal",
    },
]

export default url;