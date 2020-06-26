using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace MoviesAPI.Filters
{
    public class MyActionFilter: IActionFilter
    {
        private ILogger<MyActionFilter> _logger;

        public MyActionFilter(ILogger<MyActionFilter> logger)
        {
            _logger = logger;
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogWarning("OnActionExcecuting");
        }
        
        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogWarning("OnActionExcecuted");
        }
    }
}