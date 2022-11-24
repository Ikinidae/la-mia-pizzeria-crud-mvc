using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;

namespace la_mia_pizzeria_static.Models.Form
{
    public class FormPizzaCategory
    {
        //model del db che con le views: create, read (list, details), update
        public Pizza Pizza { get; set; }

        //views: create, update, 
        public List<Category>? Categories { get; set; }
        public List<SelectListItem>? Ingredients { get; set; }
        public List<int>? SelectedIngredients { get; set; }
    }
}
