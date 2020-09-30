using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SprintBetApi.Controllers
{
    public class SprintBetBaseController : ControllerBase
    {
        /// <summary>
        ///     Helper uri builder method
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static Uri GetBaseUri(HttpRequest request, string path)
        {
            var uriBuilder = new UriBuilder
            {
                Scheme = request.Scheme,
                Host = request.Host.Host,
                Port = request.Host.Port.GetValueOrDefault(-1),
                Path = path
            };

            return uriBuilder.Uri;
        }
    }
}