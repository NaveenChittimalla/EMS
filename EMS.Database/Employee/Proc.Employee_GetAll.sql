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