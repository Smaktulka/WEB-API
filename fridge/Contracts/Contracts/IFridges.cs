using Entities.DataTransferObjects;
using Entities.Models;
using Entities.RequestFeatures;
using fridge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IFridgesRepository
    {
        Task<IEnumerable<Fridges>> GetAllFridgesAsync(bool trackChanges);
        Task<Fridges> GetFridgeAsync(Guid fridgeId, bool trackChanges);
        void CreateFridge(Fridges fridge);
        Task<IEnumerable<Fridges>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
        void DeleteFridge(Fridges fridge);
    }
}

namespace Contracts
{
    public interface IProductsRepository
    {
        Task<IEnumerable<Products>> GetAllProductsAsync(bool trackChanges);
        Task<Products> GetProductAsync(Guid productId, bool trackChanges);
    }
}

namespace Contracts
{
    public interface IFridgeProductsRepository
    {
        Task<IEnumerable<FridgeProducts>> GetFridgeProductsAsync(Guid fridgeId, bool trackChanges);
        Task<FridgeProducts> GetFridgeProductAsync(Guid fridgeId, Guid Id, bool trackChanges);
        void CreateFridgeProductForFridge(Guid fridgeId, FridgeProducts fridgeProducts);
        Task<IEnumerable<FridgeProducts>> GetFridgeProductsByIdsAsync(IEnumerable<Guid> fridgeProductId,
            bool trackChanges);
        void DeleteFridgeProducts(FridgeProducts fridgeProducts);
    }
}

namespace Contracts
{
    public interface IFridgeModelRepository
    {
        Task<FridgeModels> GetFridgeModelByIdAsync(Guid modelId, bool trackChanges);

        Task<IEnumerable<FridgeModels>> GetAllFridgeModelsAsync(bool trackChanges);
    }
}