using EventApplicationCore.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventApplicationCore.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic;

namespace EventApplicationCore.Concrete
{
    public class LightConcrete : ILight
    {
        private DatabaseContext _context;

        public LightConcrete(DatabaseContext context)
        {
            _context = context;
        }

        public int DeleteLight(int id)
        {
            Light Light = _context.Light.Find(id);
            _context.Light.Remove(Light);
            return _context.SaveChanges();
        }

        public Light LightByID(int id)
        {
            try
            {
                return _context.Light.Find(id);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void SaveLight(Light Light)
        {
            try
            {

                _context.Light.Add(Light);
                _context.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IQueryable<Light> ShowLight(string sortColumn, string sortColumnDir, string Search)
        {
            var IQueryableLight = (from tempLight in _context.Light
                                   select new Light
                                   {
                                       LightID = tempLight.LightID,
                                       LightName = tempLight.LightName,
                                       LightCost = tempLight.LightCost,
                                       LightTypeName = (tempLight.LightType == "1" ? "In Door" : "Out Door"),
                                       Createdate = tempLight.Createdate
                                   });

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                IQueryableLight = IQueryableLight.OrderBy(sortColumn + " " + sortColumnDir);
            }
            if (!string.IsNullOrEmpty(Search))
            {
                IQueryableLight = IQueryableLight.Where(m => m.LightName == Search);
            }

            return IQueryableLight;
        }


        public void UpdateLight(Light Light)
        {
            try
            {
                if (Light.LightFilename != null)
                {
                    _context.Entry(Light).Property(x => x.LightName).IsModified = true;
                    _context.Entry(Light).Property(x => x.LightCost).IsModified = true;
                    _context.Entry(Light).Property(x => x.LightFilename).IsModified = true;
                    _context.Entry(Light).Property(x => x.LightFilePath).IsModified = true;
                    _context.Entry(Light).Property(x => x.Createdate).IsModified = true;
                    _context.SaveChanges();
                }
                else
                {
                    _context.Light.Attach(Light);
                    _context.Entry(Light).Property(x => x.LightName).IsModified = true;
                    _context.Entry(Light).Property(x => x.LightType).IsModified = true;
                    _context.Entry(Light).Property(x => x.LightCost).IsModified = true;
                    _context.Entry(Light).Property(x => x.Createdate).IsModified = true;
                    _context.SaveChanges();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool CheckLightAlready(string LightName)
        {
            var LightCount = (from light in _context.Light
                              where light.LightName == LightName
                              select light).Count();

            if (LightCount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<LightModel> GetAllLight()
        {
            var LightList = (from light in _context.Light
                              select new LightModel { LightID = light.LightID, LightName = light.LightName }).ToList();

            return LightList;
        }

        public IEnumerable<Light> ShowAllLight()
        {
            var LightList = (from light in _context.Light
                              select new Light
                              {
                                  LightID = light.LightID,
                                  LightFilename = light.LightName,
                                  LightFilePath = light.LightFilePath
                              }).ToList();

            return LightList;
        }
    }
}
