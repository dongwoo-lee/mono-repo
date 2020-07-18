
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
      {
        icon: "simple-icon-paper-plane",
        label: "menu.dev",
        to: "/app/dev"
      },
      {
        icon: "simple-icon-paper-plane",
        label: "menu.apidev",
        to: "/app/apidev"
      },
    ]
  },
  {
    id: "making",
    icon: "iconsminds-digital-drawing",
    label: "menu.making", // 제작
    subs: [
      {
        icon: "simple-icon-app-following",
        label: "menu.program", // 프로그램
        to: "/app/program",
      },
      {
        icon: "simple-icon-app-follow",
        label: "menu.relief-spot", // 부조 SPOT
        to: "/app/relief-spot",
      }, {
        icon: "simple-icon-app-unfollow",
        label: "menu.shared-material", // 공유소재
        to: "/app/shared-material",
      },
      {
        icon: "simple-icon-app-following",
        label: "menu.coverage", // 취재물
        to: "/app/coverage",
      },
      {
        icon: "simple-icon-app-following",
        label: "menu.pro-materials", // (구)프로소재
        to: "/app/pro-materials",
        // newWindow: true
      }
    ]
  },
  {
    id: "soundtrack", // 음원
    icon: "iconsminds-digital-drawing",
    label: "menu.soundtrack",
    subs: [
      {
      icon: "simple-icon-app-following",
      label: "menu.music-record-room", // 음반기록실
      to: "/app/music-record-room",
      },
      {
        icon: "simple-icon-app-follow",
        label: "menu.sound-effect", // 효과음
        to: "/app/sound-effect",
      },
    ]
  },
  {
    id: "advertising", // 광고
    icon: "iconsminds-digital-drawing",
    label: "menu.advertising",
    subs: [
      {
      icon: "simple-icon-app-following",
      label: "menu.casting-sb", // 주조SB
      to: "/app/casting-sb",
      },
      {
        icon: "simple-icon-app-follow",
        label: "menu.relief-sb",  // 부조SB
        to: "/app/relief-sb",
      },
      {
        icon: "simple-icon-app-follow",
        label: "menu.cm", // CM
        to: "/app/cm"
      },
    ]
  },
  {
    id: "combination-md", // 편성 MD
    icon: "iconsminds-digital-drawing",
    label: "menu.combination-md",
    subs: [
      {
      icon: "simple-icon-app-following",
      label: "menu.casting-spot", // 주조SPOT
      to: "/app/casting-spot",
      },
      {
        icon: "simple-icon-app-follow",
        label: "menu.filler-pr", // Filler(PR)
        to: "/app/filler-pr",
      },
      {
        icon: "simple-icon-app-follow",
        label: "menu.filler-material", // Filler(소재)
        to: "/app/filler-material",
      },
      {
        icon: "simple-icon-app-follow",
        label: "menu.filler-time", // Filler(시간)
        to: "/app/filler-time",
      },
      {
        icon: "simple-icon-app-follow",
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
