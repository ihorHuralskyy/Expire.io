using Xunit;
using Moq;
using Expire.io.Services.Contracts;
using Expire.io.Controllers;
using System.Threading.Tasks;
using System.Collections.Generic;
using Expire.io.DTOs;
using Microsoft.AspNetCore.Mvc;
using Expire.io.ViewModels;
using Expire.io.Services.Responses;
using Microsoft.AspNetCore.Identity;
using Expire.io.Models.Entities;

namespace Expire.io.Tests.Controllers
{
    public class AdminControllerTests
    {
        private readonly AdminController adminController;
        private readonly Mock<IAdminService> adminService;
        public AdminControllerTests()
        {
            adminService = new Mock<IAdminService>();

            adminController = new AdminController(adminService.Object);
        }

        [Fact]
        public async void AllUsersTest()
        {
            var list = new List<UserDTORolesList>();
            adminService
                .Setup(a => a.AllUsers())
                .Returns(Task.FromResult(list));

            var result = (await adminController.AllUsers()) as ViewResult;

            adminService.Verify(a => a.AllUsers(), Times.Once);

            Assert.Equal(list.Count, ((result.Model) as List<UserDTORolesList>).Count);
        }

        [Fact]
        public async void CreateUserBadRequestTest()
        {
            var model = new AdminAddUserViewModel();

            adminService.Setup(a => a.CreateUser(model))
                .Returns(Task.FromResult(new ResponseForAdmin
                {
                    User = null,
                    Result = IdentityResult.Failed()
                }));

            var result = await adminController.CreateUser(model);

            Assert.IsAssignableFrom<BadRequestObjectResult>(result);
        }

        [Fact]
        public async void CreateUserJsonTest()
        {
            var model = new AdminAddUserViewModel();

            adminService.Setup(a => a.CreateUser(model))
                .Returns(Task.FromResult(new ResponseForAdmin
                {
                    User = null,
                    Result = IdentityResult.Success
                }));

            var result = await adminController.CreateUser(model);

            Assert.IsAssignableFrom<OkObjectResult>(result);
        }

        [Fact]
        public async void DeleteUserTest()
        {
            adminService.Setup(a => a.DeleteUser(""))
                .Returns(Task.FromResult(new ResponseForAdmin
                {
                    User = null,
                    Result = IdentityResult.Failed()
                }));

            var result = await adminController.DeleteUser("");

            Assert.IsAssignableFrom<BadRequestObjectResult>(result);
        }

        [Fact]
        public async void DeleteUserJsonTest()
        {
            adminService.Setup(a => a.DeleteUser(""))
                .Returns(Task.FromResult(new ResponseForAdmin
                {
                    User = new User {
                        Id = 1
                    },
                    Result = IdentityResult.Success
                }));

            var res = await adminController.DeleteUser("");

            Assert.IsAssignableFrom<OkObjectResult>(res);
        }

        [Fact]
        public void MakeUserAnAdminBadRequestTest()
        {
            adminService.Setup(a => a.MakeUserAnAdmin(""))
                .Returns(new ResponseForAdmin
                {
                    User = null,
                    Result = IdentityResult.Failed()
                });

            var result = adminController.MakeUserAnAdmin("");

            Assert.IsAssignableFrom<BadRequestObjectResult>(result);
        }

        [Fact]
        public void MakeUserAnAdminTest()
        {
            adminService.Setup(a => a.MakeUserAnAdmin(""))
                .Returns(new ResponseForAdmin
                {
                    User = new User
                    {
                        UserName = ""
                    },
                    Result = IdentityResult.Success
                });

            var result = adminController.MakeUserAnAdmin("");

            Assert.IsAssignableFrom<OkObjectResult>(result);
        }

        [Fact]
        public void MakeUserAnManagerTest()
        {
            adminService.Setup(a => a.MakeUserAnManager(""))
                .Returns(new ResponseForAdmin
                {
                    User = new User
                    {
                        UserName = ""
                    },
                    Result = IdentityResult.Success
                });

            var result = adminController.MakeUserAnManager("");

            Assert.IsAssignableFrom<OkObjectResult>(result);
        }

        [Fact]
        public void MakeUserAnManagerBadRequestTest()
        {
            adminService.Setup(a => a.MakeUserAnManager(""))
                .Returns(new ResponseForAdmin
                {
                    Result = IdentityResult.Failed()
                });

            var result = adminController.MakeUserAnManager("");

            Assert.IsAssignableFrom<BadRequestObjectResult>(result);
        }

        [Fact]
        public void GetManagerListTest()
        {
            var col = new List<string>();

            adminService.Setup(a => a.GetManagerList())
                .Returns(col);

            var result = adminController.GetManagerList() as PartialViewResult;

            var model = result.Model as List<string>;

            Assert.Equal(col.Count, model.Count);
        }

        [Fact]
        public void GiveToManagerJsonTest()
        {
            adminService.Setup(a => a.GiveToManager("", 1))
                .Returns(null as ManagerUser);

            var result = adminController.GiveToManager("", 1);

            Assert.IsAssignableFrom<OkObjectResult>(result);
        }

        [Fact]
        public void GiveToManagerOkTest()
        {
            adminService.Setup(a => a.GiveToManager("", 1))
                .Returns(new ManagerUser());

            var result = adminController.GiveToManager("", 1);

            Assert.IsAssignableFrom<JsonResult>(result);
        }
    }
}
