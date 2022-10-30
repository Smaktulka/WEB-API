using Contracts;
using Entities.Models;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
using Entities.DataTransferObjects;

namespace Repository
{
    public class FridgeProductsRepository : RepositoryBase<FridgeProducts>, IFridgeProductsRepository
    {
        public FridgeProductsRepository(RepositoryContext repositoryContext)
                : base(repositoryContext)
        {

        }

        public async Task<IEnumerable<FridgeProducts>> GetFridgeProductsAsync(Guid fridgeId,
            bool trackChanges) =>
            await FindByCondition(e => e.FridgeId.Equals(fridgeId), trackChanges)
            .ToListAsync();

        public async Task<FridgeProducts> GetFridgeProductAsync(Guid fridgeId, Guid id, bool trackChanges) =>
            FindByCondition(p => p.FridgeId.Equals(fridgeId) && p.Id.Equals(id), trackChanges)
            .SingleOrDefault();

        public async Task<IEnumerable<FridgeProducts>> GetFridgeProductsByIdsAsync(
            IEnumerable<Guid> fridgeProductIds, bool trackChanges) =>
            await FindByCondition(e => fridgeProductIds.Contains(e.Id), trackChanges)
            .ToListAsync();

        public void CreateFridgeProductForFridge(Guid fridgeId, FridgeProducts fridgeProducts)
        {
            fridgeProducts.FridgeId = fridgeId;
            Create(fridgeProducts);
        }

        public void DeleteFridgeProducts(FridgeProducts fridgeProducts)
        {
            Delete(fridgeProducts);
        }
    }

}
