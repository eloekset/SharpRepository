using SharpRepository.ManualRuntimeTests.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpRepository.ManualRuntimeTests
{
    public class Program
    {
        public static void Main(string[] args)
        {
            InMemoryRepository.InMemoryRepository<Employee> employeeRepo = new InMemoryRepository.InMemoryRepository<Employee>();
            employeeRepo.Add(new Employee { EmployeeId = 1, Name = "Adam" });
            employeeRepo.Add(new Employee { EmployeeId = 2, Name = "Ashley" });

            foreach(var employee in employeeRepo.GetAll())
            {
                Console.WriteLine($"Employee {employee.EmployeeId} is called {employee.Name}.");
            }

            Console.ReadKey();
        }
    }
}
