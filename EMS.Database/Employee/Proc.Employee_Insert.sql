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