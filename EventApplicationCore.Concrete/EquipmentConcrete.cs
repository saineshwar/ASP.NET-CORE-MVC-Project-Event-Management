using EventApplicationCore.Interface;
using EventApplicationCore.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;

namespace EventApplicationCore.Concrete
{
    public class EquipmentConcrete : IEquipment
    {
        private DatabaseContext _context;

        public EquipmentConcrete(DatabaseContext context)
        {
            _context = context;
        }

        public void SaveEquipment(Equipment Equipment)
        {
            try
            {
                _context.Equipment.Add(Equipment);
                _context.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IQueryable<Equipment> ShowEquipment(string sortColumn, string sortColumnDir, string Search)
        {
            var IQueryableEquipment = (from tempequipment in _context.Equipment select tempequipment);

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                IQueryableEquipment = IQueryableEquipment.OrderBy(sortColumn + " " + sortColumnDir);
            }
            if (!string.IsNullOrEmpty(Search))
            {
                IQueryableEquipment = IQueryableEquipment.Where(m => m.EquipmentName == Search);
            }

            return IQueryableEquipment;
        }

        public IEnumerable<Equipment> ShowAllEquipment()
        {
            var EquipmentList = (from equipment in _context.Equipment
                                 select new Equipment
                                 { EquipmentID = equipment.EquipmentID,
                                   EquipmentName = equipment.EquipmentName ,
                                   EquipmentFilePath = equipment.EquipmentFilePath
                                 }).ToList();

            return EquipmentList;
        }

        public List<EquipmentModel> GetAllEquipment()
        {
            var EquipmentList = (from equipment in _context.Equipment
                          select new EquipmentModel { EquipmentID = equipment.EquipmentID, EquipmentName = equipment.EquipmentName }).ToList();

            return EquipmentList;
        }

        public int DeleteEquipment(int id)
        {
            try
            {
                Equipment equipment = _context.Equipment.Find(id);
                _context.Equipment.Remove(equipment);
               return _context.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Equipment GetEquipmentByID(int id)
        {
            if (id != 0)
            {
                return _context.Equipment.Find(id);
            }
            else
            {
                return new Equipment();
            }
        }

        public void UpdateEquipment(Equipment Equipment)
        {
            try
            {
                if (Equipment.EquipmentFilename != null)
                {
                    _context.Entry(Equipment).Property(x => x.EquipmentName).IsModified = true;
                    _context.Entry(Equipment).Property(x => x.EquipmentCost).IsModified = true;
                    _context.Entry(Equipment).Property(x => x.EquipmentFilename).IsModified = true;
                    _context.Entry(Equipment).Property(x => x.EquipmentFilePath).IsModified = true;
                    _context.Entry(Equipment).Property(x => x.Createdate).IsModified = true;
                    _context.SaveChanges();
                }
                else
                {
                    _context.Equipment.Attach(Equipment);
                    _context.Entry(Equipment).Property(x => x.EquipmentName).IsModified = true;
                    _context.Entry(Equipment).Property(x => x.EquipmentCost).IsModified = true;
                    _context.Entry(Equipment).Property(x => x.Createdate).IsModified = true;
                    _context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckEquipmentAlready(string EquipmentName)
        {
            var EquipmentCount = (from equipment in _context.Equipment
                              where equipment.EquipmentName == EquipmentName
                              select equipment).Count();

            if (EquipmentCount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
