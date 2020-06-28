
const data = [
  {
    id: "private",
    icon: "iconsminds-shop",
    label: "menu.private-space", // 개인공간
    to: "/app/private",
    subs: [{
      icon: "simple-icon-paper-plane",
      label: "menu.private-space",
      to: "/app/private"
    },
    ]
  },
  {
    id: "waste-basket",
    icon: "iconsminds-chemical",
    label: "menu.waste-basket", // 휴지통
    to: "/app/waste-basket",
    subs: [{
      icon: "simple-icon-paper-plane",
      label: "menu.waste-basket",
      to: "/app/waste-basket"
    },
    ]
  },
  {
    id: "making",
    icon: "iconsminds-digital-drawing",
    label: "menu.making", // 제작
    to: "/app/making",
    subs: [
      {
        icon: "simple-icon-app-following",
        label: "menu.program", // 프로그램
        to: "/app/program",
        newWindow: true
      },
      {
        icon: "simple-icon-app-follow",
        label: "menu.relief-spot", // 부조 SPOT
        to: "/app/relief-spot",
        newWindow: true
      }, {
        icon: "simple-icon-app-unfollow",
        label: "menu.shared-material", // 공유소재
        to: "/app/shared-material",
        newWindow: true
      },
      {
        icon: "simple-icon-app-following",
        label: "menu.coverage", // 취재물
        to: "/app/coverage",
        newWindow: true
      },
      {
        icon: "simple-icon-app-following",
        label: "menu.pro-materials", // (구)프로소재
        to: "/app/pro-materials",
        newWindow: true
      }
    ]
  },
  {
    id: "soundtrack",
    icon: "iconsminds-digital-drawing",
    label: "menu.soundtrack",
    to: "/app/soundtrack",
    subs: [
      {
      icon: "simple-icon-app-following",
      label: "menu.music-record-room",
      to: "/app/music-record-room",
      newWindow: true
      },
      {
        icon: "simple-icon-app-follow",
        label: "menu.sound-effect",
        to: "/app/sound-effect",
        newWindow: true
      },
    ]
  },
  {
    id: "advertising",
    icon: "iconsminds-digital-drawing",
    label: "menu.advertising",
    to: "/app/advertising",
    subs: [
      {
      icon: "simple-icon-app-following",
      label: "menu.casting-sb",
      to: "/app/casting-sb",
      newWindow: true
      },
      {
        icon: "simple-icon-app-follow",
        label: "menu.relief-sb",
        to: "/app/relief-sb",
        newWindow: true
      },
    ]
  },
  {
    id: "cm",
    icon: "iconsminds-three-arrow-fork",
    label: "menu.cm",
    to: "/app/cm"
  },
  {
    id: "combination-md",
    icon: "iconsminds-digital-drawing",
    label: "menu.combination-md",
    to: "/app/combination-md",
    subs: [
      {
      icon: "simple-icon-app-following",
      label: "menu.casting-spot",
      to: "/app/casting-spot",
      newWindow: true
      },
      {
        icon: "simple-icon-app-follow",
        label: "menu.filler-pr",
        to: "/app/filler-pr",
        newWindow: true
      },
      {
        icon: "simple-icon-app-follow",
        label: "menu.filler-material",
        to: "/app/filler-material",
        newWindow: true
      },
      {
        icon: "simple-icon-app-follow",
        label: "menu.filler-time",
        to: "/app/filler-time",
        newWindow: true
      },
      {
        icon: "simple-icon-app-follow",
        label: "menu.filler-other",
        to: "/app/filler-other",
        newWindow: true
      }
    ]
  },
  {
    id: "dl3",
    icon: "iconsminds-three-arrow-fork",
    label: "menu.dl3",
    to: "/app/dl3"
  },
  {
    id: "second-menu",
    icon: "iconsminds-chemical",
    label: "menu.second-menu",
    to: "/app/second-menu",
    subs: [{
      icon: "simple-icon-paper-plane",
      label: "menu.second",
      to: "/app/second-menu/second"
    },
    ]
  },
  {
    id: "pages",
    icon: "iconsminds-digital-drawing",
    label: "menu.pages",
    to: "/user/login",
    subs: [
      {
        icon: "simple-icon-user-following",
        label: "menu.login",
        to: "/user/login",
        // newWindow: true
      },
      // {
      //   icon: "simple-icon-user-follow",
      //   label: "menu.register",
      //   to: "/user/register",
      //   newWindow: true
      // },
      // {
      //   icon: "simple-icon-user-unfollow",
      //   label: "menu.forgot-password",
      //   to: "/user/forgot-password",
      //   newWindow: true
      // },
      // {
      //   icon: "simple-icon-user-following",
      //   label: "menu.reset-password",
      //   to: "/user/reset-password",
      //   newWindow: true
      // }
    ]
  },
];
export default data;
