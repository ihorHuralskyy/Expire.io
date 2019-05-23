using Expire.io.DTO_s;
using Expire.io.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Expire.io.Services.Contracts;

namespace Expire.io.Controllers
{
    [Authorize]
    public class DocumentController : Controller
    {
        private readonly IDocumentService _documentService;
        public DocumentController(IDocumentService documentService)
        {
            _documentService = documentService;
        }

        public IActionResult Index()
        {
            var list = _documentService.Index();

            return View(list);
        }

        [HttpGet]
        public IActionResult GetAllDocumentsByUser(string userName)
        {
            var list = _documentService.GetAllDocumentsByUser(userName);

            ViewBag.UserName = userName;

            return View(list);
        }

        public IActionResult GetExpiredDocumentsByUser(int id)
        {
            return View(_documentService.GetExpiredDocumentsByUser(id));
        }

        [HttpPost]
        public IActionResult CreateDocument(DocumentCreationViewModel model)
        {
            var user = _documentService.CreateDocument(model);

            if (user == null)
            {
                if (model == null)
                {
                    Response.StatusCode = 500;
                    return Json(new
                    { mess = $"Username does not exist"});
                }
                else
                {
                    return Json(new
                    { data = $"Username {model.UserName.ToString()} does not exist", color = "red"});
                }
            }
            else
            {
                return Json(new
                { data = $"{model.TypeOfDocName} for {model.UserName} was created successfuly", color = "#353A40" });
            }
        }

        [HttpGet]
        public IActionResult CreateDocument()
        {
            return PartialView();
        }

        public IActionResult DocumentInfo(int id)
        {
            return PartialView(_documentService.DocumentInfo(id));
        }

        public IActionResult UpdateDocument(UpdateDocumentDTO documentDto)
        {
            var result = _documentService.UpdateDocument(documentDto);
            return Ok(Json(new {photo = result[0], date = result[1]}));
        }
    }
}