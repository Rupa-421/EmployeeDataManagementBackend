using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Web.Http;
using EmployeeDataManagement.Models;
using System.Web.Http.Cors;

namespace EmployeeDataManagement.Controllers
{

    public class EmployeeController : ApiController
    {
        [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
        public HttpResponseMessage Get()
        {
            using (EmployeeDBContext dbContext = new EmployeeDBContext())
            {
                var Employees = dbContext.employees.ToList();
                return Request.CreateResponse(HttpStatusCode.OK, Employees);

            }
        }
        public HttpResponseMessage Get(int id)
        {
            using (EmployeeDBContext dbContext = new EmployeeDBContext())
            {
                var entity = dbContext.employees.FirstOrDefault(e => e.id == id);
                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                        "Employee with ID " + id.ToString() + "not found");
                }
            }
        }
        public HttpResponseMessage Post([FromBody] employee employee)
        {
            try
            {
                using (EmployeeDBContext dbContext = new EmployeeDBContext())
                {
                    dbContext.employees.Add(employee);
                    dbContext.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, employee);
                    message.Headers.Location = new Uri(Request.RequestUri +
                        employee.id.ToString());

                    return message;
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
        public HttpResponseMessage Put(int id, [FromBody] employee employee)
        {
            try
            {
                using (EmployeeDBContext dbContext = new EmployeeDBContext())
                {
                    var entity = dbContext.employees.FirstOrDefault(e => e.id == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                            "Employee with Id " + id.ToString() + " not found to update");
                    }
                    else
                    {
               
                        entity.name = employee.name;
                        entity.dept_name = employee.dept_name;
                        entity.email = employee.email;
                        entity.salary = employee.salary;
                        entity.isactive = employee.isactive;



                        dbContext.SaveChanges();

                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                using (EmployeeDBContext dbContext = new EmployeeDBContext())
                {
                    var entity = dbContext.employees.FirstOrDefault(e => e.id == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                            "Employee with Id = " + id.ToString() + " not found to delete");
                    }
                    else
                    {
                        dbContext.employees.Remove(entity);
                        dbContext.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
        public HttpResponseMessage Delete()
        {
            try
            {
                using (EmployeeDBContext dbContext = new EmployeeDBContext())
                {
                    // Drop the table using DbContext.Database.ExecuteSqlCommand
                    dbContext.Database.ExecuteSqlCommand("DROP TABLE employee");

                    return Request.CreateResponse(HttpStatusCode.OK, "Table dropped successfully");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

    }
}
