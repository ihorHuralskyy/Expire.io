using Expire.io.DTOs;
using Expire.io.Models.Data;
using Expire.io.Models.Entities;
using Expire.io.Services.Contracts;
using Expire.io.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Expire.io.DTO_s;

namespace Expire.io.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly ExpireContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContext;

        public DocumentService(ExpireContext context, UserManager<User> userManager, IHttpContextAccessor httpContext)
        {
            _context = context;
            _userManager = userManager;
            _httpContext = httpContext;
        }

        public User CreateDocument(DocumentCreationViewModel model)
        {
            var user = _userManager.Users.Where(u => u.UserName == model.UserName).FirstOrDefault();
            if (user != null)
            {
                TypeOfDoc type = new TypeOfDoc();
                switch (model.TypeOfDocName)
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
                    Image<Rgba32> image = Image.Load(img);

                    image.Mutate(x => x.
                        Resize(200, 200));

                    ms.Seek(0, SeekOrigin.Begin);

                    image.SaveAsJpeg(ms);

                    img = ms.ToArray();

                    dc = new DocumentImage { Image = img };
                }

                _context.DocumentImages.Add(dc);

                var a = model.datetime.Split("-");
                var b = a[2].Split("T");
                var c = b[1].Split(":");
                DateTime time = new DateTime(int.Parse(a[0]), int.Parse(a[1]), int.Parse(b[0]), int.Parse(c[0]), int.Parse(c[1]), 0);
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
            }
            return user;
        }

        public List<DocumentDTO> GetAllDocumentsByUser(string userName)
        {
            var id = _context.Users.Where(u => u.UserName == userName).First().Id;

            var list = _context.UserDocuments.Where(ud => ud.UserId == id).Select(ud => new DocumentDTO
            {
                Id = ud.Id,
                Name = ud.Document.Name,
                DateOfExpiry = ud.Document.DateOfExpiry,
                TypeOfDocName = ud.Document.TypeOfDoc.Name,
                Image = Convert.ToBase64String(_context.DocumentImages.FirstOrDefault(item => item.DocumentId == ud.DocumentId).Image)
            }).ToList();

            return list;
        }

        public List<DocumentDTO> Index()
        {
            var id = _context.Users
                .Where(u => u.UserName == _httpContext.HttpContext.User.Identity.Name)
                .First()
                .Id;

            var list = _context.UserDocuments.Where(ud => ud.UserId == id).Select(ud => new DocumentDTO
            {
                Id = ud.DocumentId,
                Name = ud.Document.Name,
                DateOfExpiry = ud.Document.DateOfExpiry,
                TypeOfDocName = ud.Document.TypeOfDoc.Name,
                Image = Convert.ToBase64String(_context.DocumentImages
                    .FirstOrDefault(item => item.DocumentId == ud.DocumentId).Image)
            }).ToList();

            return list;
        }

        public List<DocumentDTO> GetExpiredDocumentsByUser(int id)
        {
            var list = _context.Documents.Where(item => item.Id == _context.UserDocuments
                         .FirstOrDefault(ud => ud.UserId == id && ud.DocumentId == item.Id).DocumentId &&
                     (item.DateOfExpiry.Value - DateTime.Now.Date).TotalDays < 0).Select(item => new DocumentDTO
                     {
                         Id =  item.Id,
                         TypeOfDocName = item.TypeOfDoc.Name,
                         Name = item.Name,
                         Image = Convert.ToBase64String(item.Image.Image),
                         DateOfExpiry = item.DateOfExpiry
                     });

            return list.ToList();
        }

        public DocumentDTO DocumentInfo(int id)
        {
            var list = _context.Documents.ToList();
            Document document = list.Find(item => item.Id == id);
            DocumentDTO documentDto = new DocumentDTO
            {
                Id = document.Id,
                Image = Convert.ToBase64String(_context.DocumentImages.FirstOrDefault(item=>item.DocumentId == document.Id).Image),
                Name = document.Name,
                TypeOfDocName = _context.TypeOfDocs.FirstOrDefault(item=>item.Id == document.TypeOfDocId).Name,
                DateOfExpiry = document.DateOfExpiry.Value
            };
            return documentDto;
        }

        public string[] UpdateDocument(UpdateDocumentDTO model)
        {
            var document = _context.Documents.FirstOrDefault(item => item.Id == model.Id);
            var a = model.date.Split("-");
            var b = a[2].Split("T");
            var c = b[1].Split(":");
            DateTime time = new DateTime(int.Parse(a[0]), int.Parse(a[1]), int.Parse(b[0]), int.Parse(c[0]), int.Parse(c[1]), 0);
            document.DateOfExpiry = time;
            DocumentImage dc;
            using (MemoryStream ms = new MemoryStream())
            {
                model.Photo.CopyTo(ms);
                byte[] img = ms.ToArray();
                Image<Rgba32> image = Image.Load(img);

                image.Mutate(x => x.
                    Resize(200, 200));

                ms.Seek(0, SeekOrigin.Begin);

                image.SaveAsJpeg(ms);

                img = ms.ToArray();

                dc = new DocumentImage { Image = img };
            }

            int idToDelete = _context.DocumentImages.FirstOrDefault(item=>item.DocumentId == document.Id).Id;
            _context.DocumentImages.Add(dc);

            _context.DocumentImages.Remove(_context.DocumentImages.FirstOrDefault(item=>item.Id==idToDelete));

            _context.DocumentImages.Add(dc);

            document.Image = dc;

            _context.Documents.Update(document);
            _context.SaveChanges();

            return new string[]{Convert.ToBase64String(document.Image.Image),time.ToString()};
        }
    }
}
