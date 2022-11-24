using la_mia_pizzeria_static.Validator;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace la_mia_pizzeria_static.Models
{
    public class Pizza
    {
        //properties
        public int Id { get; set; }

        [Required(ErrorMessage = "Il campo è obbligatorio")]
        [StringLength(100, ErrorMessage = "Il titolo non può essere più lungo di 100 caratteri")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Il campo è obbligatorio")]
        [StringLength(100, ErrorMessage = "La descrizione non può essere più lunga di 500 caratteri")]
        [Column(TypeName = "text")]
        [FiveWords]
        public string Description { get; set; }

        [Required(ErrorMessage = "Il campo è obbligatorio")]
        public string Image { get; set; }

        public float Price { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public List<Ingredient>? Ingredients { get; set; }

        //costruttore
        public Pizza (string name, string description, string image, float price)
        {
            Name= name;
            Description= description;
            Image= image;
            Price= price;
        }

        public Pizza ()
        {

        }
    }
}
