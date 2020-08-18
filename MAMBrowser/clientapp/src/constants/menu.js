
const data = [
  {
    id: "private",
    icon: "iconsminds-shop",
    label: "menu.private", // MY 공간
    subs: [
      {
        icon: "simple-icon-paper-plane",
        label: "menu.private",
        to: "/app/private"
      },
      {
        icon: "iconsminds-chemical",
        label: "menu.waste-basket", // 휴지통
        to: "/app/waste-basket"
      },
    ]
  },
  {
    id: "making",
    icon: "iconsminds-blackboard",
    label: "menu.making", // 제작
    subs: [
      {
        icon: "iconsminds-blackboard",
        label: "menu.program", // 프로그램
        to: "/app/program",
      },
      {
        icon: "iconsminds-blackboard",
        label: "menu.relief-spot", // 부조 SPOT
        to: "/app/relief-spot",
      }, {
        icon: "iconsminds-blackboard",
        label: "menu.shared-material", // 공유소재
        to: "/app/shared-material",
      },
      {
        icon: "iconsminds-blackboard",
        label: "menu.coverage", // 취재물
        to: "/app/coverage",
      },
      {
        icon: "iconsminds-blackboard",
        label: "menu.pro-materials", // (구)프로소재
        to: "/app/pro-materials",
        // newWindow: true
      }
    ]
  },
  {
    id: "soundtrack", // 음원
    icon: "iconsminds-clef",
    label: "menu.soundtrack",
    subs: [
      {
        icon: "iconsminds-clef",
        label: "menu.music-record-room", // 음반기록실
        to: "/app/music-record-room",
      },
      {
        icon: "iconsminds-clef",
        label: "menu.sound-effect", // 효과음
        to: "/app/sound-effect",
      },
    ]
  },
  {
    id: "advertising", // 광고
    icon: "iconsminds-optimization",
    label: "menu.advertising",
    subs: [
      {
        icon: "iconsminds-optimization",
        label: "menu.casting-sb", // 주조SB
        to: "/app/casting-sb",
      },
      {
        icon: "iconsminds-optimization",
        label: "menu.relief-sb",  // 부조SB
        to: "/app/relief-sb",
      },
      {
        icon: "iconsminds-optimization",
        label: "menu.cm", // CM
        to: "/app/cm"
      },
    ]
  },
  {
    id: "combination-md", // 편성 MD
    icon: "iconsminds-notepad",
    label: "menu.combination-md",
    subs: [
      {
        icon: "iconsminds-notepad",
        label: "menu.casting-spot", // 주조SPOT
        to: "/app/casting-spot",
      },
      {
        icon: "iconsminds-notepad",
        label: "menu.filler-pr", // Filler(PR)
        to: "/app/filler-pr",
      },
      {
        icon: "iconsminds-notepad",
        label: "menu.filler-material", // Filler(소재)
        to: "/app/filler-material",
      },
      {
        icon: "iconsminds-notepad",
        label: "menu.filler-time", // Filler(시간)
        to: "/app/filler-time",
      },
      {
        icon: "iconsminds-notepad",
        label: "menu.filler-other", // Filler(기타)
        to: "/app/filler-other",
      }
    ]
  },
  {
    id: "dl3",
    icon: "iconsminds-three-arrow-fork",
    label: "menu.dl3", // DL3.0
    to: "/app/dl3"
  },
];
export default data;
