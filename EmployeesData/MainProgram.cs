using CsvHelper;
using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace EmployeeData
{
    public class Program
    {
       public static async Task Main(string[] args)
        {
            // Read the CSV file and store the data in memory in parallel
            ConcurrentBag<Employee> employees = await ReadCsvFileAsync("employees.csv");

            // Calculate the average salary of all employees in parallel
            decimal averageSalary = CalculateAverageSalary(employees);
            Console.WriteLine($"Average Salary: {averageSalary:C}");

            // Calculate the highest and lowest salary of all employees in parallel
            decimal highestSalary = CalculateHighestSalary(employees);
            Console.WriteLine($"Highest Salary: {highestSalary:C}");

            decimal lowestSalary = CalculateLowestSalary(employees);
            Console.WriteLine($"Lowest Salary: {lowestSalary:C}");

            // Calculate the average salary of employees for each department in parallel
            var departmentAverages = CalculateAverageSalaryByDepartment(employees);
            foreach (var departmentAverage in departmentAverages)
            {
                Console.WriteLine($"Average Salary for {departmentAverage.Department}: {departmentAverage.AverageSalary:C}");
            }

            // Generate a report showing the total salary and number of employees for each department in parallel
            var departmentReports = GenerateDepartmentReports(employees);
            foreach (var departmentReport in departmentReports)
            {
                Console.WriteLine($"Department: {departmentReport.Department}, Total Salary: {departmentReport.TotalSalary:C}, Employee Count: {departmentReport.EmployeeCount}");
            }
            string csvFilePath = "department_reports.csv";
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string fullPath = Path.GetFullPath(Path.Combine(currentDirectory, csvFilePath));
            // Generate the CSV file

            await GenerateCsvFile(departmentReports, fullPath);
            Console.WriteLine($"CSV file generated: {fullPath}");

            Process.Start(new ProcessStartInfo
            {
                FileName = Path.GetDirectoryName(fullPath),
                UseShellExecute = true,
                Verb = "open"
            });

            Console.ReadLine();
        }

      public  static async Task<ConcurrentBag<Employee>> ReadCsvFileAsync(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var employees = new ConcurrentBag<Employee>();
                await foreach (var record in csv.GetRecordsAsync<Employee>())
                {
                    employees.Add(record);
                }
                return employees;
            }
        }

        public static decimal CalculateAverageSalary(ConcurrentBag<Employee> employees)
        {
            var tasks = employees.AsParallel().Select(e => e.Salary).ToList();
            Task.WaitAll(tasks.Select(t => Task.Run(() => t)).ToArray());
            return tasks.Average();
        }

        public static decimal CalculateHighestSalary(ConcurrentBag<Employee> employees)
        {
            var tasks = employees.AsParallel().Select(e => e.Salary).ToList();
            Task.WaitAll(tasks.Select(t => Task.Run(() => t)).ToArray());
            return tasks.Max();
        }

       public static decimal CalculateLowestSalary(ConcurrentBag<Employee> employees)
        {
            var tasks = employees.AsParallel().Select(e => e.Salary).ToList();
            Task.WaitAll(tasks.Select(t => Task.Run(() => t)).ToArray());
            return tasks.Min();
        }

        public static List<DepartmentAverage> CalculateAverageSalaryByDepartment(ConcurrentBag<Employee> employees)
        {
            var departmentAverages = employees.AsParallel()
                .GroupBy(e => e.Department)
                .Select(g => new DepartmentAverage
                {
                    Department = g.Key,
                    AverageSalary = g.Average(e => e.Salary)
                })
                .ToList();

            return departmentAverages;
        }

        public  static List<DepartmentReport> GenerateDepartmentReports(ConcurrentBag<Employee> employees)
        {

            var departmentReports = employees.AsParallel()
                .GroupBy(e => e.Department)
                .Select(g => new DepartmentReport
                {
                    Department = g.Key,
                    EmployeeCount = g.Count(),
                    TotalSalary = g.Sum(e => e.Salary),
                    AverageSalary = g.Average(e => e.Salary),
                    MinSalary = g.Min(e => e.Salary),
                    MinSalaryEmployee = g.FirstOrDefault(e => e.Salary == g.Min(e => e.Salary))?.Name,
                    MaxSalary = g.Max(e => e.Salary),
                    MaxSalaryEmployee = g.FirstOrDefault(e => e.Salary == g.Max(e => e.Salary))?.Name
                })
                .ToList();
            return departmentReports;

        }

        public static async Task GenerateCsvFile(List<DepartmentReport> departmentReports, string filePath)
        {
            using (var writer = new StreamWriter(filePath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                await csv.WriteRecordsAsync(departmentReports);
            }
        }
    }

    public class Employee
    {
        [Name("name")]
        public string Name { get; set; }

        [Name("department")]
        public string Department { get; set; }

        [Name("salary")]
        public decimal Salary { get; set; }
    }

    public class DepartmentReport
    {
        public string Department { get; set; }
        public int EmployeeCount { get; set; }
        public decimal TotalSalary { get; set; }
        public decimal AverageSalary { get; set; }
        public decimal MinSalary { get; set; }
        public string? MinSalaryEmployee { get; set; }
        public decimal MaxSalary { get; set; }
        public string? MaxSalaryEmployee { get; set; }

    }

    public class DepartmentAverage
    {
        public string Department { get; set; }
        public decimal AverageSalary { get; set; }
    }
}