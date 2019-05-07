using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Expire.io.DTOs;
using Expire.io.Models.Data;
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
            return View();
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
        public IActionResult CreateDocument()
        {

        }
    }
}