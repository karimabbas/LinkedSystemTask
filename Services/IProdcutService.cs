using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinkSystem.Models;

namespace LinkSystem.Services
{
    public interface IProdcutService
    {
        Task<List<Prodcut>> GetProdcuts();
        Task<Prodcut> GetProdcutById(Guid Id);
        Task<bool> CreateProduct(Prodcut prodcut);
        Task<bool> UpdateProdcut(Prodcut prodcut);
        Task<bool> DeleteProdcut(Guid Id);
    }
}