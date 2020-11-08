using MAMBrowser.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAMBrowser.Middleware
{
    public class RequestLogAttribute : ActionFilterAttribute
    {
        private readonly LogService _logService;
        public RequestLogAttribute(LogService logService)
        {
            _logService = logService;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var a = context;
            foreach (var arg in context.ActionArguments)
            {
                // arg.Value 직렬화해서 로깅할것.
                // 직렬화 데이터를 계속 붙여서 하나의 문자열로 만들것.


                // file  이름은 업로드되는 파일이므로 제외할것. file:true로 반환할것. (있는경우)
                //대용량 파일업로드로 바꾸면 될지 모르겠음.
            }
        }
    }
}
