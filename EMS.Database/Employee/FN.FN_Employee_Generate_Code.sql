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
