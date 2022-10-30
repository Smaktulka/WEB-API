using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IRepositoryManager
    {
        IFridgesRepository Fridges { get; }
        IProductsRepository Products { get; }
        IFridgeModelRepository FridgeModels { get; }
        IFridgeProductsRepository FridgeProducts { get; }
        Task SaveAsync();
        void Save();
    }
}
