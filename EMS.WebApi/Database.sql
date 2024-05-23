CREATE Database EMS
GO

CREATE TABLE [dbo].[Employee] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [EmployeeCode] NVARCHAR (250) NULL,
    [FirstName]    NVARCHAR (250) NOT NULL,
    [LastName]     NVARCHAR (250) NOT NULL,
    [Email]        NVARCHAR (500) NOT NULL,
    [Active]       BIT            NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO

CREATE PROCEDURE dbo.Employee_Update_Details (
    @Id INT,
    @FirstName NVARCHAR(250),
    @LastName NVARCHAR(250),
    @Email NVARCHAR(550),
    @Active BIT
)
AS
BEGIN
  SET NOCOUNT ON;

  UPDATE Employee
  SET FirstName = @FirstName,
      LastName = @LastName,
      Email = @Email,
      Active = @Active
  FROM dbo.Employee Employee
  WHERE Id = @Id

END
GO
