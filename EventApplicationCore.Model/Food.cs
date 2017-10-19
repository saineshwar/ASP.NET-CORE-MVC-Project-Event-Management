using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventApplicationCore.Model
{
    public class Food
    {
        [Key]
        public int FoodID { get; set; }

        [Required(ErrorMessage = "FoodType Required")]
        [Display(Name = "Food Type")]
        public string FoodType { get; set; }

        [Required(ErrorMessage = "FoodName Required")]
        [Display(Name = "Food Name")]
        public string FoodName { get; set; }

        public string FoodFilename { get; set; }
        public string FoodFilePath { get; set; }
        public int? Createdby { get; set; }
        public DateTime? Createdate { get; set; }

        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Enter only numeric number")]
        [Required(ErrorMessage = "FoodCost Required")]
        public int? FoodCost { get; set; }

        [Display(Name = "Meal Type")]
        [Required(ErrorMessage = "MealType Required")]
        public string MealType { get; set; }

        [Display(Name = "Dish Type")]
        [Required(ErrorMessage = "DishType Required")]
        public int? DishType { get; set; }

        [NotMapped]
        public string DishTypeName { get; set; }

        [NotMapped]
        public string FoodTypeName { get; set; }

        [NotMapped]
        public string MealTypeName { get; set; }
    }
}
