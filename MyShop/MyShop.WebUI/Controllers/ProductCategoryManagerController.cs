using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.DataAccess.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShop.WebUI.Controllers
{
    public class ProductCategoryManagerController : Controller
    {
        IRepository<ProductCategory> context;

        public ProductCategoryManagerController(IRepository<ProductCategory> context)
        { 
            this.context = context;
        }
        // GET: ProductManager
        public ActionResult Index()
        {
            List<ProductCategory> productCategoriess = context.Collection().ToList();


            return View(productCategoriess);
        }

        public ActionResult Create()
        {
            ProductCategory productCategory = new ProductCategory();

            return View(productCategory);
        }

        [HttpPost]
        public ActionResult Create(ProductCategory productCategory)
        {
            if (!ModelState.IsValid)
            {
                return View(productCategory);
            }
            else
            {

                context.Insert(productCategory);
                context.Commit();

                return RedirectToAction("Index");
            }


        }



        public ActionResult Edit(String Id)
        {

            ProductCategory productCategory = context.Find(Id);

            if (productCategory == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(productCategory);
            }


        }


        [HttpPost]
        public ActionResult Edit(ProductCategory productCategory, String Id)
        {

            ProductCategory ProductCategoryToEdit = context.Find(Id);

            if (ProductCategoryToEdit == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    return View(productCategory);
                }

                ProductCategoryToEdit.Category = productCategory.Category;


                context.Commit();

                return RedirectToAction("Index");
            }

        }




        public ActionResult Delete(String Id)
        {

            ProductCategory ProductCategoryToDelete = context.Find(Id);

            if (ProductCategoryToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(ProductCategoryToDelete);
            }

        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(String Id)
        {

            ProductCategory ProductCategoryToDelete = context.Find(Id);

            if (ProductCategoryToDelete == null)
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