CREATE FUNCTION [dbo].[FN_Department_Generate_Code]
(
	@DepartmentId int
)
RETURNS NVARCHAR(100)
AS
BEGIN
	RETURN CONCAT('EMS_D', @DepartmentId)
END
GO
