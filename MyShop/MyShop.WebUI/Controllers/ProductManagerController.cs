using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using MyShop.DataAccess.InMemory;

namespace MyShop.WebUI.Controllers
{
    public class ProductManagerController : Controller
    {

        InMemoryRepository<Product> context;
        InMemoryRepository<ProductCategory> productCategories;

        public ProductManagerController()
        {
            context = new InMemoryRepository<Product>();
            productCategories = new InMemoryRepository<ProductCategory>();
        }
        // GET: ProductManager
        public ActionResult Index()
        {
            List<Product> products = context.Collection().ToList();


            return View(products);
        }

        public ActionResult Create()
        {
            ProductManagerViewModel viewModel = new ProductManagerViewModel();
            viewModel.Product = new Product();
            viewModel.ProductCategories = productCategories.Collection();

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }
            else
            {

                context.Insert(product);
                context.Commit();

                return RedirectToAction("Index");
            }


        }



        public ActionResult Edit(String Id)
        {

            Product product = context.Find(Id);

            if (product == null)
            {
                return HttpNotFound();
            }
            else
            {
                ProductManagerViewModel viewModel = new ProductManagerViewModel();
                viewModel.Product = new Product();
                viewModel.ProductCategories = productCategories.Collection();
                return View(viewModel);
            }


        }


        [HttpPost]
        public ActionResult Edit(Product product, String Id)
        {

            Product ProductToEdit = context.Find(Id);

            if (ProductToEdit == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    return View(product);
                }

                ProductToEdit.Category = product.Category;
                ProductToEdit.Description = product.Description;
                ProductToEdit.Image = product.Image;
                ProductToEdit.Name = product.Name;
                ProductToEdit.Price = product.Price;

                context.Commit();

                return RedirectToAction("Index");
            }

        }




        public ActionResult Delete(String Id)
        {

            Product ProductToDelete = context.Find(Id);

            if (ProductToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(ProductToDelete);
            }

        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(String Id)
        {

            Product ProductToDelete = context.Find(Id);

            if (ProductToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                context.Delete(Id);
                context.Commit();
                return RedirectToAction("Index");
            }

        }










    }
}