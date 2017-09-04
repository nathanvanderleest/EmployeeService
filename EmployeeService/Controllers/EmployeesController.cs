using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EmployeeDataAccess;

namespace EmployeeService.Controllers
{
    public class EmployeesController : ApiController
    {

        // GET api/employee
        public IEnumerable<employee> Get()
        {
            using(TestDBEntities entities = new TestDBEntities())
            {
                return entities.employees.ToList();
            }
        }

        // GET api/employee/5
        public employee Get(int id)
        {
            using (TestDBEntities entities = new TestDBEntities())
            {
                return entities.employees.FirstOrDefault(e => e.ID == id);
            }
        }



    }
}
