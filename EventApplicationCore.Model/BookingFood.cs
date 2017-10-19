using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventApplicationCore.Model
{
    public class BookingFood
    {
        [Key]
        public int BookFoodID { get; set; }

        [Required(ErrorMessage = "Select FoodType")]
        public string FoodType { get; set; }

        [Required(ErrorMessage = "Select MealType")]
        public string MealType { get; set; }

        [Required(ErrorMessage = "Select DishType")]
        public int? DishType { get; set; }

        public int? DishName { get; set; }

        public int? Createdby { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int BookingID { get; set; }

        [NotMapped]
        public List<FoodModel> FoodList { get; set; }
    }
}
