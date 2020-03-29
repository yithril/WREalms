using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Connections.Features;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WanderlustRealms.Hubs
{
    static public class SignalrExtensions
    {
        static public HttpContext GetHttpContext(this HubCallerContext context) =>
           context
             ?.Features
             .Select(x => x.Value as IHttpContextFeature)
             .FirstOrDefault(x => x != null)
             ?.HttpContext;

        static public T GetQueryParameterValue<T>(this IQueryCollection httpQuery, string queryParameterName) =>
           httpQuery.TryGetValue(queryParameterName, out var value) && value.Any()
             ? (T)Convert.ChangeType(value.FirstOrDefault(), typeof(T))
             : default;
    }
}
