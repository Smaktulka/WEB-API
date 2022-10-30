
using Entities;
using Entities.Models;
using fridge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Repository
{
    public class FridgeModelsRepository : RepositoryBase<FridgeModels>, IFridgeModelRepository
    {
        public FridgeModelsRepository(RepositoryContext repositoryContext)
                : base(repositoryContext)
        {

        }

        public async Task<FridgeModels> GetFridgeModelByIdAsync(Guid modelId, bool trackChanges) =>
             FindByCondition(c => c.Id.Equals(modelId), trackChanges)
            .SingleOrDefault();

        public async Task<IEnumerable<FridgeModels>> GetAllFridgeModelsAsync(bool trackChanges) =>
            FindAll(trackChanges)
            .OrderBy(c => c.Name)
            .ToList();
    }
}
