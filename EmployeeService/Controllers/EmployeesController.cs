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
        [HttpGet]
        public HttpResponseMessage LoadAllEmployees(string gender = "all")
        {
            try
            {
                using (TestDBEntities entities = new TestDBEntities())
                {
                    switch (gender.ToLower())
                    {
                        case "all":
                            return Request.CreateResponse(HttpStatusCode.OK, entities.employees.ToList());
                        case "female":
                            return Request.CreateResponse(HttpStatusCode.OK, entities.employees.Where(e => e.Gender.ToLower() == "female").ToList());
                        case "male":
                            return Request.CreateResponse(HttpStatusCode.OK, entities.employees.Where(e => e.Gender.ToLower() == "male").ToList());
                        default:
                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, 
                                "Value for gender must all, female, or male. '" + gender.ToLower() + "' is invaild.");
                    }
                }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        // GET api/employee/5
        [HttpGet]
        public HttpResponseMessage LoadEmployeeById(int id)
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
        [HttpPost]
        public HttpResponseMessage InsertEmployee([FromBody] employee employee)
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
        [HttpDelete]
        public HttpResponseMessage RemoveEmployee(int id)
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

        // PUT api/employee/5
        [HttpPut]
        public HttpResponseMessage UpdateEmployee([FromUri]int id, [FromBody] employee employee)
        {
            try
            {
                using (TestDBEntities entities = new TestDBEntities())
                {
                    var entity = entities.employees.FirstOrDefault(e => e.ID == id);

                    if(entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with ID: " + id + " does not exist");
                    }
                    else
                    {
                        entity.Name = employee.Name;
                        entity.Department = employee.Department;
                        entity.Salary = employee.Salary;
                        entity.Gender = employee.Gender;

                        entities.SaveChanges();

                        return Request.CreateResponse(HttpStatusCode.OK, entity);
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
