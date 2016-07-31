using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharpRepository.ManualRuntimeTests.Model;
using SharpRepository.Repository.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpRepository.ManualRuntimeTests
{
    public class Program
    {
        private static IConfigurationRoot Configuration;
        private static SharpRepositoryConfiguration sharpRepository;
        private static SharpRepositoryConfiguration sharpRepository2;

        public static void Main(string[] args)
        {
            LoadConfigurationObjectStyle();
            //LoadConfigurationArrayStyle();
        }

        private static void LoadConfigurationObjectStyle()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("objectsharprepositorysettings.json");

            Configuration = builder.Build();
            sharpRepository = new SharpRepositoryConfiguration();
            var sharpRepositoryConfigurationSection = Configuration.GetSection("sharpRepository");
            ConfigurationBinder.Bind(sharpRepositoryConfigurationSection, sharpRepository);
            sharpRepository2 = new SharpRepositoryConfiguration();
            var sharpRepository2ConfigurationSection = Configuration.GetSection("sharpRepository2");
            ConfigurationBinder.Bind(sharpRepository2ConfigurationSection, sharpRepository2);
        }

        private static void LoadConfigurationArrayStyle()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("arraysharprepositorysettings.json");

            Configuration = builder.Build();
            sharpRepository = new SharpRepositoryConfiguration();
            var sharpRepositoryConfigurationSection = Configuration.GetSection("sharpRepository");
            ConfigurationBinder.Bind(sharpRepositoryConfigurationSection, sharpRepository);
            sharpRepository2 = new SharpRepositoryConfiguration();
            var sharpRepository2ConfigurationSection = Configuration.GetSection("sharpRepository2");
            ConfigurationBinder.Bind(sharpRepository2ConfigurationSection, sharpRepository2);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // TODO: Dig into ASP.NET Core to see how this method gets called.
        }

        private static void RunSimpleInMemoryTest()
        {
            InMemoryRepository.InMemoryRepository<Employee> employeeRepo = new InMemoryRepository.InMemoryRepository<Employee>();
            employeeRepo.Add(new Employee { EmployeeId = 1, Name = "Adam" });
            employeeRepo.Add(new Employee { EmployeeId = 2, Name = "Ashley" });

            foreach (var employee in employeeRepo.GetAll())
            {
                Console.WriteLine($"Employee {employee.EmployeeId} is called {employee.Name}.");
            }

            Console.ReadKey();
        }
    }
}
