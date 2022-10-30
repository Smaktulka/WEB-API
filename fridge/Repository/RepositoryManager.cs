using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts;
using Entities;
using Repository;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private RepositoryContext _repositoryContext;

        private IFridgesRepository _fridgesRepository;
        private IProductsRepository _productsRepository;
        private IFridgeModelRepository _fridgeModelRepository;
        private IFridgeProductsRepository _fridgeProductsRepository;
        public RepositoryManager(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }
        public IFridgesRepository Fridges
        {
            get
            {
               if (_fridgesRepository == null)
                   _fridgesRepository = new FridgesRepository(_repositoryContext);

                return _fridgesRepository; 
            }

        }

        public IProductsRepository Products
        {
            get
            {
                if (_productsRepository == null)
                    _productsRepository = new ProductsRepository(_repositoryContext);

                return _productsRepository;
            }
        }

        public IFridgeProductsRepository FridgeProducts
        {
            get
            {
                if (_fridgeProductsRepository == null)
                    _fridgeProductsRepository = new FridgeProductsRepository(_repositoryContext);

                return _fridgeProductsRepository;
            }
        }

        public IFridgeModelRepository FridgeModels
        {
            get
            {
                if (_fridgeModelRepository == null)
                    _fridgeModelRepository = new FridgeModelsRepository(_repositoryContext);

                return _fridgeModelRepository;
            }
        }
        public Task SaveAsync() => _repositoryContext.SaveChangesAsync(); 
        public void Save() => _repositoryContext.SaveChanges();

    }
}
