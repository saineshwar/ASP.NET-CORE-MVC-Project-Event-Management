using EventApplicationCore.Interface;
using EventApplicationCore.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;

namespace EventApplicationCore.Concrete
{
    public class FoodConcrete : IFood
    {
        private DatabaseContext _context;

        public FoodConcrete(DatabaseContext context)
        {
            _context = context;
        }

        public void SaveFood(Food Food)
        {
            try
            {
                _context.Food.Add(Food);
                _context.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IQueryable<Food> ShowFood(string sortColumn, string sortColumnDir, string Search)
        {
            var IQueryableFood = (from tempfood in _context.Food
                                  join dishtypes in _context.Dishtypes on tempfood.DishType equals dishtypes.ID
                                  select new Food
                                  {
                                      FoodID = tempfood.FoodID,
                                      FoodName = tempfood.FoodName,
                                      FoodCost = tempfood.FoodCost,
                                      FoodTypeName = (tempfood.FoodType == "1" ? "Veg" : "Non-Veg"),
                                      MealTypeName = (tempfood.MealType == "1" ? "Lunch" : "Dinner"),
                                      DishTypeName = dishtypes.Dishtype,
                                      Createdate = tempfood.Createdate
                                  });

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                IQueryableFood = IQueryableFood.OrderBy(sortColumn + " " + sortColumnDir);
            }
            if (!string.IsNullOrEmpty(Search))
            {
                IQueryableFood = IQueryableFood.Where(m => m.FoodName == Search.Trim());
            }

            return IQueryableFood;
        }

        public int DeleteFood(int id)
        {
            try
            {
                Food Food = _context.Food.Find(id);
                _context.Food.Remove(Food);
                return _context.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Food FoodByID(int id)
        {
            try
            {
                return _context.Food.Find(id);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void UpdateFood(Food Food)
        {

            try
            {
                if (Food.FoodFilename != null)
                {
                    _context.Entry(Food).State = EntityState.Modified;
                    _context.SaveChanges();
                }
                else
                {
                    _context.Food.Attach(Food);
                    _context.Entry(Food).Property(x => x.FoodName).IsModified = true;
                    _context.Entry(Food).Property(x => x.FoodType).IsModified = true;
                    _context.Entry(Food).Property(x => x.DishType).IsModified = true;
                    _context.Entry(Food).Property(x => x.FoodCost).IsModified = true;
                    _context.Entry(Food).Property(x => x.MealType).IsModified = true;
                    _context.SaveChanges();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool CheckDishNameAlready(string DishName, string FoodType)
        {
            var dishCount = (from food in _context.Food
                             where food.FoodName == DishName && food.FoodType == FoodType
                             select food).Count();

            if (dishCount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<FoodModel> GetAllFood()
        {
            var FoodList = (from food in _context.Food
                            select new FoodModel { FoodID = food.FoodID, FoodName = food.FoodName }).ToList();

            return FoodList;
        }

        public IEnumerable<Food> ShowAllFood()
        {
            var FoodList = (from food in _context.Food
                            select new Food
                            {
                                FoodID = food.FoodID,
                                FoodFilename = food.FoodFilename,
                                FoodFilePath = food.FoodFilePath
                            }).ToList();

            return FoodList;
        }
    }
}
