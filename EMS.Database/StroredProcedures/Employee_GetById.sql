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