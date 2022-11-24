using Azure;
using la_mia_pizzeria_static.Data;
using la_mia_pizzeria_static.Models;
using la_mia_pizzeria_static.Models.Form;
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
            Pizza pizza = db.Pizzas.Where(p => p.Id == id).Include("Category").Include("Ingredients").FirstOrDefault();

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

            //associazione degli ingredienti selezionat al modello
            formData.Pizza.Ingredients = new List<Ingredient>();

            foreach (int ingredientId in formData.SelectedIngredients)
            {
                Ingredient ingredient = db.Ingredients.Where(t => t.Id == ingredientId).FirstOrDefault();
                formData.Pizza.Ingredients.Add(ingredient);
            }

            db.Pizzas.Add(formData.Pizza);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Update(int id)
        {
            Pizza pizza = db.Pizzas.Where(p => p.Id == id).Include(p => p.Ingredients).FirstOrDefault();

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

            //return View() --> non funziona perchè non ha la memoria della postItem
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


            Pizza pizzaItem = db.Pizzas.Where(post => post.Id == id).Include(p => p.Ingredients).FirstOrDefault();

            if (pizzaItem == null)
            {
                return NotFound();
            }

            pizzaItem.Name = formData.Pizza.Name;
            pizzaItem.Description = formData.Pizza.Description;
            pizzaItem.Image = formData.Pizza.Image;
            pizzaItem.CategoryId = formData.Pizza.CategoryId;

            pizzaItem.Ingredients.Clear();

            if (formData.SelectedIngredients == null)
            {
                formData.SelectedIngredients = new List<int>();
            }

            foreach (int ingredientId in formData.SelectedIngredients)
            {
                Ingredient ingredient = db.Ingredients.Where(i => i.Id == ingredientId).FirstOrDefault();
                pizzaItem.Ingredients.Add(ingredient);
            }

            //db.Pizzas.Update(formData.Pizza);
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
