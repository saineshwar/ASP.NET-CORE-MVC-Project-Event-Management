using EventApplicationCore.Interface;
using EventApplicationCore.Model;
using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Collections.Generic;

namespace EventApplicationCore.Concrete
{
    public class FlowerConcrete : IFlower
    {
        private DatabaseContext _context;

        public FlowerConcrete(DatabaseContext context)
        {
            _context = context;
        }

        public void SaveFlower(Flower Flower)
        {
            try
            {
                _context.Flower.Add(Flower);
                _context.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IQueryable<Flower> ShowFlower(string sortColumn, string sortColumnDir, string Search)
        {
            var IQueryableFlower = (from tempflower in _context.Flower select tempflower);

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                IQueryableFlower = IQueryableFlower.OrderBy(sortColumn + " " + sortColumnDir);
            }
            if (!string.IsNullOrEmpty(Search))
            {
                IQueryableFlower = IQueryableFlower.Where(m => m.FlowerName == Search);
            }

            return IQueryableFlower;
        }



        public int DeleteFlower(int id)
        {
            try
            {
                Flower flower = _context.Flower.Find(id);
                _context.Flower.Remove(flower);
                return _context.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Flower GetFlowerByID(int id)
        {
            if (id != 0)
            {
                return _context.Flower.Find(id);
            }
            else
            {
                return new Flower();
            }
        }

        public void UpdateFlower(Flower Flower)
        {
            try
            {
                if (Flower.FlowerFilename != null)
                {
                    _context.Entry(Flower).Property(x => x.FlowerName).IsModified = true;
                    _context.Entry(Flower).Property(x => x.FlowerCost).IsModified = true;
                    _context.Entry(Flower).Property(x => x.FlowerFilename).IsModified = true;
                    _context.Entry(Flower).Property(x => x.FlowerFilePath).IsModified = true;
                    _context.Entry(Flower).Property(x => x.Createdate).IsModified = true;
                    _context.SaveChanges();
                }
                else
                {
                    _context.Flower.Attach(Flower);
                    _context.Entry(Flower).Property(x => x.FlowerName).IsModified = true;
                    _context.Entry(Flower).Property(x => x.FlowerCost).IsModified = true;
                    _context.Entry(Flower).Property(x => x.Createdate).IsModified = true;
                    _context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckFlowerAlready(string FlowerName)
        {
            var FlowerCount = (from flower in _context.Flower
                                  where flower.FlowerName == FlowerName
                                  select flower).Count();

            if (FlowerCount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<FlowerModel> GetAllFlower()
        {
            var FlowerList = (from flower in _context.Flower
                            select new FlowerModel { FlowerID = flower.FlowerID, FlowerName = flower.FlowerName }).ToList();

            return FlowerList;
        }

        public IEnumerable<Flower> ShowAllFlower()
        {

            var FlowerList = (from flower in _context.Flower
                            select new Flower
                            {
                                FlowerID = flower.FlowerID,
                                FlowerFilename = flower.FlowerName,
                                FlowerFilePath = flower.FlowerFilePath
                            }).ToList();

            return FlowerList;
        }
    }
}
