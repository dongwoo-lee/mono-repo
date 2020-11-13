// default options
export const defaultMenuType = 'menu-sub-hidden' // 'menu-default', 'menu-sub-hidden', 'menu-hidden';
export const subHiddenBreakpoint = 1440
export const menuHiddenBreakpoint = 768
export const defaultLocale = 'ko'
export const defaultDirection = 'ltr'
export const localeOptions = [
  { id: 'en', name: 'English LTR', direction: 'ltr' },
  { id: 'es', name: 'Español', direction: 'ltr' },
  { id: 'enrtl', name: 'English RTL', direction: 'rtl' }
]
export const defaultColor = 'light.blueolympic'

/* refresh token timer */
export const REFRESH_TOKEN_TIME = 60 * 60;
/* behavior system management code */
export const SYSTEM_MANAGEMENT_CODE  = 'S01G02C001';
/* behavior preview listening code */
export const PREVIEW_CODE  = 'S01G02C002';
/* behavior download code */
export const DOWNLOAD_CODE  = 'S01G02C003';
/* behavior authority*/
export const AUTHORITY_ADMIN = 'ADMIN';
/* behavior authority*/
export const AUTHORITY_MANAGER = 'MANAGER';