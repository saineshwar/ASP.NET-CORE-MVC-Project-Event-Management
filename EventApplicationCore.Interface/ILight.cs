using EventApplicationCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventApplicationCore.Interface
{
    public interface ILight
    {
        void SaveLight(Light Light);
        IQueryable<Light> ShowLight(string sortColumn, string sortColumnDir, string Search);
        int DeleteLight(int id);
        Light LightByID(int id);
        void UpdateLight(Light Light);
        bool CheckLightAlready(string LightName);
        List<LightModel> GetAllLight();
        IEnumerable<Light> ShowAllLight();
    }
}
