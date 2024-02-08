using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinkSystem.Data;
using LinkSystem.Models;
using LinkSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace LinkSystem.Repository
{
    public class ProdcutRepository(MyDataContext myDataContext) : IProdcutService
    {
        private readonly MyDataContext _myDataContext = myDataContext;

        public async Task<bool> CreateProduct(Prodcut prodcut)
        {
            await _myDataContext.Prodcuts.AddAsync(prodcut);
            return await _myDataContext.SaveChangesAsync() > 0;

        }

        public async Task<bool> DeleteProdcut(Guid Id)
        {
            var prodcut = await GetProdcutById(Id);
            if (prodcut is not null)
                _myDataContext.Prodcuts.Remove(prodcut);
            return await _myDataContext.SaveChangesAsync() > 0;

        }
        public async Task<Prodcut> GetProdcutById(Guid Id)
        {
            return await _myDataContext.Prodcuts.FindAsync(Id);
        }

        public async Task<List<Prodcut>> GetProdcuts()
        {
            return await _myDataContext.Prodcuts.AsNoTracking().ToListAsync();
        }

        public async Task<bool> UpdateProdcut(Prodcut prodcut)
        {
            _myDataContext.Prodcuts.Update(prodcut);
            return await _myDataContext.SaveChangesAsync() > 0;

        }
    }
}