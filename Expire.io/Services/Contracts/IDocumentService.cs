using Expire.io.DTOs;
using Expire.io.Models.Entities;
using Expire.io.ViewModels;
using System.Collections.Generic;

namespace Expire.io.Services.Contracts
{
    public interface IDocumentService
    {
        List<DocumentDTO> Index();
        List<DocumentDTO> GetAllDocumentsByUser(string userName);
        User CreateDocument(DocumentCreationViewModel model);
    }
}
