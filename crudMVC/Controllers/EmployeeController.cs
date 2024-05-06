using crudMVC.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace crudMVC.Controllers
{
    public class EmployeeController : Controller
    {
        crudMVCEntities _context = new crudMVCEntities();

        // GET: Employee List
        public ActionResult Index()
        {
            var employeeList = _context.Employees.ToList();
            return View(employeeList);
        }

        //Employee Register
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                //check if employee already exist
                if (_context.Employees.Any(e => e.EmployeeEmail == employee.EmployeeEmail))
                {
                    ModelState.AddModelError("EmployeeEmail", "Employee already registered");
                    return View(employee);
                }

                //check if employee is above 18
                if (DateTime.Today.AddYears(-18) < employee.DOB)
                {
                    ModelState.AddModelError("DOB", "Employee should be above 18 years");
                    return View(employee);
                }

                Employee obj = new Employee();

                obj.EmployeeName = employee.EmployeeName;
                obj.EmployeeEmail = employee.EmployeeEmail;
                obj.AadharCardNumber = employee.AadharCardNumber;
                obj.ContactNumber = employee.ContactNumber;
                obj.Department = employee.Department;
                obj.Designation = employee.Designation;
                obj.Address = employee.Address;
                obj.PinCode = employee.PinCode;
                obj.DOB = employee.DOB;

                _context.Employees.Add(obj);
                _context.SaveChanges();
            }

            ModelState.Clear();
            return RedirectToAction("Index");
        }

        //Employee Edit
        public ActionResult Edit(int employeeId)
        {
            var existEmployee = _context.Employees.Find(employeeId);
            if (existEmployee == null)
            {
                return HttpNotFound();
            }

            return View(existEmployee);
        }

        [HttpPost]
        public ActionResult Edit(Employee employee) 
        {
            if (ModelState.IsValid)
            {
                //check if employee is above 18
                if (DateTime.Today.AddYears(-18) < employee.DOB)
                {
                    ModelState.AddModelError("DOB", "Employee should be above 18 years");
                    return View(employee);
                }

                _context.Entry(employee).State = EntityState.Modified;
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(employee);
        }

        //Employee Delete
        public ActionResult Delete (int employeeId)
        {
            var existEmployee = _context.Employees.Find(employeeId);
            if (existEmployee == null)
            {
                return HttpNotFound();
            }

            _context.Employees.Remove(existEmployee);
            _context.SaveChanges();

            var employeeList = _context.Employees.ToList();

            return View("Index", employeeList);
        }
    }
}