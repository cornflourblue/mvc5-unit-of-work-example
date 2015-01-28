using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitOfWorkExample.Domain.Entities;
using UnitOfWorkExample.Domain.Helpers;
using UnitOfWorkExample.Domain.Repositories;

namespace UnitOfWorkExample.Domain.Services
{
    public interface IProductService
    {
        IList<Product> GetAll();
        Product GetById(int id);
        void Create(Product product);
        void Update(Product product);
        void Delete(int id);
    }

    public class ProductService : IProductService
    {
        private IRepository<Product> _productRepository;

        public ProductService(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public IList<Product> GetAll()
        {
            return _productRepository
                .GetAll()
                .ToList();
        }

        public Product GetById(int id)
        {
            return _productRepository.GetById(id);
        }

        public void Create(Product product)
        {
            _productRepository.Create(product);
        }

        public void Update(Product product)
        {
            _productRepository.Update(product);
        }

        public void Delete(int id)
        {
            _productRepository.Delete(id);
        }

    }
}
