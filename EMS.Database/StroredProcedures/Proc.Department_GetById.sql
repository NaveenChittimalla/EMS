CREATE PROCEDURE [dbo].[Department_GetById]
	@Id INT
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT 
		Id,
		Code,
		Name,
		Active
	FROM dbo.Department
	WHERE Id = @Id
END
GO