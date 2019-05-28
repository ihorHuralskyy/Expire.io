using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Expire.io.DTOs;
using Expire.io.Helpers;
using Expire.io.Models.Data;
using Expire.io.Models.Entities;
using Expire.io.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Expire.io.Controllers
{
    public class ManagerController : Controller
    {
        private readonly IManagerService _managerService;

        public ManagerController(IManagerService managerService)
        {
            _managerService = managerService;
        }

        public IActionResult AllUsersByManager()
        {
            var list = _managerService.AllUsersByManager();

            return View(list);
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Notify(string email)
        {
            _managerService.Notify(email);

            return Ok();
        }

    }
}