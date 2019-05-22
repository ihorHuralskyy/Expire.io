using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Expire.io.ViewModels;
using Expire.io.Services.Contracts;

namespace Expire.io.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        public AccountController(
            IAccountService accountService)
        {
            _accountService = accountService;
        }
        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _accountService.Register(model);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", controllerName: "Home");
                    }
                    else
                    {
                        foreach (var item in result.Errors)
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
            return View(model);
        }
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var res = await _accountService.Login(model);
                if (res.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction(controllerName: "Home", actionName: "Index");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Password is not valid");
                }
            }
            return View(model);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> LogOff()
        {
            await _accountService.LogOff();

            return RedirectToAction("Login");
        }
    }
}