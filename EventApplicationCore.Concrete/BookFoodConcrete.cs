using EventApplicationCore.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventApplicationCore.Model;

namespace EventApplicationCore.Concrete
{
    public class BookFoodConcrete : IBookFood
    {
        private DatabaseContext _context;

        public BookFoodConcrete(DatabaseContext context)
        {
            _context = context;
        }

        public int BookFood(BookingFood Food)
        {
            _context.BookingFood.Add(Food);
            return _context.SaveChanges();
        }

        public IEnumerable<Food> FoodList(Food Food)
        {
           
            if (Food != null)
            {
                var FoodList = (from tempfood in _context.Food
                                where tempfood.FoodType == Food.FoodType && tempfood.MealType == Food.MealType && tempfood.DishType == Food.DishType
                                select tempfood).ToList();
                return FoodList;
            }
            else
            {
                return Enumerable.Empty<Food>();
            }

        }
    }
}
