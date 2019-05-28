using System;
using System.Linq;
using System.Threading.Tasks;
using Expire.io.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Expire.io.Services.Contracts;

namespace Expire.io.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;

        public AdminController(
            IAdminService adminService)
        {
            _adminService = adminService;
        }
        public async Task<IActionResult> AllUsers()
        {
            var allUsers = await _adminService.AllUsers();

            return View(allUsers.ToList());
        }

        [HttpGet]
        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(AdminAddUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                { 
                    var result = await _adminService.CreateUser(model);
                    if (result.Result.Succeeded)
                    {
                        return Ok(new { Success = true });
                    }
                    else
                    {
                        foreach (var item in result.Result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, item.Description);
                        }

                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }

            return BadRequest("Your data is not valid");
        }


        public async Task<IActionResult> DeleteUser(string username)
        {            
            var result = await _adminService.DeleteUser(username);
            if (result.Result.Succeeded)
            {
                return Ok(new { resp = "User was deleted", id = result.User.Id });
            }
            else
            {
                return BadRequest(new { resp = "Unexpected server error" });
            }
        }

        public IActionResult MakeUserAnAdmin(string username)
        {

            var result = _adminService.MakeUserAnAdmin(username);
            if (result.Result.Succeeded)
            {
                return Ok(new
                {
                    resp = result.User.UserName.ToString() + " was added to role Admin ", id = result.User.Id
                });
            }
            else
            {
                return BadRequest(new { resp = "Unexpected server error" });
            }
        }

        public IActionResult MakeUserAnManager(string username)
        {
            var result = _adminService.MakeUserAnManager(username);
            if (result.Result.Succeeded == true)
            {
                return Ok(new { resp = result.User.UserName.ToString() + " was added to role Manager ",
                    id = result.User.Id });
            }
            else
            {
                return BadRequest(new { resp = "Unexpected server error" });
            }
        }

        public IActionResult GetManagerList()
        {

            var list = _adminService.GetManagerList();

            return PartialView(list);
        }

        public IActionResult GiveToManager(string managerName, int userId)
        {
            var res = _adminService.GiveToManager(managerName, userId);
            if (res != null)
            {
                return Json(new { resp = "User is already added" });
            }
            else
            {
                return Ok(new { resp = "User was added to manager" });
            }
        }
    }
}