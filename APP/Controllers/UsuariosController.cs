﻿using CodeData_Connection.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeData_Connection.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class UsuariosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
