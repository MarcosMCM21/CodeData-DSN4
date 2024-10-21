using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Authentication;

namespace CodeData_Connection.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            switch (statusCode)
            {
                case 0:
                    return View("NotFound");
                case 401:
                    return View("401");
                case 403:
                    return View("403");
                case 404:
                    return View("404");
                default:
                    return View("500");
            }
        }
    }
}
