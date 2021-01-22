using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClassProject.Controllers
{
    public class ReferenceUrl
    {
        public static string ReferUrl(HttpRequestBase request)
        {
            return request.UrlReferrer == null ? "" : request.UrlReferrer.ToString();
        }
    }
}