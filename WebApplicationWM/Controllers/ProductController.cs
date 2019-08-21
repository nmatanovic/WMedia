using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplicationWM.Controllers
{
    public class ProductController : Controller
    {
        private WMDAL.IWMReprository<WMDAL.Product> m_ProductReprository = WMDAL.Factory.Get("DB");

        // GET: Product
        public ActionResult Index()
        {
            var products = m_ProductReprository.GetAll();

            return View(products);
        }

        // GET: Product/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}


        //
        // GET: Product/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View(new WMDAL.Product());
        }

        // POST: Product/Create
        [HttpPost]
        //public ActionResult Create(FormCollection collection)
        public ActionResult Create(WMDAL.Product product)
        {

            m_ProductReprository.Save(product);

            return RedirectToAction("Index");

        }

        // GET: Product/Edit/5
        public ActionResult Edit(int id)
        {
            WMDAL.Product p = null;

            p = m_ProductReprository.FindById(id);

            if (p != null)
            {
                return View(p);
            }
            else
            {
                return RedirectToAction("Index");
            }

        }

        // On Save
        // POST: Product/Edit/5
        [HttpPost]
        //public ActionResult Edit(int id, FormCollection collection)
        public ActionResult Edit(WMDAL.Product p)
        {
            m_ProductReprository.Update(p);

            return RedirectToAction("Index");
        }

        // GET: Product/Delete/5
        public ActionResult Delete(int id)
        {
            m_ProductReprository.Delete(id);

            return RedirectToAction("Index");
        }

        // POST: Product/Delete/5
        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
