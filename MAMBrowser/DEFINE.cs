using System;

namespace MAMBrowser
{
    /// <summary>
    /// 결과 코드
    /// </summary>
    public enum RESUlT_CODES
    {
        SUCCESS = 0,
        INVALID_DATA = 1,
        DENY_ACCESS = 2,
        DB_ERROR = 3,
        TOKEN_ERROR = 4,
        TOKEN_EXPIRATION = 5,
        SERVICE_ERROR = 6,
        APPLIED_NONE_WARN = 7,
        FILE_NOT_FOUND = 8
    }

    public enum LOG_CATEGORIES
    {
        CALL,   //debug system call method
        AUTO_EXECUTE,    
        MANUAL_EXECUTE,
        KNOWN_EXCEPTION,
        UNKNOWN_EXCEPTION,

    }
    [Flags]
    public enum MusicSearchTypes1 : int
    {
        None = 0,
        Internal = 1,
        External = 2,
        Classic = 4,
        All = 7,

    }
    [Flags]
    public enum MusicSearchTypes2 : int
    {
        song_idx,   //전체
        song_name_idx,  //곡명
        songname_artist_idx,    // 곡명 + 아티스트
        song_artist_idx,    //아티스트
        song_disc_arr_num_idx,  //배열번호
        song_country_name_idx   //국가명
    }
    [Flags]
    public enum GradeTypes : int
    {
        None = 0,
        Heat = 1,   //히트 p4=1
        Forbid = 2, //금지  p5=2
        Caution = 4,    //주의 p8=1
        HarmfulJuveniles = 8,    //청소년 유해 p7=1
        All = 15, //p4=1&p5=2&p8=1&p7=1
    }

}
