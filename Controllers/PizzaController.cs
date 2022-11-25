using Azure;
using la_mia_pizzeria_static.Data;
using la_mia_pizzeria_static.Models;
using la_mia_pizzeria_static.Models.Form;
using la_mia_pizzeria_static.Models.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace la_mia_pizzeria_static.Controllers
{
    public class PizzaController : Controller
    {
        PizzeriaDbContext db;
        IDbPizzaRepository pizzaRepository;

        public PizzaController(IDbPizzaRepository _pizzaRepository) : base()
        {
            db = new PizzeriaDbContext();
            pizzaRepository = _pizzaRepository;
        }


        public IActionResult Index()
        {
            List<Pizza> listPizzas = pizzaRepository.All();

            return View(listPizzas);
        }

        public IActionResult Details(int id)
        {
            Pizza pizza = pizzaRepository.GetById(id);

            return View(pizza);
        }

        public IActionResult Create()
        {
            FormPizzaCategory formData = new FormPizzaCategory();

            formData.Pizza = new Pizza();
            formData.Categories = db.Categories.ToList();

            formData.Ingredients = new List<SelectListItem>();
            List<Ingredient> ingredientsList = db.Ingredients.ToList();

            foreach (Ingredient ingredient in ingredientsList)
            {
                formData.Ingredients.Add(new SelectListItem(ingredient.Name, ingredient.Id.ToString()));
            }

            return View(formData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(FormPizzaCategory formData)
        {
            if (!ModelState.IsValid)
            {
                formData.Categories = db.Categories.ToList();

                formData.Ingredients = new List<SelectListItem>();
                List<Ingredient> ingredientList = db.Ingredients.ToList();

                foreach (Ingredient ingredient in ingredientList)
                {
                    formData.Ingredients.Add(new SelectListItem(ingredient.Name, ingredient.Id.ToString()));
                }
                
                return View(formData);

                //if (ModelState["Price"].Errors.Count > 0)
                //{
                //    ModelState["Price"].Errors.Clear();
                //    ModelState["Price"].Errors.Add("Il prezzo deve essere compreso tra 1 e 30");
                //}
            }
                        
            //db.Pizzas.Add(formData.Pizza);
            //db.SaveChanges();
            pizzaRepository.Create(formData.Pizza, formData.SelectedIngredients);

            return RedirectToAction("Index");
        }

        public IActionResult Update(int id)
        {
            Pizza pizza = pizzaRepository.GetById(id);

            if (pizza == null)
                return NotFound();

            FormPizzaCategory formData = new FormPizzaCategory();

            formData.Pizza = pizza;
            formData.Categories = db.Categories.ToList();

            formData.Ingredients = new List<SelectListItem>();

            List<Ingredient> ingredientsList = db.Ingredients.ToList();

            foreach (Ingredient ingredient in ingredientsList)
            {
                formData.Ingredients.Add(new SelectListItem(
                    ingredient.Name,
                    ingredient.Id.ToString(),
                    pizza.Ingredients.Any(i => i.Id == ingredient.Id)
                ));
            }

            return View(formData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, FormPizzaCategory formData)
        {

            if (!ModelState.IsValid)
            {
                formData.Pizza.Id = id;
                formData.Categories = db.Categories.ToList();

                formData.Ingredients = new List<SelectListItem>();
                List<Ingredient> ingredientsList = db.Ingredients.ToList();

                foreach (Ingredient ingredient in ingredientsList)
                {
                    formData.Ingredients.Add(new SelectListItem(ingredient.Name, ingredient.Id.ToString()));
                }

                return View(formData);
            }


            Pizza pizzaItem = pizzaRepository.GetById(id);

            if (pizzaItem == null)
            {
                return NotFound();
            }

            pizzaRepository.Update(pizzaItem, formData.Pizza, formData.SelectedIngredients);

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            Pizza pizza = pizzaRepository.GetById(id);

            if (pizza == null)
            {
                return NotFound();
            }

            pizzaRepository.Delete(pizza);

            return RedirectToAction("Index");
        }
    }
}
