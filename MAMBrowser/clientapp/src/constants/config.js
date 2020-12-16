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
/* route name */
export const ROUTE_NAMES = {
  PRIVATE: 'private',
  SHARED: 'public',
}
/* file upload accept */
export const FILE_UPLOAD_ACCEPT = '.mp2,.mp3,.wav';
/* file upload extensions */
export const FILE_UPLOAD_EXTENSIONS = ['mp2', 'mp3', 'wav'];
/* system management access */
export const SYSTEM_MANAGEMENT_ACCESS_PAGE_CODE = ['config', 'log'];
/* AUTHORITY*/
export const AUTHORITY = 'authority';
/* ROLE */
export const ROLE = 'role';
/* USER ID */
export const USER_ID = 'user_id';
/* ACCESS_TOKEN */
export const ACCESS_TOKEN = 'access_token';
/* input maxlength */
export const INPUT_MAX_LENGTH = 200;
