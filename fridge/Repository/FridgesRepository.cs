using Contracts;
using Entities;
using Entities.Models;
using fridge.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class FridgesRepository : RepositoryBase<Fridges>, IFridgesRepository
    {
        public FridgesRepository(RepositoryContext repositoryContext)
                : base(repositoryContext)
        {

        }

        public async Task<IEnumerable<Fridges>> GetAllFridgesAsync(bool trackChanges) =>
             FindAll(trackChanges)
            .OrderBy(c => c.Name)
            .ToList();

        public async Task<Fridges> GetFridgeAsync(Guid fridgesId, bool trackChanges) => 
             FindByCondition(c => c.Id.Equals(fridgesId), trackChanges)
            .SingleOrDefault();

        public void CreateFridge(Fridges fridge) => Create(fridge);

        public async Task<IEnumerable<Fridges>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges) =>
           await FindByCondition(x => ids.Contains(x.Id), trackChanges)
            .ToListAsync();

        public void DeleteFridge(Fridges fridge)
        {
            Delete(fridge);
        }
    }
}
