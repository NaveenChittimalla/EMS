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

CREATE FUNCTION [dbo].[FN_Employee_Generate_Code]
(
	@EmployeeId int
)
RETURNS NVARCHAR(100)
AS
BEGIN
	RETURN CONCAT('EMS', @EmployeeId)
END
GO

CREATE PROCEDURE [dbo].[Employee_Insert]
    @FirstName NVARCHAR(250),
    @LastName NVARCHAR(250),
    @Email NVARCHAR(250),
    @Active BIT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Employee
    (
        FirstName,
        LastName,
        Email,
        Active
    )
    values
    (
        @FirstName,
        @LastName,
        @Email,
        @Active
    );

    DECLARE @NewEmployeeId INT
    SET @NewEmployeeId = @@IDENTITY
    
    UPDATE Employee
    SET EmployeeCode = [dbo].[FN_Employee_Generate_Code](@NewEmployeeId)
    FROM dbo.Employee Employee
    WHERE id = @NewEmployeeId;
    
    SELECT @NewEmployeeId
END
GO

CREATE PROCEDURE [dbo].[Employee_GetAll]
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT 
		Id,
		EmployeeCode,
		FirstName,
		LastName,
		Email,
		Active
	FROM dbo.Employee
END
GO

CREATE PROCEDURE [dbo].[Employee_GetById]
	@Id INT
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT 
		Id,
		EmployeeCode,
		FirstName,
		LastName,
		Email,
		Active
	FROM dbo.Employee
	WHERE Id = @Id
END
GO

CREATE PROCEDURE [dbo].[Employee_Update_Details] 
    @Id INT,
    @FirstName NVARCHAR(250),
    @LastName NVARCHAR(250),
    @Email NVARCHAR(550),
    @Active BIT
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

CREATE PROCEDURE [dbo].[Employee_DeleteById] @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Employee
    WHERE Id = @Id
END
GO