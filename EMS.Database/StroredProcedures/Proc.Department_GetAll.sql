CREATE PROCEDURE [dbo].[Department_GetAll]
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT 
		Id,
		Code,
		Name,
		Active
	FROM dbo.Department
END
GO