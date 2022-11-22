using la_mia_pizzeria_static.Data;
using la_mia_pizzeria_static.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace la_mia_pizzeria_static.Controllers
{
    public class PizzaController : Controller
    {
        PizzeriaDbContext db;

        public PizzaController() : base()
        {
            db = new PizzeriaDbContext();
        }

        public IActionResult Index()
        {
            List<Pizza> listPizzas = db.Pizzas.ToList();

            return View(listPizzas);
        }

        public IActionResult Details(int id)
        {
            Pizza pizza = db.Pizzas.Where(p => p.Id == id).FirstOrDefault();

            return View(pizza);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Pizza pizza)
        {
            if (!ModelState.IsValid)
            {
                //return View(post);
                return View();
            }

            db.Pizzas.Add(pizza);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        //public IActionResult Update(int id)
        //{
        //    Post post = db.Posts.Where(post => post.Id == id).FirstOrDefault();

        //    if (post == null)
        //        return NotFound();

        //    //return View() --> non funziona perchè non ha la memoria della post
        //    return View(post);
        //}
    }
}
