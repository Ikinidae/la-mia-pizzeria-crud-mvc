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
                return View();

                //if (ModelState["Price"].Errors.Count > 0)
                //{
                //    ModelState["Price"].Errors.Clear();
                //    ModelState["Price"].Errors.Add("Il prezzo deve essere compreso tra 1 e 30");
                //}

            }

            db.Pizzas.Add(pizza);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Update(int id)
        {
            Pizza pizza = db.Pizzas.Where(p => p.Id == id).FirstOrDefault();

            if (pizza == null)
                return NotFound();

            return View(pizza);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Pizza pizza)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }

            db.Pizzas.Update(pizza);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            Pizza pizza = db.Pizzas.Where(p => p.Id == id).FirstOrDefault();

            if (pizza == null)
            {
                return NotFound();
            }

            db.Pizzas.Remove(pizza);
            db.SaveChanges();


            return RedirectToAction("Index");
        }
    }
}
