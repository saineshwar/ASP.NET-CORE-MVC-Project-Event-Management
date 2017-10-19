using EventApplicationCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventApplicationCore.Interface
{
    public interface IEquipment
    {
        void SaveEquipment(Equipment Equipment);
        IQueryable<Equipment> ShowEquipment(string sortColumn, string sortColumnDir, string Search);
        int DeleteEquipment(int id);
        Equipment GetEquipmentByID(int id);
        void UpdateEquipment(Equipment Equipment);
        bool CheckEquipmentAlready(string EquipmentName);
        List<EquipmentModel> GetAllEquipment();
        IEnumerable<Equipment> ShowAllEquipment();
    }
}
