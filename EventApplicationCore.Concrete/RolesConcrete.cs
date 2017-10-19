using EventApplicationCore.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventApplicationCore.Concrete
{
    public class RolesConcrete : IRoles
    {
        private DatabaseContext _context;

        public RolesConcrete(DatabaseContext context)
        {
            _context = context;
        }


        /// <summary>
        /// Get RoleID Name by RoleName
        /// </summary>
        /// <param name="Rolename"></param>
        /// <returns></returns>
        public int getRolesofUserbyRolename(string Rolename)
        {
            var roleID = (from role in _context.Roles
                          where role.Rolename == Rolename
                          select role.RoleID).SingleOrDefault();

            return roleID;
        }
    }
}
