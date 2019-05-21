using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Expire.io.DTOs;
using Expire.io.Models.Data;
using Expire.io.Models.Entities;
using Expire.io.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using SixLabors.ImageSharp;

namespace Expire.io.Controllers
{
    [Authorize]
    public class DocumentController : Controller
    {

        private readonly ExpireContext _context;
        private readonly UserManager<User> _userManager;
        public DocumentController(ExpireContext c, UserManager<User> userManager)
        {
            _context = c;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var id = _context.Users.Where(u => u.UserName == User.Identity.Name).First().Id;
            var list = _context.UserDocuments.Where(ud => ud.UserId == id).Select(ud => new DocumentDTO
            {
                Id = ud.Id,
                Name = ud.Document.Name,
                DateOfExpiry = ud.Document.DateOfExpiry,
                TypeOfDocId = ud.Document.TypeOfDoc.Name,
                Image = Convert.ToBase64String(_context.DocumentImages
                    .FirstOrDefault(item => item.DocumentId == ud.DocumentId).Image)
            }).ToList();

            return View(list);
        }

        [HttpGet]
        public IActionResult GetAllDocumentsByUser(string userName)
        {
            var id = _context.Users.Where(u => u.UserName == userName).First().Id;
            ViewBag.UserName = userName;
            return View(_context.UserDocuments.Where(ud => ud.UserId == id).Select(ud => new DocumentDTO
            {
                Id = ud.Id,
                Name = ud.Document.Name,
                DateOfExpiry = ud.Document.DateOfExpiry,
                TypeOfDocId = ud.Document.TypeOfDoc.Name,
                Image = Convert.ToBase64String(_context.DocumentImages.FirstOrDefault(item=>item.DocumentId == ud.DocumentId).Image)
            }).ToList());
        }

        [HttpPost]
        public IActionResult CreateDocument(DocumentCreationViewModel model)
        {
            var user = _userManager.Users.Where(u => u.UserName == model.UserName).FirstOrDefault();
            if (user == null)
            {
                if (model == null)
                {
                    Response.StatusCode = 500;
                    return Json(new {mess = $"Username does not exist"});
                }
                else
                {
                    return Json(new {data = $"Username {model.UserName.ToString()} does not exist", color = "red"});
                }
            }
            else
            {
                TypeOfDoc type = new TypeOfDoc();
                switch (model.TypeOfDocId)
                {
                    case "License":
                        type = _context.TypeOfDocs.Where(t => t.Name == "license").First();
                        break;
                    case "Sertificate":
                        type = _context.TypeOfDocs.Where(t => t.Name == "sertificate").First();
                        break;
                    case "Insurance":
                        type = _context.TypeOfDocs.Where(t => t.Name == "insurance").First();
                        break;
                }

                DocumentImage dc;
                using (MemoryStream ms = new MemoryStream())
                {
                    model.Photo.CopyTo(ms);
                    byte[] img = ms.ToArray();
                    dc = new DocumentImage {Image = img};
                }

                _context.DocumentImages.Add(dc);

                var a = model.datetime.Split("-");
                var b = a[2].Split("T");
                var c = b[1].Split(":");
                DateTime time = new DateTime(int.Parse(a[0]),int.Parse(a[1]), int.Parse(b[0]),int.Parse(c[0]),int.Parse(c[1]),0);
                Document docToCreate = new Document
                {
                    Name = model.Name,
                    DateOfExpiry = time,
                    TypeOfDoc = type,
                    Image = dc
                };
                dc.Document = docToCreate;
                UserDocument ud = new UserDocument
                {
                    User = user,
                    Document = docToCreate
                };
                _context.Documents.Add(docToCreate);
                _context.UserDocuments.Add(ud);
                _context.SaveChanges();
                return Json(new { data = $"{model.TypeOfDocId} for {model.UserName} was created successfuly", color = "#353A40" });
            }
        }

        [HttpGet]
        public IActionResult CreateDocument()
        {
            return PartialView();
        }
    }
}