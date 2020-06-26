using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace MoviesAPI.Filters
{
    public class MyExceptionFilter: ExceptionFilterAttribute
    {
        private readonly ILogger<MyExceptionFilter> _logger;
        
        public MyExceptionFilter(ILogger<MyExceptionFilter> logger)
        {
            _logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, context.Exception.Message);
            
            base.OnException(context);
        }
    }
}