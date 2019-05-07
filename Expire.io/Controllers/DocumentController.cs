using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Expire.io.DTOs;
using Expire.io.Models.Data;
using Expire.io.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Expire.io.Controllers
{
    [Authorize]
    public class DocumentController : Controller
    {

        private readonly ExpireContext _context;

        public DocumentController(ExpireContext c)
        {
            _context = c;
        }

        public IActionResult Index()
        {
            var id = _context.Users.Where(u => u.UserName == User.Identity.Name).First().Id;
            return View(_context.UserDocuments.Where(ud => ud.UserId == id).Select(ud => new DocumentDTO {
                Id = ud.Id,
                Name = ud.Document.Name,
                DateOfExpiry = ud.Document.DateOfExpiry,
                TypeOfDocId = ud.Document.TypeOfDoc.Name
            }).ToList());
        }

        [HttpGet("[action]")]
        public IActionResult GetAllDocumentsByUser()
        {
            var id = _context.Users.Where(u => u.UserName == User.Identity.Name).First().Id;
            return Ok(_context.UserDocuments.Where(ud => ud.UserId == id).Select(ud=>new {
                Name = ud.Document.Name,
                DateOfExpiry = ud.Document.DateOfExpiry,
                TypeOfDocId = ud.Document.TypeOfDoc.Name }).ToList());
        }

        [HttpPost("[action]")]
        public IActionResult CreateDocument(DocumentCreationViewModel model)
        {
            return Ok();
        }

        [HttpGet("[action]")]
        public IActionResult CreateDocument()
        {
            return View();
        }
    }
}