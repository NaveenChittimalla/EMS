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
