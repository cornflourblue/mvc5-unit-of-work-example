using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UnitOfWorkExample.Domain.Entities;
using UnitOfWorkExample.Domain.Services;
using UnitOfWorkExample.Web.Helpers;
using UnitOfWorkExample.Web.Models.Home;

namespace UnitOfWorkExample.Web.Controllers
{
    public class HomeController : BaseController
    {
        private IProductService _productService;

        public HomeController(IProductService productService)
        {
            _productService = productService;
        }

        public ActionResult Index()
        {
            // ensure there are products for the example
            if (!_productService.GetAll().Any())
            {
                _productService.Create(new Product { Name = "Product 1" });
                _productService.Create(new Product { Name = "Product 2" });
                _productService.Create(new Product { Name = "Product 3" });
            }

            var viewModel = new IndexViewModel();
            viewModel.Products = _productService.GetAll();

            return View(viewModel);
        }
    }
}
