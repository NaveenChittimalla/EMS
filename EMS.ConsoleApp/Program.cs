using EMS.CoreLibrary.Models;

namespace EMS.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string projectTitle = "Employee Management System";
            Console.WriteLine("----------------------------");
            Console.WriteLine(projectTitle);
            Console.WriteLine("----------------------------");

            //Employee newEmployee1 = new Employee();
            //newEmployee1.EmployeeCode = "E";
            //newEmployee1.FirstName = "Test Emp 6";
            //newEmployee1.LastName = "Test Emp 6";
            //newEmployee1.Create();

            Console.Write("Enter EmployeeCode: ");
            string ECode = Console.ReadLine();

            Employee employee = new Employee();
            employee.EmployeeCode = ECode;
            employee.Get();

            Console.WriteLine("------- Employee Details -------");
            Console.Write(employee.EmployeeCode + "\t");
            Console.Write(employee.FirstName + "\t");
            Console.Write(employee.LastName + "\t");
            Console.Write(employee.Email + "\t");

            //Employee employee = new Employee();
            //List<Employee> employeeList = employee.GetAll();

            //Console.WriteLine("------- Employee Details -------");
            //foreach (Employee emp in employeeList)
            //{
            //    Console.Write(emp.EmployeeCode + "\t");
            //    Console.Write(emp.FirstName + "\t");
            //    Console.Write(emp.LastName + "\t");
            //    Console.Write(emp.Email + "\t");
            //    Console.WriteLine();
            //}

            Console.ReadLine();
        }
    }
}
