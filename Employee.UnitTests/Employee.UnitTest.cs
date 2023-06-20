using Moq;
using System.Collections.Concurrent;
using System.Diagnostics;


namespace EmployeeData.UnitTests
{
    public class EmployeeTests
    {
        [Fact]
        public void CalculateAverageSalary_ShouldCalculateAverageSalaryOfEmployees()
        {
            // Arrange
            var employees = new ConcurrentBag<Employee>
            {
                new Employee { Name = "John Smith", Department = "IT", Salary = 5000 },
                new Employee { Name = "Jane Doe", Department = "IT", Salary = 6000 },
                new Employee { Name = "Michael Johnson", Department = "HR", Salary = 7000 },
                new Employee { Name = "Mary Brown", Department = "HR", Salary = 8000 }
            };

            // Act
            decimal averageSalary = Program.CalculateAverageSalary(employees);

            // Assert
            Assert.Equal(6500, averageSalary);
        }

        [Fact]
        public void CalculateHighestSalary_ShouldCalculateHighestSalaryOfEmployees()
        {
            // Arrange
            var employees = new ConcurrentBag<Employee>
            {
                new Employee { Name = "John Smith", Department = "IT", Salary = 5000 },
                new Employee { Name = "Jane Doe", Department = "IT", Salary = 6000 },
                new Employee { Name = "Michael Johnson", Department = "HR", Salary = 7000 },
                new Employee { Name = "Mary Brown", Department = "HR", Salary = 8000 }
            };

            // Act
            decimal highestSalary = Program.CalculateHighestSalary(employees);

            // Assert
            Assert.Equal(8000, highestSalary);
        }

        [Fact]
        public void CalculateLowestSalary_ShouldCalculateLowestSalaryOfEmployees()
        {
            // Arrange
            var employees = new ConcurrentBag<Employee>
            {
                new Employee { Name = "John Smith", Department = "IT", Salary = 5000 },
                new Employee { Name = "Jane Doe", Department = "IT", Salary = 6000 },
                new Employee { Name = "Michael Johnson", Department = "HR", Salary = 7000 },
                new Employee { Name = "Mary Brown", Department = "HR", Salary = 8000 }
            };

            // Act
            decimal lowestSalary = Program.CalculateLowestSalary(employees);

            // Assert
            Assert.Equal(5000, lowestSalary);
        }

        [Fact]
        public void CalculateAverageSalaryByDepartment_ShouldCalculateAverageSalaryForEachDepartment()
        {
            // Arrange
            var employees = new ConcurrentBag<Employee>
            {
                new Employee { Name = "John Smith", Department = "IT", Salary = 5000 },
                new Employee { Name = "Jane Doe", Department = "IT", Salary = 6000 },
                new Employee { Name = "Michael Johnson", Department = "HR", Salary = 7000 },
                new Employee { Name = "Mary Brown", Department = "HR", Salary = 8000 }
            };

            // Act
            var departmentAverages = Program.CalculateAverageSalaryByDepartment(employees);

            // Assert
            Assert.Equal(2, departmentAverages.Count);

            var itDepartmentAverage = departmentAverages.FirstOrDefault(d => d.Department == "IT");
            Assert.NotNull(itDepartmentAverage);
            Assert.Equal(5500, itDepartmentAverage.AverageSalary);

            var hrDepartmentAverage = departmentAverages.FirstOrDefault(d => d.Department == "HR");
            Assert.NotNull(hrDepartmentAverage);
            Assert.Equal(7500, hrDepartmentAverage.AverageSalary);
        }

        [Fact]
        public void GenerateDepartmentReports_ShouldGenerateReportsWithTotalSalaryAndEmployeeCountForEachDepartment()
        {
            // Arrange
            var employees = new ConcurrentBag<Employee>
            {
                new Employee { Name = "John Smith", Department = "IT", Salary = 5000 },
                new Employee { Name = "Jane Doe", Department = "IT", Salary = 6000 },
                new Employee { Name = "Michael Johnson", Department = "HR", Salary = 7000 },
                new Employee { Name = "Mary Brown", Department = "HR", Salary = 8000 }
            };

            // Act
            var departmentReports = Program.GenerateDepartmentReports(employees);

            // Assert
            Assert.Equal(2, departmentReports.Count);

            var itDepartmentReport = departmentReports.FirstOrDefault(d => d.Department == "IT");
            Assert.NotNull(itDepartmentReport);
            Assert.Equal(2, itDepartmentReport.EmployeeCount);
            Assert.Equal(11000, itDepartmentReport.TotalSalary);
            Assert.Equal(5500, itDepartmentReport.AverageSalary);
            Assert.Equal(5000, itDepartmentReport.MinSalary);
            Assert.Equal("John Smith", itDepartmentReport.MinSalaryEmployee);
            Assert.Equal(6000, itDepartmentReport.MaxSalary);
            Assert.Equal("Jane Doe", itDepartmentReport.MaxSalaryEmployee);

            var hrDepartmentReport = departmentReports.FirstOrDefault(d => d.Department == "HR");
            Assert.NotNull(hrDepartmentReport);
            Assert.Equal(2, hrDepartmentReport.EmployeeCount);
            Assert.Equal(15000, hrDepartmentReport.TotalSalary);
            Assert.Equal(7500, hrDepartmentReport.AverageSalary);
            Assert.Equal(7000, hrDepartmentReport.MinSalary);
            Assert.Equal("Michael Johnson", hrDepartmentReport.MinSalaryEmployee);
            Assert.Equal(8000, hrDepartmentReport.MaxSalary);
            Assert.Equal("Mary Brown", hrDepartmentReport.MaxSalaryEmployee);
        }
        [Fact]
        public void ReadCsvFileAsync_ShouldReadCsvFileAndReturnEmployees()
        {
            // Arrange
            var filePath = "employees.csv";

            // Act
            var employees = Program.ReadCsvFileAsync(filePath).Result;

            // Assert
            Assert.NotNull(employees);
            Assert.NotEmpty(employees);
            // Add additional assertions based on the expected content of the CSV file
        }

        [Fact]
        public void GenerateCsvFile_ShouldGenerateCsvFileWithDepartmentReports()
        {
            // Arrange
            var departmentReports = new List<DepartmentReport>
            {
                new DepartmentReport { Department = "IT", EmployeeCount = 2, TotalSalary = 11000, AverageSalary = 5500 },
                new DepartmentReport { Department = "HR", EmployeeCount = 2, TotalSalary = 15000, AverageSalary = 7500 }
            };
            var filePath = "department_reports.csv";

            // Act
            Program.GenerateCsvFile(departmentReports, filePath).Wait();

            // Assert
            Assert.True(File.Exists(filePath));
            // Add additional assertions to validate the content of the generated CSV file
        }
       
    }
}
