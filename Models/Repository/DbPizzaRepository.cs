using la_mia_pizzeria_static.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.SqlServer.Server;

namespace la_mia_pizzeria_static.Models.Repository
{
    public class DbPizzaRepository : IDbPizzaRepository
    {
        private PizzeriaDbContext db;
        public DbPizzaRepository() : base()
        {
            db = new PizzeriaDbContext();
        }

        public List<Pizza> All()
        {
            return db.Pizzas.Include(pizza => pizza.Category).Include(pizza => pizza.Ingredients).ToList();
        }

        public Pizza GetById(int id)
        {
            return db.Pizzas.Where(p => p.Id == id).Include("Category").Include("Ingredients").FirstOrDefault();
        }

        public void Create(Pizza pizza, List<int> selectedIngredients)
        {
            //associazione degli ingredienti selezionat al modello
            pizza.Ingredients = new List<Ingredient>();

            foreach (int ingredientId in selectedIngredients)
            {
                Ingredient ingredient = db.Ingredients.Where(t => t.Id == ingredientId).FirstOrDefault();
                pizza.Ingredients.Add(ingredient);
            }

            db.Pizzas.Add(pizza);
            db.SaveChanges();
        }

        public void Update(Pizza pizza, Pizza formData, List<int> selectedIngredients)
        {
            if (selectedIngredients == null)
            {
                selectedIngredients = new List<int>();
            }

            pizza.Name = formData.Name;
            pizza.Description = formData.Description;
            pizza.Image = formData.Image;
            pizza.CategoryId = formData.CategoryId;

            pizza.Ingredients.Clear();


            foreach (int ingredientId in selectedIngredients)
            {
                Ingredient ingredient = db.Ingredients.Where(i => i.Id == ingredientId).FirstOrDefault();
                pizza.Ingredients.Add(ingredient);
            }

            db.SaveChanges();
        }

        //METODO ALTERNATIVO CON OVERLOAD
        //public void Update(int id, Pizza formData, List<int>? selectedIngredients)
        //{
        //    Pizza pizzaItem = GetById(id);

        //    if(pizzaItem == null)
        //    {
        //        //throw new
        //    }

        //    this.Update(pizzaItem, formData, selectedIngredients);

        //}

        public void Delete(Pizza pizza)
        {
            db.Pizzas.Remove(pizza);
            db.SaveChanges();
        }
    }
}
