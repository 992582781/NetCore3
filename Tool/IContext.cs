using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tool
{
    public class IContext
    {
        public static IHttpContextAccessor Accessor;
        public static HttpContext GetContext()
        {
            return Accessor.HttpContext;
        }
    }
}
