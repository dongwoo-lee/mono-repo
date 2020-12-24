// default options
export const defaultMenuType = 'menu-sub-hidden' // 'menu-default', 'menu-sub-hidden', 'menu-hidden';
export const subHiddenBreakpoint = 1440
export const menuHiddenBreakpoint = 768
export const defaultLocale = 'ko'
export const defaultDirection = 'ltr'
export const localeOptions = [
  { id: 'en', name: 'English LTR', direction: 'ltr' },
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
/* file upload maximum size */
export const MAXIMUM_FILE_SIZE = (1000 * 1000 * 1000) * 2;  //  KB -> MB -> GB * 2 = 2GB;
/* file upload accept */
export const FILE_UPLOAD_ACCEPT = '.mp3,.wav';
/* file upload extensions */
export const FILE_UPLOAD_EXTENSIONS = ['mp3', 'wav'];
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
/* minimum date */
export const MINIMUM_DATE = '2018-01-01';
/* max date: 현재날짜 기준 + 몇일 */
export const MAXIMUM_DATE_NUM = 1;
/* 최대 검색 일수 현재날짜 기준 */
export const MAXIMUM_SEARCH_DATE = 30;