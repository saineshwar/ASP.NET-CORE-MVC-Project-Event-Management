using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EventApplicationCore.Model;
using EventApplicationCore.Interface;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EventApplicationCore.Controllers
{
    [Route("api/[controller]")]
    public class StateAPIController : Controller
    {
        private IState _IState;

        public StateAPIController(IState IState)
        {
            _IState = IState;
        }

        // POST api/values
        [HttpPost]
        public List<States> Post(string id)
        {
            try
            {
                if (id == null)
                {
                    return null;
                }

                var listofState = _IState.ListofState(Convert.ToInt32(id));
                return listofState;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
