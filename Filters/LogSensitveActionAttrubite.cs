using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;

namespace api.Filters;

// [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
public class LogSensitveActionAttrubite : ActionFilterAttribute
{
    public override void OnActionExecuted(ActionExecutedContext context)
    {
        Debug.WriteLine("Sensitive action executed !!!!!!!!!!");
    }
}