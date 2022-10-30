using Contracts;
using Entities;
using Entities.Models;
using fridge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class ProductsRepository : RepositoryBase<Products>, IProductsRepository
    {
        public ProductsRepository(RepositoryContext repositoryContext)
                : base(repositoryContext)
        {

        }

        public async Task<Products> GetProductAsync(Guid productId, bool trackChanges) =>
            FindByCondition(c => c.Id.Equals(productId), trackChanges)
            .SingleOrDefault();

        public async Task<IEnumerable<Products>> GetAllProductsAsync(bool trackChanges) =>
            FindAll(trackChanges)
            .OrderBy(c => c.Name)
            .ToList();
    }
}
