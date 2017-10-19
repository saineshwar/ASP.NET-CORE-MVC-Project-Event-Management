using EventApplicationCore.Interface;
using EventApplicationCore.Model;
using System;
using System.Linq;

namespace EventApplicationCore.Concrete
{
    public class LoginConcrete : ILogin
    {
        private DatabaseContext _context;

        public LoginConcrete(DatabaseContext context)
        {
            _context = context;
        }

        public Registration ValidateUser(string userName, string passWord)
        {
            try
            {
                var validate = (from user in _context.Registration
                                where user.Username == userName && user.Password == passWord
                                select user).SingleOrDefault();

                return validate;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool UpdatePassword(Registration Registration)
        {
            _context.Registration.Attach(Registration);
            _context.Entry(Registration).Property(x => x.Password).IsModified = true;
            int result = _context.SaveChanges();

            if(result > 0)
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
