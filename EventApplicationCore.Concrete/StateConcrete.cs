using EventApplicationCore.Interface;
using EventApplicationCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventApplicationCore.Concrete
{
    public class StateConcrete : IState
    {
        private DatabaseContext _context;

        public StateConcrete(DatabaseContext context)
        {
            _context = context;
        }

        public List<States> ListofState(int? ID)
        {
            if (ID == null)
            {
                return new List<States>();
            }

            var listofState = (from states in _context.States
                               where states.CountryID == ID
                               select states).ToList();

            listofState.Insert(0, new States { StateID = 0, StateName = "----Select----" });

            return listofState;
        }
    }
}
