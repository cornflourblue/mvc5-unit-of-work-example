using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using UnitOfWorkExample.Domain.Entities;
using UnitOfWorkExample.Domain.Services;

namespace UnitOfWorkExample.WebApi.Controllers
{
    [RoutePrefix("products")]
    public class ProductsController : ApiController
    {
        private IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [Route]
        public IHttpActionResult GetProducts()
        {
            // ensure there are products for the example
            if (!_productService.GetAll().Any())
            {
                _productService.Create(new Product { Name = "Product 1" });
                _productService.Create(new Product { Name = "Product 2" });
                _productService.Create(new Product { Name = "Product 3" });
            }

            return Ok(_productService.GetAll());
        }
    }
}
