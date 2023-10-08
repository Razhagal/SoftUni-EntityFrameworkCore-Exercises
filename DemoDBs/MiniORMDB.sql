CREATE DATABASE MiniORM
GO
USE MiniORM
GO

CREATE TABLE Projects
(
	Id INT IDENTITY PRIMARY KEY,
	Name VARCHAR(50) NOT NULL
)

CREATE TABLE Departments
(
	Id INT PRIMARY KEY IDENTITY,
	Name VARCHAR(50) NOT NULL
)

CREATE TABLE Employees
(
	Id INT PRIMARY KEY IDENTITY,
	FirstName VARCHAR(50) NOT NULL,
	MiddleName VARCHAR(50),
	LastName VARCHAR(50) NOT NULL,
	IsEmployed BIT NOT NULL,
	DepartmentId INT,
	CONSTRAINT FK_Employees_Departments FOREIGN KEY (DepartmentId) REFERENCES Departments(Id)
)

CREATE TABLE EmployeesProjects
(
	ProjectId INT NOT NULL,
	EmployeeId INT NOT NULL,
	CONSTRAINT PK_Projects_Employees PRIMARY KEY (ProjectId, EmployeeId),
	CONSTRAINT FK_EmployeesProjects_Projects FOREIGN KEY (ProjectId) REFERENCES Projects(Id),
	CONSTRAINT FK_EmployeesProjects_Employees FOREIGN KEY (EmployeeId) REFERENCES Employees(Id)
)
GO

INSERT INTO MiniORM.dbo.Departments (Name) VALUES ('Research');
INSERT INTO MiniORM.dbo.Employees (FirstName, MiddleName, LastName, IsEmployed, DepartmentId) VALUES
('Stamat', NULL, 'Ivanov', 1, 1),
('Petar', 'Ivanov', 'Petrov', 0, 1),
('Ivan', 'Petrov', 'Georgiev', 1, 1),
('Gosho', NULL, 'Ivanov', 1, 1);
INSERT INTO MiniORM.dbo.Projects (Name) 
VALUES ('C# Project'), ('Java Project');
INSERT INTO MiniORM.dbo.EmployeesProjects (ProjectId, EmployeeId) VALUES
(1, 1),
(1, 3),
(2, 2),
(2, 3)