﻿CREATE TABLE [dbo].[Employee]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[EmployeeCode] NVARCHAR(100) NULL,
	[FirstName] NVARCHAR(250) NOT NULL,
	[LastName] NVARCHAR(250) NULL,
	[Email] NVARCHAR(250) NOT NULL,
	[Active] BIT DEFAULT(1)
);
