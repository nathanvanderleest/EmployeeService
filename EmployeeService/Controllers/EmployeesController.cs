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
        public HttpResponseMessage Get(int id)
        {
            using (TestDBEntities entities = new TestDBEntities())
            {
                var entity = entities.employees.FirstOrDefault(e => e.ID == id);

                if(entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee not found with ID: " + id);
                }
            }
        }

        // Post api/employee
        public HttpResponseMessage Post([FromBody] employee employee)
        {
            try
            {
                using (TestDBEntities entities = new TestDBEntities())
                {
                    entities.employees.Add(employee);
                    entities.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, employee);
                    message.Headers.Location = new Uri(Request.RequestUri + employee.ID.ToString());
                    return message;
                }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        // Delete api/employee/5
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                using(TestDBEntities entities = new TestDBEntities())
                {
                    var employee = entities.employees.FirstOrDefault(e => e.ID == id);
                    if(employee != null)
                    {
                        entities.employees.Remove(employee);
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, "Employee with ID: " + id + " was deleted");
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with ID: " + id + " does not exist");
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }


    }
}
