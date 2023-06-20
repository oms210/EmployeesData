# EmployeeData

This project is a C# application that performs calculations on employee data stored in a CSV file. It calculates the average, highest, and lowest salaries of employees, as well as generates reports based on department-wise statistics. The generated reports are stored in a CSV file.

## Getting Started

To run the application, follow these steps:

1. Clone the repository.
2. Open the solution in Visual Studio.
3. Build the solution to restore NuGet packages.
4. Set the `employees.csv` file path in the `Main` method of the `Program` class.
5. Run the application.

## Dependencies

The project depends on the following NuGet packages:

- `CsvHelper` for reading and writing CSV files.
- `Moq` for unit testing.

## Usage

The `Program` class contains the main logic of the application. It reads the employee data from the CSV file, performs calculations, generates reports, and saves them to a CSV file.

The key methods in the `Program` class are:

- `ReadCsvFileAsync` - Reads the employee data from the CSV file asynchronously.
- `CalculateAverageSalary` - Calculates the average salary of all employees.
- `CalculateHighestSalary` - Calculates the highest salary among all employees.
- `CalculateLowestSalary` - Calculates the lowest salary among all employees.
- `CalculateAverageSalaryByDepartment` - Calculates the average salary for each department.
- `GenerateDepartmentReports` - Generates reports containing total salary and employee count for each department.
- `GenerateCsvFile` - Saves the department reports to a CSV file.

Feel free to modify the code or integrate it into your own projects as needed.

## Unit Tests

The `EmployeeTests` class contains unit tests for the key methods in the `Program` class. It uses the `Moq` framework to create test doubles for the dependencies.

To run the unit tests, build the solution and run the tests using the test runner in Visual Studio.

## License

This code is for personal practice purposes only. It is not intended for distribution or commercial use.

